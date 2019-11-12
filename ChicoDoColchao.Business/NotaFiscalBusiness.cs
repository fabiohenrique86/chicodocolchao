using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Xml.Linq;

namespace ChicoDoColchao.Business
{
    public class NotaFiscalBusiness
    {
        NotaFiscalRepository notaFiscalRepository;
        ProdutoRepository produtoRepository;
        LojaRepository lojaRepository;
        LogRepository logRepository;

        public NotaFiscalBusiness()
        {
            notaFiscalRepository = new NotaFiscalRepository();
            produtoRepository = new ProdutoRepository();
            lojaRepository = new LojaRepository();
            logRepository = new LogRepository();
        }

        public List<NotaFiscalDao> Listar(NotaFiscalDao notaFiscalDao)
        {
            try
            {
                return notaFiscalRepository.Listar(notaFiscalDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public void ImportarXML(NotaFiscalDao notaFiscalDao, out List<string> mensagem, out int qtdNFeImportada)
        {
            try
            {
                qtdNFeImportada = 0;
                mensagem = new List<string>();

                var lojaDao = lojaRepository.Listar(new Loja() { Deposito = true, Ativo = true }).FirstOrDefault();

                // se não houver loja depósito, retorna
                if (lojaDao == null)
                {
                    mensagem.Add("Loja de depósito não cadastrada");
                    qtdNFeImportada = 0;
                    return;
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(10) }))
                {
                    XNamespace nsNFe = "http://www.portalfiscal.inf.br/nfe";

                    foreach (Stream item in notaFiscalDao.Arquivo)
                    {
                        var todosProdutoAtualizados = true;
                        string xmlString = string.Empty;

                        using (StreamReader sr = new StreamReader(item))
                            xmlString = sr.ReadToEnd();

                        if (string.IsNullOrEmpty(xmlString))
                        {
                            mensagem.Add("XML está vazio");
                            continue;
                        }

                        XDocument xml = XDocument.Parse(xmlString, LoadOptions.None);

                        IEnumerable<XElement> noNFe = xml.Descendants(nsNFe + "NFe");

                        if (!noNFe.Any())
                        {
                            mensagem.Add("XML não é uma NFe");
                            continue;
                        }

                        IEnumerable<XElement> NFe = xml.Descendants(nsNFe + "NFe");
                        IEnumerable<XElement> NFe_infNFe = NFe.Elements(nsNFe + "infNFe");
                        IEnumerable<XElement> NFe_infNFe_det = NFe_infNFe.Elements(nsNFe + "det");

                        if (NFe_infNFe_det == null || NFe_infNFe_det.Count() <= 0)
                        {
                            mensagem.Add("XML não tem produtos");
                            continue;
                        }

                        foreach (XElement det in NFe_infNFe_det)
                        {
                            XElement NFe_infNFe_det_prod = det.Elements(nsNFe + "prod").FirstOrDefault();

                            if (NFe_infNFe_det_prod == null)
                            {
                                mensagem.Add("Produto não encontrado no XML");
                                continue;
                            }

                            XElement NFe_infNFe_det_prod_cProd = NFe_infNFe_det_prod.Elements(nsNFe + "cProd").FirstOrDefault();
                            XElement NFe_infNFe_det_prod_qCom = NFe_infNFe_det_prod.Elements(nsNFe + "qCom").FirstOrDefault();

                            long cProd = 0;
                            short qCom = 0;

                            if (NFe_infNFe_det_prod_cProd != null)
                                cProd = Convert.ToInt64(NFe_infNFe_det_prod_cProd.Value);

                            if (NFe_infNFe_det_prod_qCom != null)
                                qCom = Convert.ToInt16(Math.Floor(Convert.ToDecimal(NFe_infNFe_det_prod_qCom.Value, new CultureInfo("en-US").NumberFormat)));

                            if (cProd <= 0)
                                mensagem.Add("Código do produto não encontrado no XML");

                            if (qCom <= 0)
                                mensagem.Add("Quantidade do produto não encontrado no XML");

                            if (cProd <= 0 || qCom <= 0)
                                continue;

                            // atualiza o estoque do produto
                            var atualizado = produtoRepository.Atualizar(lojaDao.LojaID, cProd, qCom);

                            if (!atualizado)
                            {
                                mensagem.Add(string.Format("Produto {0} não está cadastrado na loja do depósito", cProd));
                                todosProdutoAtualizados = false;
                            }
                        }

                        if (todosProdutoAtualizados)
                            qtdNFeImportada++;
                    }

                    scope.Complete();
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {

                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}

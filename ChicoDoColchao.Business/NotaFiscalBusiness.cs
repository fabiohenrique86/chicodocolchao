using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using System.Xml.Linq;
using System.IO;
using System.Globalization;

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
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public int ImportarXML(NotaFiscalDao notaFiscalDao)
        {
            int qtdNFeImportada = 0;

            try
            {
                var lojaDao = lojaRepository.Listar(new Loja() { Deposito = true }).FirstOrDefault();

                // se não houver loja depósito, retorna
                if (lojaDao == null) { return qtdNFeImportada; }

                XNamespace nsNFe = "http://www.portalfiscal.inf.br/nfe";

                foreach (Stream item in notaFiscalDao.Arquivo)
                {
                    string xmlString = string.Empty;

                    using (StreamReader sr = new StreamReader(item))
                    {
                        xmlString = sr.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(xmlString))
                    {
                        XDocument xml = XDocument.Parse(xmlString, LoadOptions.None);
                        
                        IEnumerable<XElement> noNFe = xml.Descendants(nsNFe + "NFe");

                        if (noNFe.Any())
                        {
                            IEnumerable<XElement> NFe = xml.Descendants(nsNFe + "NFe");
                            IEnumerable<XElement> NFe_infNFe = NFe.Elements(nsNFe + "infNFe");
                            IEnumerable<XElement> NFe_infNFe_det = NFe_infNFe.Elements(nsNFe + "det");

                            foreach (XElement det in NFe_infNFe_det)
                            {
                                XElement NFe_infNFe_det_prod = det.Elements(nsNFe + "prod").FirstOrDefault();

                                if (NFe_infNFe_det_prod != null)
                                {
                                    XElement NFe_infNFe_det_prod_cProd = NFe_infNFe_det_prod.Elements(nsNFe + "cProd").FirstOrDefault();
                                    XElement NFe_infNFe_det_prod_qCom = NFe_infNFe_det_prod.Elements(nsNFe + "qCom").FirstOrDefault();

                                    long cProd = 0;
                                    short qCom = 0;

                                    if (NFe_infNFe_det_prod_cProd != null) { cProd = Convert.ToInt64(NFe_infNFe_det_prod_cProd.Value); }
                                    if (NFe_infNFe_det_prod_qCom != null) { qCom = Convert.ToInt16(Math.Floor(Convert.ToDecimal(NFe_infNFe_det_prod_qCom.Value, new CultureInfo("en-US").NumberFormat))); }

                                    if (cProd > 0 && qCom > 0)
                                    {
                                        qtdNFeImportada += produtoRepository.Atualizar(lojaDao.LojaID, cProd, qCom);
                                    }
                                }
                            }
                        }
                    }
                }

                return qtdNFeImportada;
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }  
    }
}

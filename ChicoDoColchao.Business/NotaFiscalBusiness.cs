using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        CategoriaBusiness categoriaBusiness;
        MedidaBusiness medidaBusiness;
        NotaFiscalProdutoBusiness notaFiscalProdutoBusiness;

        public NotaFiscalBusiness()
        {
            notaFiscalRepository = new NotaFiscalRepository();
            produtoRepository = new ProdutoRepository();
            lojaRepository = new LojaRepository();
            logRepository = new LogRepository();
            categoriaBusiness = new CategoriaBusiness();
            medidaBusiness = new MedidaBusiness();
            notaFiscalProdutoBusiness = new NotaFiscalProdutoBusiness();
        }

        private void ValidarIncluir(NotaFiscalDao notaFiscalDao)
        {
            if (notaFiscalDao == null)
                throw new BusinessException("Nota Fiscal é obrigatório");

            if (notaFiscalDao.Numero <= 0)
                throw new BusinessException("Número é obrigatório");

            if (notaFiscalDao.LojaDao == null || notaFiscalDao.LojaDao.LojaID <= 0)
                throw new BusinessException("Loja é obrigatório");

            if (notaFiscalDao.DataEmissao == DateTime.MinValue)
                throw new BusinessException("Data Emissão é obrigatório");

            if (notaFiscalDao.DataCadastro == DateTime.MinValue)
                throw new BusinessException("Data Cadastro é obrigatório");

            var notaFiscal = Listar(new NotaFiscalDao() { Numero = notaFiscalDao.Numero }).FirstOrDefault();

            if (notaFiscal != null)
                throw new BusinessException(string.Format("Nota Fiscal {0} já cadastrada", notaFiscalDao.Numero));
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

        public int Incluir(NotaFiscalDao notaFiscalDao)
        {
            try
            {
                ValidarIncluir(notaFiscalDao);

                return notaFiscalRepository.Incluir(notaFiscalDao.ToBd());
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

        public void ImportarXML(NotaFiscalDao notaFiscalDao, out List<string> mensagemErro, out List<string> mensagemSucesso, out int qtdNFeImportada)
        {
            try
            {
                qtdNFeImportada = 0;
                mensagemErro = new List<string>();
                mensagemSucesso = new List<string>();

                var lojaDepositoDao = lojaRepository.Listar(new Loja() { Deposito = true, Ativo = true }).FirstOrDefault();

                // se não houver loja depósito, retorna
                if (lojaDepositoDao == null)
                {
                    mensagemErro.Add("Loja de depósito não cadastrada");
                    qtdNFeImportada = 0;
                    return;
                }

                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(10) }))
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
                            mensagemErro.Add("XML está vazio");
                            continue;
                        }

                        XDocument xml = XDocument.Parse(xmlString, LoadOptions.None);

                        IEnumerable<XElement> noNFe = xml.Descendants(nsNFe + "NFe");

                        if (!noNFe.Any())
                        {
                            mensagemErro.Add("XML não é uma NFe (NFe)");
                            continue;
                        }

                        IEnumerable<XElement> NFe = xml.Descendants(nsNFe + "NFe");
                        IEnumerable<XElement> NFe_infNFe = NFe.Elements(nsNFe + "infNFe");
                        IEnumerable<XElement> NFe_infNFe_det = NFe_infNFe.Elements(nsNFe + "det");
                        IEnumerable<XElement> NFe_infNFe_ide = NFe_infNFe.Elements(nsNFe + "ide");

                        if (NFe_infNFe_det == null || NFe_infNFe_det.Count() <= 0)
                        {
                            mensagemErro.Add("XML não tem produtos (det)");
                            continue;
                        }

                        if (NFe_infNFe_ide == null)
                        {
                            mensagemErro.Add("XML não tem identificação (ide)");
                            continue;
                        }

                        var NFe_infNFe_nNF = NFe_infNFe_ide.Elements(nsNFe + "nNF").FirstOrDefault();
                        if (NFe_infNFe_nNF == null)
                        {
                            mensagemErro.Add("XML não tem número (nNF)");
                            continue;
                        }

                        int numeroNfe;
                        int.TryParse(NFe_infNFe_nNF.Value, out numeroNfe);

                        if (numeroNfe <= 0)
                        {
                            mensagemErro.Add("XML tem número inválido (nNF)");
                            continue;
                        }

                        var NFe_infNFe_dhEmi = NFe_infNFe_ide.Elements(nsNFe + "dhEmi").FirstOrDefault();
                        if (NFe_infNFe_dhEmi == null)
                        {
                            mensagemErro.Add("XML não tem data de emissão (dhEmi)");
                            continue;
                        }

                        DateTime dataEmissaoNfe;
                        DateTime.TryParse(NFe_infNFe_dhEmi.Value, out dataEmissaoNfe);

                        if (dataEmissaoNfe == DateTime.MinValue)
                        {
                            mensagemErro.Add("XML tem data de emissão inválida (dhEmi)");
                            continue;
                        }

                        var notafiscalDao = new NotaFiscalDao() { Numero = numeroNfe, LojaDao = new LojaDao() { LojaID = lojaDepositoDao.LojaID }, DataEmissao = dataEmissaoNfe, DataCadastro = DateTime.Now };
                        var notaFiscalId = Incluir(notafiscalDao);

                        foreach (XElement det in NFe_infNFe_det)
                        {
                            XElement NFe_infNFe_det_prod = det.Elements(nsNFe + "prod").FirstOrDefault();

                            if (NFe_infNFe_det_prod == null)
                            {
                                mensagemErro.Add("Produto não encontrado no XML (prod)");
                                continue;
                            }

                            XElement NFe_infNFe_det_prod_cProd = NFe_infNFe_det_prod.Elements(nsNFe + "cProd").FirstOrDefault();
                            XElement NFe_infNFe_det_prod_qCom = NFe_infNFe_det_prod.Elements(nsNFe + "qCom").FirstOrDefault();
                            XElement NFe_infNFe_det_prod_xProd = NFe_infNFe_det_prod.Elements(nsNFe + "xProd").FirstOrDefault();
                            XElement NFe_infNFe_det_prod_vProd = NFe_infNFe_det_prod.Elements(nsNFe + "vProd").FirstOrDefault();

                            long cProd = 0;
                            short qCom = 0;
                            double vProd = 0;

                            if (NFe_infNFe_det_prod_cProd != null)
                                cProd = Convert.ToInt64(NFe_infNFe_det_prod_cProd.Value);

                            if (NFe_infNFe_det_prod_qCom != null)
                                qCom = Convert.ToInt16(Math.Floor(Convert.ToDecimal(NFe_infNFe_det_prod_qCom.Value, new CultureInfo("en-US").NumberFormat)));

                            if (NFe_infNFe_det_prod_vProd != null)
                                vProd = Convert.ToDouble(NFe_infNFe_det_prod_vProd.Value, new CultureInfo("en-US").NumberFormat);

                            if (cProd <= 0)
                                mensagemErro.Add("Código do produto não encontrado no XML");

                            if (qCom <= 0)
                                mensagemErro.Add("Quantidade do produto não encontrado no XML");

                            if (cProd <= 0 || qCom <= 0)
                                continue;

                            var produtoExisteNaLojaDeposito = produtoRepository.ListarEmLoja(0, cProd, 0, lojaDepositoDao.LojaID);
                            var atualizado = false;

                            if (produtoExisteNaLojaDeposito != null && produtoExisteNaLojaDeposito.ProdutoID > 0)
                            {
                                atualizado = produtoRepository.Atualizar(lojaDepositoDao.LojaID, cProd, qCom);

                                if (atualizado)
                                    mensagemSucesso.Add(string.Format("Produto {0}: Atualizado a quantidade do estoque do depósito em {1}", cProd, qCom));
                            }
                            else
                            {
                                var produtoDao = new ProdutoDao() { Numero = cProd, Quantidade = qCom, Ativo = true, Preco = vProd };

                                // encontra a medida dentro do campo xProd
                                var regexMedida = Regex.Match(NFe_infNFe_det_prod_xProd.Value, @"[0-9]?[0-9]?[0-9]?\s[X-x]?\s[0-9]?[0-9]?[0-9]?\s[X-x]?\s[0-9]?[0-9]?[0-9]");
                                var medidaDao = new MedidaDao() { Descricao = regexMedida.Value.Replace(" ", "") };

                                // se encontrar a medida na base de dados, associa ao produto. Caso contrário, cadastra-a
                                var medida = medidaBusiness.Listar(medidaDao).FirstOrDefault();
                                if (medida == null)
                                {
                                    int medidaId = medidaBusiness.Incluir(medidaDao);

                                    produtoDao.MedidaDao.MedidaID = medidaId;
                                    produtoDao.MedidaDao.Ativo = true;
                                }
                                else
                                {
                                    produtoDao.MedidaDao = medida;
                                }

                                // encontra a categoria dentro do campo xProd
                                var regexCategoria = Regex.Match(NFe_infNFe_det_prod_xProd.Value, @"^[^\s]+");
                                var descricaoCategoria = regexCategoria.Value;

                                if (descricaoCategoria == "BAU")
                                    descricaoCategoria = "BAÚ";
                                else if (descricaoCategoria == "COLCHAO")
                                    descricaoCategoria = "COLCHÃO";
                                else if (descricaoCategoria == "ORTOPEDICO")
                                    descricaoCategoria = "ORTOPÉDICO";
                                else if (descricaoCategoria == "SOFA")
                                    descricaoCategoria = "SOFÁ";
                                else if (descricaoCategoria == "COLCHAO")
                                    descricaoCategoria = "COLCHÃO";

                                var categoriaDao = categoriaBusiness.Listar(new CategoriaDao() { Descricao = descricaoCategoria }).FirstOrDefault();

                                // se encontrar a categoria na base de dados, associa ao produto. Caso contrário, cadastra-a
                                if (categoriaDao == null)
                                {
                                    var categoriaId = categoriaBusiness.Incluir(categoriaDao);

                                    produtoDao.CategoriaDao.FirstOrDefault().CategoriaID = categoriaId;
                                    produtoDao.CategoriaDao.FirstOrDefault().Ativo = true;
                                }
                                else
                                {
                                    produtoDao.CategoriaDao.Clear();
                                    produtoDao.CategoriaDao.Add(categoriaDao);
                                }

                                produtoDao.Descricao = NFe_infNFe_det_prod_xProd.Value.Substring(0, regexMedida.Index).Trim();

                                // cadastra o produto
                                var produtoId = produtoRepository.Incluir(produtoDao.ToBd());

                                // atualiza a quantidade desse produto na loja do depósito 
                                produtoRepository.Atualizar(lojaDepositoDao.LojaID, cProd, qCom);

                                notaFiscalProdutoBusiness.Incluir(new NotaFiscalProdutoDao() { NotaFiscalID = notaFiscalId, ProdutoDao = new ProdutoDao() { ProdutoID = produtoId }, Quantidade = qCom });

                                mensagemSucesso.Add(string.Format("Produto {0}: Cadastrado em todas as lojas e atualizado a quantidade do estoque do depósito em {1}", cProd, qCom));
                            }
                        }

                        if (todosProdutoAtualizados)
                            qtdNFeImportada++;
                    }

                    scope.Complete();
                }

                mensagemSucesso.Add(string.Format("{0} notas fiscais importadas com sucesso", qtdNFeImportada));
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

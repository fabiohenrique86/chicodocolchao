using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class MovimentoDeCaixaController : BaseController
    {
        private LojaBusiness lojaBusiness;
        private PedidoBusiness pedidoBusiness;
        private MovimentoCaixaBusiness movimentoCaixaBusiness;

        public MovimentoDeCaixaController()
        {
            lojaBusiness = new LojaBusiness();
            pedidoBusiness = new PedidoBusiness();
            movimentoCaixaBusiness = new MovimentoCaixaBusiness();
        }

        public ActionResult Index()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var lojasDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            return View(lojasDao);
        }

        public JsonResult Listar(string dataMovimento, int lojaId = 0)
        {
            var lista = new List<MovimentoCaixaDao>();

            try
            {
                DateTime dtMovimento;
                if (!DateTime.TryParse(dataMovimento, out dtMovimento) && !string.IsNullOrEmpty(dataMovimento))
                {
                    return Json(new { Sucesso = false, Mensagem = $"Data inválida {dataMovimento}" }, JsonRequestBehavior.AllowGet);
                }

                lista = movimentoCaixaBusiness.Listar(new MovimentoCaixaDao() { DataMovimento = dtMovimento, LojaDao = new LojaDao() { LojaID = lojaId } });

                return new JsonResult { Data = new { Sucesso = true, Mensagem = string.Empty, Lista = lista }, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Gerar(string dataMovimento = null, int lojaId = 0, string nomeFantasia = null)
        {
            try
            {
                var lista = new List<MovimentoCaixaDao>();

                if (string.IsNullOrEmpty(dataMovimento) || lojaId <= 0)
                {
                    return Json(new { Sucesso = false, Mensagem = "Informe Data do Movimento e Loja" }, JsonRequestBehavior.AllowGet);
                }

                DateTime dtMovimento;
                if (!DateTime.TryParse(dataMovimento, out dtMovimento))
                {
                    return Json(new { Sucesso = false, Mensagem = $"Data do Movimento inválida {dataMovimento}" }, JsonRequestBehavior.AllowGet);
                }

                var pedidosDao = pedidoBusiness.Listar(new PedidoDao()
                {
                    DataPedido = dtMovimento,
                    LojaDao = new List<LojaDao>() { new LojaDao() { LojaID = lojaId } }
                }, false, 0).Where(x => x.PedidoStatusDao.FirstOrDefault().PedidoStatusID != PedidoStatusDao.EPedidoStatus.Cancelado.GetHashCode()).ToList();

                if (pedidosDao == null || pedidosDao.Count() <= 0)
                {
                    return Json(new { Sucesso = false, Mensagem = $"Não existe Movimento de Caixa na data {dataMovimento} para loja {nomeFantasia}" }, JsonRequestBehavior.AllowGet);
                }

                var valor = pedidosDao.Sum(x => x.PedidoTipoPagamentoDao.Sum(w => w.ValorPago));

                dtMovimento = dtMovimento.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);

                movimentoCaixaBusiness.Incluir(new MovimentoCaixaDao() { DataMovimento = dtMovimento, LojaDao = new LojaDao() { LojaID = lojaId }, Valor = valor, MovimentoCaixaStatusDao = new MovimentoCaixaStatusDao() { MovimentoCaixaStatusID = (int)MovimentoCaixaStatusDao.EStatus.Gerado } });

                lista = movimentoCaixaBusiness.Listar(new MovimentoCaixaDao() { DataMovimento = dtMovimento });

                return Json(new { Sucesso = true, Mensagem = "Movimento de Caixa gerado com sucesso", Lista = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Relatorio(string dataMovimento = null, int lojaId = 0, string nomeFantasia = null)
        {
            try
            {
                if (string.IsNullOrEmpty(dataMovimento) || lojaId <= 0)
                    return Json(new { Sucesso = false, Mensagem = "Informe Data do Movimento e Loja" }, JsonRequestBehavior.AllowGet);

                DateTime dtMovimento;
                if (!DateTime.TryParse(dataMovimento, out dtMovimento))
                    return Json(new { Sucesso = false, Mensagem = $"Data do Movimento inválida {dataMovimento}" }, JsonRequestBehavior.AllowGet);

                var pedidosDao = pedidoBusiness.Listar(new PedidoDao()
                {
                    DataPedido = dtMovimento,
                    LojaDao = new List<LojaDao>() { new LojaDao() { LojaID = lojaId } }
                }, false, 0).Where(x => x.PedidoStatusDao.FirstOrDefault().PedidoStatusID != PedidoStatusDao.EPedidoStatus.Cancelado.GetHashCode()).ToList();

                if (pedidosDao == null || pedidosDao.Count() <= 0)
                    return Json(new { Sucesso = false, Mensagem = $"Não existe Movimento de Caixa na data {dataMovimento} para loja {nomeFantasia}" }, JsonRequestBehavior.AllowGet);

                var movimentoCaixa = movimentoCaixaBusiness.Listar(new MovimentoCaixaDao() { DataMovimento = dtMovimento, LojaDao = new LojaDao() { LojaID = lojaId } }).FirstOrDefault();

                if (movimentoCaixa == null)
                    return Json(new { Sucesso = false, Mensagem = $"Não existe Movimento de Caixa na data {dataMovimento} para loja {nomeFantasia}" }, JsonRequestBehavior.AllowGet);

                Warning[] warnings;
                string mimeType;
                string[] streamids;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_movimento_de_caixa_{0}.pdf", pedidosDao.FirstOrDefault().DataPedido.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/MovimentoDeCaixa.rdlc");

                double dinheiro = 0,
                cartaoBancoDoBrasilMasterCard = 0,
                cartaoBancoDoBrasilVisa = 0,
                cheque = 0,
                cartaoBancoDoBrasilElo = 0,
                cartaoBancoDoBrasilHiperCard = 0,
                cartaoBancoDoBrasilAmericanExpress = 0,
                crediario = 0,
                cartaoCaixaEconomicaVisa = 0,
                cartaoItauVisa = 0,
                cartaoBradescoVisa = 0,
                cartaoSantanderVisa = 0,
                cartaoCaixaEconomicaMasterCard = 0,
                cartaoItauMasterCard = 0,
                cartaoBradescoMasterCard = 0,
                cartaoSantanderMasterCard = 0,
                cartaoCaixaEconomicaElo = 0,
                cartaoItauElo = 0,
                cartaoBradescoElo = 0,
                cartaoSantanderElo = 0,
                cartaoCaixaEconomicaHiperCard = 0,
                cartaoItauHiperCard = 0,
                cartaoBradescoHiperCard = 0,
                cartaoSantanderHiperCard = 0,
                cartaoCaixaEconomicaAmericanExpress = 0,
                cartaoItauAmericanExpress = 0,
                cartaoBradescoAmericanExpress = 0,
                cartaoSantanderAmericanExpress = 0,
                cartaoOutros = 0,
                totalRecebido = 0,
                totalFrete = 0;

                foreach (var item in pedidosDao)
                {
                    dinheiro += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Dinheiro.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoBancoDoBrasilVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBancoDoBrasilVisa.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoCaixaEconomicaVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoCaixaEconomicaVisa.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoItauVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoItauVisa.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoBradescoVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBradescoVisa.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoSantanderVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoSantanderVisa.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoBancoDoBrasilMasterCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBancoDoBrasilMasterCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoCaixaEconomicaMasterCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoCaixaEconomicaMasterCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoItauMasterCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoItauMasterCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoBradescoMasterCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBradescoMasterCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoSantanderMasterCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoSantanderMasterCard.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoBancoDoBrasilElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBancoDoBrasilElo.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoCaixaEconomicaElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoCaixaEconomicaElo.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoItauElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoItauElo.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoBradescoElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBradescoElo.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoSantanderElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoSantanderElo.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoBancoDoBrasilHiperCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBancoDoBrasilHiperCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoCaixaEconomicaHiperCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoCaixaEconomicaHiperCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoItauHiperCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoItauHiperCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoBradescoHiperCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBradescoHiperCard.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoSantanderHiperCard += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoSantanderHiperCard.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoBancoDoBrasilAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBancoDoBrasilAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoCaixaEconomicaAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoCaixaEconomicaAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoItauAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoItauAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoBradescoAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoBradescoAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);
                    cartaoSantanderAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoSantanderAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);

                    cartaoOutros += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoOutros.GetHashCode()).Sum(x => x.ValorPago);

                    cheque += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Cheque.GetHashCode()).Sum(x => x.ValorPago);
                    crediario += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Crediario.GetHashCode()).Sum(x => x.ValorPago);

                    totalFrete += item.ValorFrete.GetValueOrDefault();
                }

                totalRecebido = dinheiro + cartaoBancoDoBrasilMasterCard + cartaoBancoDoBrasilVisa + cheque + cartaoBancoDoBrasilElo +
                                cartaoBancoDoBrasilHiperCard + cartaoBancoDoBrasilAmericanExpress + crediario + cartaoCaixaEconomicaVisa + cartaoItauVisa +
                                cartaoBradescoVisa + cartaoSantanderVisa + cartaoCaixaEconomicaMasterCard + cartaoItauMasterCard + cartaoBradescoMasterCard +
                                cartaoSantanderMasterCard + cartaoCaixaEconomicaElo + cartaoItauElo + cartaoBradescoElo + cartaoSantanderElo + cartaoCaixaEconomicaHiperCard +
                                cartaoItauHiperCard + cartaoBradescoHiperCard + cartaoSantanderHiperCard + cartaoCaixaEconomicaAmericanExpress + cartaoItauAmericanExpress +
                                cartaoBradescoAmericanExpress + cartaoSantanderAmericanExpress + cartaoOutros + totalFrete;

                // parâmetros
                var parametros = new List<ReportParameter>();

                parametros.Add(new ReportParameter("Cnpj", pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().Cnpj));
                parametros.Add(new ReportParameter("Data", pedidosDao.FirstOrDefault().DataPedido.ToString("dd/MM/yyyy")));
                parametros.Add(new ReportParameter("Dinheiro", dinheiro.ToString()));
                parametros.Add(new ReportParameter("CartaoVisa", (cartaoBancoDoBrasilVisa + cartaoCaixaEconomicaVisa + cartaoItauVisa + cartaoBradescoVisa + cartaoSantanderVisa).ToString()));
                parametros.Add(new ReportParameter("CartaoMaster", (cartaoBancoDoBrasilMasterCard + cartaoCaixaEconomicaMasterCard + cartaoItauMasterCard + cartaoBradescoMasterCard + cartaoSantanderMasterCard).ToString()));
                parametros.Add(new ReportParameter("Cheque", cheque.ToString()));
                parametros.Add(new ReportParameter("TotalRecebido", totalRecebido.ToString()));
                parametros.Add(new ReportParameter("CartaoElo", (cartaoBancoDoBrasilElo + cartaoCaixaEconomicaElo + cartaoItauElo + cartaoBradescoElo + cartaoSantanderElo).ToString()));
                parametros.Add(new ReportParameter("CartaoHiper", (cartaoBancoDoBrasilHiperCard + cartaoCaixaEconomicaHiperCard + cartaoItauHiperCard + cartaoBradescoHiperCard + cartaoSantanderHiperCard).ToString()));
                parametros.Add(new ReportParameter("CartaoAmericanExpress", (cartaoBancoDoBrasilAmericanExpress + cartaoCaixaEconomicaAmericanExpress + cartaoItauAmericanExpress + cartaoBradescoAmericanExpress + cartaoSantanderAmericanExpress).ToString()));
                parametros.Add(new ReportParameter("Crediario", crediario.ToString()));
                parametros.Add(new ReportParameter("TotalFrete", totalFrete.ToString()));
                parametros.Add(new ReportParameter("RazaoSocial", pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().RazaoSocial ?? string.Empty));
                parametros.Add(new ReportParameter("NomeFantasia", pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().NomeFantasia ?? string.Empty));
                parametros.Add(new ReportParameter("CartaoOutros", cartaoOutros.ToString()));
                parametros.Add(new ReportParameter("NumeroSequencial", movimentoCaixa.NumeroSequencial.GetValueOrDefault().ToString()));

                viewer.LocalReport.SetParameters(parametros);

                var pedidos = new List<dynamic>();
                var id = 1;

                foreach (var item in pedidosDao)
                {
                    pedidos.Add(new
                    {
                        ID = id,
                        PedidoID = item.PedidoID,
                        ValorPago = item.PedidoTipoPagamentoDao.Sum(x => x.ValorPago),
                        Forma = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.TipoPagamentoDao.Descricao)),
                        Prazo = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.ParcelaDao.Numero)),
                        Observacao = item.Observacao,
                        CV = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.CV)),
                        ValorFrete = item.ValorFrete
                    });

                    id++;
                }

                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_movimento_caixa", pedidos));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Receber(int movimentoCaixaId, int movimentoCaixaStatusId, int usuarioId)
        {
            try
            {
                movimentoCaixaBusiness.Receber(new MovimentoCaixaDao() { MovimentoCaixaID = movimentoCaixaId, MovimentoCaixaStatusDao = new MovimentoCaixaStatusDao() { MovimentoCaixaStatusID = movimentoCaixaStatusId }, DataRecebimento = DateTime.Now, UsuarioRecebimento = new UsuarioDao() { UsuarioID = usuarioId } });

                var lista = movimentoCaixaBusiness.Listar(new MovimentoCaixaDao() { MovimentoCaixaID = movimentoCaixaId }).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Recebimento confirmado com sucesso", Lista = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
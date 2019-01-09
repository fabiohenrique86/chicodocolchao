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

        public MovimentoDeCaixaController()
        {
            lojaBusiness = new LojaBusiness();
            pedidoBusiness = new PedidoBusiness();
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

        public JsonResult Gerar(string data = null, int lojaId = 0)
        {
            try
            {
                if (data == null || lojaId == 0)
                {
                    return Json(new { Sucesso = false, Mensagem = "Informe Data e/ou Loja" }, JsonRequestBehavior.AllowGet);
                }

                var pedidosDao = pedidoBusiness.Listar(new PedidoDao()
                {
                    DataPedido = Convert.ToDateTime(data),
                    LojaDao = new List<LojaDao>() { new LojaDao() { LojaID = lojaId } }
                }, false, 0).Where(x => x.PedidoStatusDao.FirstOrDefault().PedidoStatusID != PedidoStatusDao.EPedidoStatus.Cancelado.GetHashCode()).ToList();

                if (pedidosDao == null || pedidosDao.Count() <= 0)
                {
                    return Json(new { Sucesso = false, Mensagem = "Não existe Movimento de Caixa para data e/ou loja informados" }, JsonRequestBehavior.AllowGet);
                }

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
                                cartaoBradescoAmericanExpress + cartaoSantanderAmericanExpress + cartaoOutros;

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
                parametros.Add(new ReportParameter("RazaoSocial", pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().RazaoSocial ?? pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().NomeFantasia));

                viewer.LocalReport.SetParameters(parametros);

                // pedido
                var pedidos = new List<dynamic>();
                foreach (var item in pedidosDao)
                {
                    pedidos.Add(new
                    {
                        PedidoID = item.PedidoID,
                        ValorPago = item.PedidoTipoPagamentoDao.Sum(x => x.ValorPago),
                        Forma = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.TipoPagamentoDao.Descricao)),
                        Prazo = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.ParcelaDao.Numero)),
                        Observacao = item.Observacao,
                        CV = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.CV)),
                        ValorFrete = item.ValorFrete
                    });
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
    }
}
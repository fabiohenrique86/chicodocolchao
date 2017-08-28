using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using Microsoft.Reporting.WebForms;
using System.Linq;

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

            var lojasDao = lojaBusiness.Listar(new LojaDao());

            return View(lojasDao);
        }

        public ActionResult Gerar(string data = null, int lojaId = 0)
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            if (data == null || lojaId == 0)
            {
                return null;
            }

            var pedidosDao = pedidoBusiness.Listar(new PedidoDao()
            {
                DataPedido = Convert.ToDateTime(data),
                LojaDao = new List<LojaDao>() { new LojaDao() { LojaID = lojaId } }
            });

            if (pedidosDao == null || pedidosDao.Count() <= 0)
            {
                return null;
            }

            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;

            var viewer = new ReportViewer();

            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Server.MapPath("~/bin/Reports/MovimentoDeCaixa.rdlc");

            double dinheiro = 0, cartaoVisa = 0, cartaoMaster = 0, cheque = 0, cartaoElo = 0, cartaoHiper = 0, cartaoAmericanExpress = 0, crediario = 0, totalRecebido = 0;
            foreach (var item in pedidosDao)
            {
                dinheiro += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Dinheiro.GetHashCode()).Sum(x => x.ValorPago);
                cartaoVisa += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoVisa.GetHashCode()).Sum(x => x.ValorPago);
                cartaoMaster += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoMaster.GetHashCode()).Sum(x => x.ValorPago);
                cheque += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Cheque.GetHashCode()).Sum(x => x.ValorPago);
                cartaoElo += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoElo.GetHashCode()).Sum(x => x.ValorPago);
                cartaoHiper += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoHiper.GetHashCode()).Sum(x => x.ValorPago);
                cartaoAmericanExpress += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.CartaoAmericanExpress.GetHashCode()).Sum(x => x.ValorPago);
                crediario += item.PedidoTipoPagamentoDao.Where(x => x.TipoPagamentoDao.TipoPagamentoID == TipoPagamentoDao.ETipoPagamento.Crediario.GetHashCode()).Sum(x => x.ValorPago);
            }

            totalRecebido = dinheiro + cartaoVisa + cartaoMaster + cheque + cartaoElo + cartaoHiper + cartaoAmericanExpress + crediario;

            // parâmetros
            List<ReportParameter> parametros = new List<ReportParameter>();
            parametros.Add(new ReportParameter("Cnpj", pedidosDao.FirstOrDefault().LojaDao.FirstOrDefault().Cnpj));
            parametros.Add(new ReportParameter("Data", string.Format("{0} {1}:{2}", data, DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString())));
            parametros.Add(new ReportParameter("Dinheiro", dinheiro.ToString()));
            parametros.Add(new ReportParameter("CartaoVisa", cartaoVisa.ToString()));
            parametros.Add(new ReportParameter("CartaoMaster", cartaoMaster.ToString()));
            parametros.Add(new ReportParameter("Cheque", cheque.ToString()));
            parametros.Add(new ReportParameter("TotalRecebido", totalRecebido.ToString()));
            parametros.Add(new ReportParameter("CartaoElo", cartaoElo.ToString()));
            parametros.Add(new ReportParameter("CartaoHiper", cartaoHiper.ToString()));
            parametros.Add(new ReportParameter("CartaoAmericanExpress", cartaoAmericanExpress.ToString()));
            parametros.Add(new ReportParameter("Crediario", crediario.ToString()));

            viewer.LocalReport.SetParameters(parametros);

            // pedido
            List<dynamic> pedidos = new List<dynamic>();
            foreach (var item in pedidosDao)
            {
                pedidos.Add(new
                {
                    PedidoID = item.PedidoID,
                    Valor = item.PedidoTipoPagamentoDao.Sum(x => x.ValorPago),
                    Forma = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.TipoPagamentoDao.Descricao)),
                    Prazo = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.ParcelaDao.Numero)),
                    Observacao = item.Observacao,
                    CV = string.Join(",", item.PedidoTipoPagamentoDao.Select(x => x.CV))
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_movimento_caixa", pedidos));

            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return new FileContentResult(bytes, mimeType);
        }
    }
}
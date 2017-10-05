using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class PedidoController : BaseController
    {
        private PedidoBusiness pedidoBusiness;
        private PedidoStatusBusiness pedidoStatusBusiness;
        private FuncionarioBusiness funcionarioBusiness;
        private LojaBusiness lojaBusiness;
        private TipoPagamentoBusiness tipoPagamentoBusiness;
        private OrcamentoBusiness orcamentoBusiness;

        public PedidoController()
        {
            pedidoBusiness = new PedidoBusiness();
            pedidoStatusBusiness = new PedidoStatusBusiness();
            funcionarioBusiness = new FuncionarioBusiness();
            lojaBusiness = new LojaBusiness();
            tipoPagamentoBusiness = new TipoPagamentoBusiness();
            orcamentoBusiness = new OrcamentoBusiness();
        }

        public ActionResult Cadastro(string orcamentoID = null)
        {
            PedidoDao pedidoDao = new PedidoDao();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                // lista somente os status "Previsão de entrega" e "Retirado na Loja"
                pedidoDao.PedidoStatusDao = pedidoStatusBusiness.Listar(new PedidoStatusDao()).Where(x => x.PedidoStatusID == (int)PedidoStatusDao.EPedidoStatus.PrevisaoDeEntrega || x.PedidoStatusID == (int)PedidoStatusDao.EPedidoStatus.RetiradoNaLoja).ToList();

                // filtra os funcionario por loja logada, se existir
                var funcionarioDao = new FuncionarioDao();
                if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                {
                    var lojaDao = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                    funcionarioDao.LojaDao.Clear();
                    funcionarioDao.LojaDao.Add(new LojaDao() { LojaID = lojaDao.LojaID });
                }
                pedidoDao.FuncionarioDao = funcionarioBusiness.Listar(funcionarioDao);

                var lojasDao = lojaBusiness.Listar(new LojaDao());
                pedidoDao.LojaSaidaDao = lojasDao;
                pedidoDao.LojaDao = lojasDao;

                pedidoDao.TipoPagamentoDao = tipoPagamentoBusiness.Listar(new TipoPagamentoDao());

                if (!string.IsNullOrEmpty(orcamentoID))
                {
                    var orcamentoDao = orcamentoBusiness.Listar(new OrcamentoDao() { OrcamentoID = Convert.ToInt32(orcamentoID) }).FirstOrDefault();

                    // se não existe orçamento ou já virou venda deve ser direcionado para outra página
                    if (orcamentoDao == null || orcamentoDao.PedidoDao != null)
                    {
                        return View(pedidoDao);
                    }

                    ViewBag.OrcamentoDao = orcamentoDao;
                }
            }
            catch (Exception ex)
            {

            }

            return View(pedidoDao);
        }

        public ActionResult Lista()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            return View();
        }

        public ActionResult CalendarioDeEntrega()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            return View();
        }

        public ActionResult Comanda(int pedidoId)
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var pedidoDao = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoId }).FirstOrDefault();

            if (pedidoDao == null)
            {
                return Content(string.Format("Pedido {0} não encontrado", pedidoId));
            }

            var bytes = pedidoBusiness.Comanda(pedidoDao);

            return new FileContentResult(bytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult Incluir(PedidoDao pedidoDao)
        {
            try
            {
                pedidoDao.DataPedido = DateTime.Now;

                int pedidoID = pedidoBusiness.Incluir(pedidoDao);

                return Json(new { Sucesso = true, Mensagem = string.Format("Pedido {0} cadastrado com sucesso!", pedidoID) }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Pedido não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Cancelar(PedidoDao pedidoDao)
        {
            try
            {
                pedidoDao.DataCancelamento = DateTime.Now;

                pedidoBusiness.Cancelar(pedidoDao);

                // obtém somente o pedido cancelado
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Pedido cancelado com sucesso!", Pedido = pedido }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Pedido não cancelado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Atualizar(PedidoDao pedidoDao)
        {
            try
            {
                pedidoBusiness.Atualizar(pedidoDao);

                // obtém somente o pedido dado baixa
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Pedido alterado com sucesso!", Pedido = pedido }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Pedido não alterado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DarBaixa(PedidoDao pedidoDao)
        {
            try
            {
                var dtBaixa = DateTime.Now;
                pedidoDao.PedidoProdutoDao.ToList().ForEach(x => x.DataBaixa = dtBaixa);

                pedidoBusiness.DarBaixa(pedidoDao);

                // obtém somente o pedido dado baixa
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Produto baixado com sucesso!", Pedido = pedido }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não baixado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EnviarComandaPorEmail(PedidoDao pedidoDao)
        {
            try
            {
                var email = string.Empty;
                //var pedido = pedidoBusiness.Listar(pedidoDao).FirstOrDefault();

                //if (pedido == null)
                //{
                //    return Json(new { Sucesso = false, Mensagem = $"Pedido {pedidoDao.PedidoID} não encontrado" }, JsonRequestBehavior.AllowGet);
                //}

                pedidoBusiness.EnviarComandaPorEmail(pedidoDao.PedidoID, out email);

                return Json(new { Sucesso = true, Mensagem = $"Comanda enviada para o email {email} com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Comanda não enviada. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(PedidoDao pedidoDao)
        {
            List<PedidoDao> pedidos = new List<PedidoDao>();

            try
            {
                // filtra os pedidos por loja selecionada [Loja.LojaID]
                if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                {
                    var loja = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                    if (loja != null) { pedidoDao.LojaDao.Clear(); pedidoDao.LojaDao.Add(new LojaDao() { LojaID = loja.LojaID }); }
                }

                pedidos = pedidoBusiness.Listar(pedidoDao);

                return Json(pedidos, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(pedidos, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(pedidos, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
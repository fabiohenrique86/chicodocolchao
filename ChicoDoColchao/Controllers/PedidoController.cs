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
        private ConsultorBusiness consultorBusiness;
        private LojaBusiness lojaBusiness;
        private TipoPagamentoBusiness tipoPagamentoBusiness;
        private OrcamentoBusiness orcamentoBusiness;
        private ParcelaBusiness parcelaBusiness;

        public PedidoController()
        {
            pedidoBusiness = new PedidoBusiness();
            pedidoStatusBusiness = new PedidoStatusBusiness();
            consultorBusiness = new ConsultorBusiness();
            lojaBusiness = new LojaBusiness();
            tipoPagamentoBusiness = new TipoPagamentoBusiness();
            orcamentoBusiness = new OrcamentoBusiness();
            parcelaBusiness = new ParcelaBusiness();
        }

        public ActionResult Cadastro(string orcamentoID = null)
        {
            var pedidoDao = new PedidoDao();

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
                
                // filtra os consultores por usuário
                var consultorDao = new ConsultorDao();
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                    if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                        consultorDao.FuncionarioID = usuarioDao.UsuarioID;
                }
                pedidoDao.ConsultorDao = consultorBusiness.Listar(consultorDao);

                var lojasDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

                pedidoDao.LojaSaidaDao = lojasDao;
                pedidoDao.LojaDao = lojasDao;

                pedidoDao.TipoPagamentoDao = tipoPagamentoBusiness.Listar(new TipoPagamentoDao());

                if (!string.IsNullOrEmpty(orcamentoID)) // orçamento
                {
                    var orcamentoDao = orcamentoBusiness.Listar(new OrcamentoDao() { OrcamentoID = Convert.ToInt32(orcamentoID) }).FirstOrDefault();

                    // se não existe orçamento ou já virou venda deve ser direcionado para outra página
                    if (orcamentoDao == null || orcamentoDao.PedidoDao != null)
                        return View(pedidoDao);

                    ViewBag.OrcamentoDao = orcamentoDao;
                }
            }
            catch (Exception ex)
            {

            }

            ViewBag.ParcelaDao = parcelaBusiness.Listar(new ParcelaDao());

            return View(pedidoDao);
        }

        public ActionResult Troca(string pedidoId = null)
        {
            var pedidoDao = new PedidoDao();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                int id = 0;
                int.TryParse(pedidoId, out id);

                if (id <= 0)
                    return RedirectToAction("Index", "Menu");

                // lista somente os status "Previsão de entrega" e "Retirado na Loja"
                pedidoDao.PedidoStatusDao = pedidoStatusBusiness.Listar(new PedidoStatusDao()).Where(x => x.PedidoStatusID == (int)PedidoStatusDao.EPedidoStatus.PrevisaoDeEntrega || x.PedidoStatusID == (int)PedidoStatusDao.EPedidoStatus.RetiradoNaLoja).ToList();
                
                // filtra os consultores por loja logada, se existir
                var consultorDao = new ConsultorDao();
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                    if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                        consultorDao.FuncionarioID = usuarioDao.UsuarioID;
                }
                pedidoDao.ConsultorDao = consultorBusiness.Listar(consultorDao);

                var lojasDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

                pedidoDao.LojaSaidaDao = lojasDao;
                pedidoDao.LojaDao = lojasDao;

                pedidoDao.TipoPagamentoDao = tipoPagamentoBusiness.Listar(new TipoPagamentoDao());

                var p = pedidoBusiness.Listar(new PedidoDao() { PedidoID = id }, false, 0).FirstOrDefault();

                if (p == null)
                    return RedirectToAction("Index", "Menu");

                ViewBag.PedidoDao = p;
            }
            catch (Exception ex)
            {

            }

            ViewBag.ParcelaDao = parcelaBusiness.Listar(new ParcelaDao());

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

            var pedidoDao = new PedidoDao();

            pedidoDao.PedidoStatusDao = pedidoStatusBusiness.Listar(new PedidoStatusDao() { Ativo = true }).ToList();
            
            // filtra os consultores por usuário logado
            var consultorDao = new ConsultorDao();
            if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
            {
                var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);

                if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                    consultorDao.FuncionarioID = usuarioDao.UsuarioID;
            }
            pedidoDao.ConsultorDao = consultorBusiness.Listar(consultorDao);

            var lojasDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            pedidoDao.LojaSaidaDao = lojasDao;
            pedidoDao.LojaDao = lojasDao;

            return View(pedidoDao);
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

            var pedidoDao = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoId }, false, 0).FirstOrDefault();

            if (pedidoDao == null)
                return Content(string.Format("Pedido {0} não encontrado", pedidoId));

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
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }, false, 0).FirstOrDefault();

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
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }, false, 0).FirstOrDefault();

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
                var pedido = pedidoBusiness.Listar(new PedidoDao() { PedidoID = pedidoDao.PedidoID }, false, 0).FirstOrDefault();

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
                var erro = string.Empty;

                var enviado = pedidoBusiness.EnviarComandaPorEmail(pedidoDao.PedidoID, out email, out erro);

                if (!enviado)
                    return Json(new { Sucesso = false, Mensagem = erro }, JsonRequestBehavior.AllowGet);

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

        public JsonResult Listar(PedidoDao pedidoDao, bool top = false, int take = 0)
        {
            var pedidos = new List<PedidoDao>();

            try
            {
                // filtra os pedidos por usuário logado
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);

                    if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                        pedidoDao.ConsultorDao.Add(new ConsultorDao() { FuncionarioID = usuarioDao.UsuarioID });
                }

                pedidos = pedidoBusiness.Listar(pedidoDao, top, take);

                return new JsonResult { Data = new { Sucesso = true, Mensagem = string.Empty, Pedidos = pedidos }, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        [HttpPost]
        public JsonResult Trocar(PedidoDao pedidoDao)
        {
            try
            {
                int pedidoID = pedidoBusiness.Trocar(pedidoDao);

                return Json(new { Sucesso = true, Mensagem = string.Format("Pedido {0} trocado com sucesso!", pedidoID) }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Pedido não trocado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
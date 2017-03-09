using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;

namespace ChicoDoColchao.Controllers
{
    public class PedidoController : BaseController
    {
        private PedidoBusiness pedidoBusiness;
        private PedidoStatusBusiness pedidoStatusBusiness;
        private FuncionarioBusiness funcionarioBusiness;
        private LojaBusiness lojaBusiness;
        private TipoPagamentoBusiness tipoPagamentoBusiness;

        public PedidoController()
        {
            pedidoBusiness = new PedidoBusiness();
            pedidoStatusBusiness = new PedidoStatusBusiness();
            funcionarioBusiness = new FuncionarioBusiness();
            lojaBusiness = new LojaBusiness();
            tipoPagamentoBusiness = new TipoPagamentoBusiness();
        }

        public ActionResult Cadastro()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            PedidoDao pedidoDao = new PedidoDao();

            try
            {
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

        [HttpPost]
        public JsonResult Incluir(PedidoDao pedidoDao)
        {
            try
            {
                // seta os pedidos por loja selecionada [Loja.LojaID]
                if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                {
                    var loja = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                    if (loja != null) { pedidoDao.LojaDao.Clear(); pedidoDao.LojaDao.Add(new LojaDao() { LojaID = loja.LojaID, NomeFantasia = loja.NomeFantasia }); }
                }

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
                pedidoBusiness.Cancelar(pedidoDao);

                // obtém somente o pedido cancelado
                var pedido = pedidoBusiness.Listar(pedidoDao).FirstOrDefault();

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
        public JsonResult Entregar(PedidoDao pedidoDao)
        {
            try
            {
                pedidoDao.DataEntrega = DateTime.Now;
                pedidoBusiness.Entregar(pedidoDao);

                // obtém somente o pedido entregue
                var pedido = pedidoBusiness.Listar(pedidoDao).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Pedido entregue com sucesso!", Pedido = pedido }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Pedido não entregue. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EnviarComandaPorEmail(PedidoDao pedidoDao)
        {
            try
            {
                var pedido = pedidoBusiness.Listar(pedidoDao).FirstOrDefault();

                if (pedido == null)
                {
                    return Json(new { Sucesso = false, Mensagem = string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID) }, JsonRequestBehavior.AllowGet);
                }

                pedidoBusiness.EnviarComandaPorEmail(pedido);
                
                return Json(new { Sucesso = true, Mensagem = string.Format("Comanda enviada para o email {0} com sucesso!", pedido.ClienteDao.FirstOrDefault().Email) }, JsonRequestBehavior.AllowGet);
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
    }
}
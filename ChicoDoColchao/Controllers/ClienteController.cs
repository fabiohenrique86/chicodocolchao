using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class ClienteController : BaseController
    {
        private ClienteBusiness clienteBusiness;
        private EstadoBusiness estadoBusiness;

        public ClienteController()
        {
            clienteBusiness = new ClienteBusiness();
            estadoBusiness = new EstadoBusiness();
        }

        public ActionResult Cadastro()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var clienteDao = new ClienteDao();

            clienteDao.EstadoDao = estadoBusiness.Listar(new EstadoDao());

            return View(clienteDao);
        }

        [HttpPost]
        public JsonResult Incluir(ClienteDao clienteDao)
        {
            try
            {
                clienteBusiness.Incluir(clienteDao);

                return Json(new { Sucesso = true, Mensagem = "Cliente cadastrado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Cliente não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
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

        public JsonResult Listar()
        {
            var clientes = new List<ClienteDao>();

            try
            {
                var clienteDao = new ClienteDao();
                
                clientes = clienteBusiness.Listar(clienteDao);

                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarAutocomplete(string term)
        {
            var clientes = new List<ClienteDao>();

            try
            {
                var clienteDao = new ClienteDao();
                
                clienteDao.Cpf = term;
                clienteDao.Cnpj = term;

                clientes = clienteBusiness.ListarAutocomplete(clienteDao);

                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(clientes, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
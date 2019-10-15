using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Cliente não cadastrado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
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

            ViewBag.EstadoDao = estadoBusiness.Listar(new EstadoDao());

            return View();
        }

        public JsonResult Listar()
        {
            try
            {
                var clientes = clienteBusiness.Listar(new ClienteDao());

                return new JsonResult { Data = new { Sucesso = true, Mensagem = string.Empty, Clientes = clientes }, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        public JsonResult ListarAutocomplete(string term)
        {
            var clientes = new List<ClienteDao>();

            try
            {
                var clienteDao = new ClienteDao();

                clienteDao.Cpf = term;
                clienteDao.Cnpj = term;

                clientes = clienteBusiness.ListarAutocomplete(clienteDao);

                return new JsonResult { Data = clientes, MaxJsonLength = int.MaxValue, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

        [HttpPost]
        public JsonResult Atualizar(ClienteDao clienteDao)
        {
            try
            {
                clienteBusiness.Atualizar(clienteDao);

                // obtém somente o cliente atualizado
                var cliente = clienteBusiness.Listar(new ClienteDao() { ClienteID = clienteDao.ClienteID }).FirstOrDefault();

                return Json(new { Sucesso = true, Mensagem = "Cliente atualizado com sucesso!", Cliente = cliente }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Cliente não atualizado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
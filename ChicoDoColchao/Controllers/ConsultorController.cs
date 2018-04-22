using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class ConsultorController : BaseController
    {
        private ConsultorBusiness consultorBusiness;
        private LojaBusiness lojaBusiness;

        public ConsultorController()
        {
            consultorBusiness = new ConsultorBusiness();
            lojaBusiness = new LojaBusiness();
        }

        public ActionResult Cadastro()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var consultorDao = new ConsultorDao();
            consultorDao.LojaDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            return View(consultorDao);
        }

        public ActionResult Lista()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var consultorDao = new ConsultorDao();
            consultorDao.LojaDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            return View(consultorDao);
        }

        [HttpPost]
        public JsonResult Incluir(ConsultorDao consultorDao)
        {
            try
            {
                consultorBusiness.Incluir(consultorDao);

                return Json(new { Sucesso = true, Mensagem = "Consultor cadastrado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Consultor não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(ConsultorDao consultorDao)
        {
            List<ConsultorDao> consultores = new List<ConsultorDao>();

            try
            {
                consultores = consultorBusiness.Listar(consultorDao);

                return Json(consultores, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(consultores, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(consultores, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Excluir(ConsultorDao consultorDao)
        {
            try
            {
                consultorBusiness.Excluir(consultorDao);

                var consultores = consultorBusiness.Listar(new ConsultorDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = $"Consultor {consultorDao.Numero} excluído com sucesso!", Lista = consultores }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Consultor não excluído. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Alterar(ConsultorDao consultorDao)
        {
            try
            {
                consultorBusiness.Alterar(consultorDao);

                var consultores = consultorBusiness.Listar(new ConsultorDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = $"Consultor alterado com sucesso!", Lista = consultores }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Consultor não alterado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
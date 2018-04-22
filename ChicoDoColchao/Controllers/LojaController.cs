using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class LojaController : BaseController
    {
        private LojaBusiness lojaBusiness;

        public LojaController()
        {
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

            return View();
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

        public ActionResult Seleciona()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(LojaDao lojaDao)
        {
            try
            {
                lojaBusiness.Incluir(lojaDao);

                return Json(new { Sucesso = true, Mensagem = "Loja cadastrada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Loja não cadastrada. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(LojaDao lojaDao)
        {
            var lojas = new List<LojaDao>();

            try
            {
                lojas = lojaBusiness.Listar(lojaDao);

                return Json(lojas, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(lojas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(lojas, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Selecionar(LojaDao lojaDao)
        {
            try
            {
                SelectStore(lojaDao);

                return RedirectToAction("Index", "Menu");
            }
            catch (BusinessException ex)
            {
                return RedirectToAction("Login", "Usuario");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Usuario");
            }
        }

        [HttpPost]
        public JsonResult Alterar(LojaDao lojaDao)
        {
            try
            {
                lojaBusiness.Alterar(lojaDao);

                var lojas = lojaBusiness.Listar(new LojaDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = "Loja alterada com sucesso!", Lista = lojas }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Loja não alterada. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Excluir(LojaDao lojaDao)
        {
            try
            {
                lojaBusiness.Excluir(lojaDao);

                var lojas = lojaBusiness.Listar(new LojaDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = $"Loja {lojaDao.Cnpj} excluída com sucesso!", Lista = lojas }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Loja não excluída. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
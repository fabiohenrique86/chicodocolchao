using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class CategoriaController : BaseController
    {
        private CategoriaBusiness categoriaBusiness;

        public CategoriaController()
        {
            categoriaBusiness = new CategoriaBusiness();
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

        [HttpPost]
        public JsonResult Incluir(CategoriaDao categoriaDao)
        {
            try
            {
                categoriaBusiness.Incluir(categoriaDao);

                return Json(new { Sucesso = true, Mensagem = "Categoria cadastrada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Categoria não cadastrada. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(CategoriaDao categoriaDao)
        {
            List<CategoriaDao> categorias = new List<CategoriaDao>();

            try
            {
                categorias = categoriaBusiness.Listar(categoriaDao);

                return Json(categorias, JsonRequestBehavior.AllowGet);
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
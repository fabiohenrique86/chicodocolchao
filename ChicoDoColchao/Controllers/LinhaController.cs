using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class LinhaController : BaseController
    {
        private LinhaBusiness linhaBusiness;

        public LinhaController()
        {
            linhaBusiness = new LinhaBusiness();
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
        public JsonResult Incluir(LinhaDao linhaDao)
        {
            try
            {
                linhaBusiness.Incluir(linhaDao);

                return Json(new { Sucesso = true, Mensagem = "Linha cadastrada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Linha não cadastrada. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(LinhaDao linhaDao)
        {
            List<LinhaDao> linhas = new List<LinhaDao>();

            try
            {
                linhas = linhaBusiness.Listar(linhaDao);

                return Json(linhas, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(linhas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(linhas, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
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
    public class MedidaController : BaseController
    {
        private MedidaBusiness medidaBusiness;

        public MedidaController()
        {
            medidaBusiness = new MedidaBusiness();
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
        public JsonResult Incluir(MedidaDao medidaDao)
        {
            try
            {
                medidaBusiness.Incluir(medidaDao);

                return Json(new { Sucesso = true, Mensagem = "Medida cadastrada com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Medida não cadastrada. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult Listar(MedidaDao medidaDao)
        {
            List<MedidaDao> medidas = new List<MedidaDao>();

            try
            {
                medidas = medidaBusiness.Listar(medidaDao);

                return Json(medidas, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(medidas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(medidas, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
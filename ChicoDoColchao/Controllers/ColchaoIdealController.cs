using System;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;

namespace ChicoDoColchao.Controllers
{
    public class ColchaoIdealController : Controller
    {
        private ColchaoIdealBusiness colchaoIdealBusiness;

        public ColchaoIdealController()
        {
            colchaoIdealBusiness = new ColchaoIdealBusiness();
        }

        public ActionResult Index()
        {
            ColchaoIdealDao colchaoIdealDao = new ColchaoIdealDao();

            return View(colchaoIdealDao);
        }

        public ActionResult Resultado(string m, string n)
        {
            ViewBag.Resultado = m;

            return View();
        }

        [HttpPost]
        public JsonResult Verificar(ColchaoIdealDao colchaoIdealDao)
        {
            try
            {
                var mensagem = colchaoIdealBusiness.Verificar(colchaoIdealDao);

                return Json(new { Sucesso = true, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
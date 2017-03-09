using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using System;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class ContatoController : Controller
    {
        private ContatoBusiness contatoBusiness;

        public ContatoController()
        {
            contatoBusiness = new ContatoBusiness();
        }

        public ActionResult Index()
        {
            ContatoDao contatoDao = new ContatoDao();

            return View(contatoDao);
        }

        [HttpPost]
        public JsonResult Enviar(ContatoDao contatoDao)
        {
            try
            {
                contatoBusiness.Enviar(contatoDao);
                
                return Json(new { Sucesso = true, Mensagem = "Mensagem enviada com sucesso" }, JsonRequestBehavior.AllowGet);
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
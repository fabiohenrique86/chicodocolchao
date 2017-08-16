using System;
using System.Web.Mvc;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business;

namespace ChicoDoColchao.Controllers
{
    public class AtendimentoDeliveryController : Controller
    {
        private AtendimentoDeliveryBusiness atendimentoDeliveryBusiness;

        public AtendimentoDeliveryController()
        {
            atendimentoDeliveryBusiness = new AtendimentoDeliveryBusiness();
        }

        public ActionResult Index()
        {
            AtendimentoDeliveryDao atendimentoDeliveryDao = new AtendimentoDeliveryDao();

            return View(atendimentoDeliveryDao);
        }

        [HttpPost]
        public JsonResult Solicitar(AtendimentoDeliveryDao atendimentoDeliveryDao)
        {
            try
            {
                atendimentoDeliveryBusiness.Solicitar(atendimentoDeliveryDao);

                return Json(new { Sucesso = true, Mensagem = "Solicitação enviada com sucesso" }, JsonRequestBehavior.AllowGet);
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
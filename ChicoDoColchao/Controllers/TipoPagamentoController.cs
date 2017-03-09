using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;

namespace ChicoDoColchao.Controllers
{
    public class TipoPagamentoController : Controller
    {
        private TipoPagamentoBusiness tipoPagamentoBusiness;

        public TipoPagamentoController()
        {
            tipoPagamentoBusiness = new TipoPagamentoBusiness();
        }

        public JsonResult Listar()
        {
            try
            {
                var tipoPagamentos = tipoPagamentoBusiness.Listar(new TipoPagamentoDao());

                return Json(new { aaData = tipoPagamentos }, JsonRequestBehavior.AllowGet);

                //return new JsonResult { Data = tipoPagamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
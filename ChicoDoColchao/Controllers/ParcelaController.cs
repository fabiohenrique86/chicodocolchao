using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;

namespace ChicoDoColchao.Controllers
{
    public class ParcelaController : Controller
    {
        private ParcelaBusiness parcelaBusiness;

        public ParcelaController()
        {
            parcelaBusiness = new ParcelaBusiness();
        }

        public JsonResult Listar()
        {
            try
            {
                var parcelas = parcelaBusiness.Listar(new ParcelaDao());

                return new JsonResult { Data = parcelas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
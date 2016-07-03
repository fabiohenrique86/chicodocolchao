using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Controllers
{
    public class AtendimentoDeliveryController : Controller
    {
        // GET: AtendimentoDelivery
        public ActionResult Index()
        {
            AtendimentoDeliveryModel atendimentoDeliveryModel = new AtendimentoDeliveryModel();

            return View(atendimentoDeliveryModel);
        }

        [HttpPost]
        public void Solicitar(AtendimentoDeliveryModel atendimentoDeliveryModel)
        {

        }
    }
}
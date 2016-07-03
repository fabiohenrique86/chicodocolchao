using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Controllers
{
    public class ColchaoIdealController : Controller
    {
        public ActionResult Index()
        {
            ColchaoIdealModel colchaoIdealModel = new ColchaoIdealModel();

            return View(colchaoIdealModel);
        }

        [HttpPost]
        public void Verificar(ColchaoIdealModel colchaoIdealModel)
        {

        }
    }
}
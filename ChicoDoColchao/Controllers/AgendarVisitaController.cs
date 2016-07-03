using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Controllers
{
    public class AgendarVisitaController : Controller
    {
        public ActionResult Index()
        {
            AgendarVisitaModel agendarVisitaModel = new AgendarVisitaModel();

            return View(agendarVisitaModel);
        }

        [HttpPost]
        public void Agendar(AgendarVisitaModel agendarVisitaModel)
        {

        }
    }
}
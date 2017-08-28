using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class GerenciarController : BaseController
    {
        public ActionResult Index()
        {
            if (!SessaoAtiva())
            {
                return RedirectToAction("Login", "Usuario");
            }

            return View();
        }
    }
}
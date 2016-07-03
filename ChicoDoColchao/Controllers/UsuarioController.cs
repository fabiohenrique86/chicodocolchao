using ChicoDoColchao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class UsuarioController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void Login(UsuarioModel usuarioModel)
        {

        }
    }
}
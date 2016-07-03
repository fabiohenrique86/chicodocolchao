using ChicoDoColchao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class ContatoController : Controller
    {
        public ActionResult Index()
        {
            ContatoModel contatoModel = new ContatoModel();

            return View(contatoModel);
        }

        [HttpPost]
        public void Enviar(ContatoModel contatoModel)
        {

        }
    }
}
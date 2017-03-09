using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class MenuController : BaseController
    {
        public ActionResult Index()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            return View();
        }
    }
}
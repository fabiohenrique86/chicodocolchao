using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class MenuController : BaseController
    {
        private MenuBusiness menuBusiness;

        public MenuController()
        {
            menuBusiness = new MenuBusiness();
        }

        public ActionResult Index()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var usuarioDao = UsuarioLogado();
            var menuDao = new MenuDao();

            if (usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Gerencial)
            {
                menuDao = menuBusiness.Listar();
            }

            return View(menuDao);
        }
    }
}
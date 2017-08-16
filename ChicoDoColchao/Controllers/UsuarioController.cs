using System;
using System.Linq;
using System.Web.Mvc;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class UsuarioController : BaseController
    {
        private UsuarioBusiness usuarioBusiness;

        public UsuarioController()
        {
            usuarioBusiness = new UsuarioBusiness();
        }

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult Entrar(UsuarioDao usuarioDao)
        {
            try
            {
                var usuarios = usuarioBusiness.Login(usuarioDao);

                if (usuarios == null || usuarios.Count() <= 0)
                {
                    return Json(new { Sucesso = false, Mensagem = "Usuário e/ou senha inválidos", Erro = string.Empty }, JsonRequestBehavior.AllowGet);
                }

                In(usuarios.FirstOrDefault());

                return Json(new { Sucesso = true, Mensagem = "", Erro = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message, Erro = string.Empty }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao realizar login", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult Sair()
        {
            Out();

            return RedirectToAction("Login", "Usuario");
        }
    }
}
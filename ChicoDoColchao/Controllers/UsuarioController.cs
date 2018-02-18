using System;
using System.Linq;
using System.Web.Mvc;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using System.Collections.Generic;

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

        public ActionResult Lista()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

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

                return Json(new { Sucesso = true, Mensagem = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao realizar login", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(UsuarioDao usuarioDao)
        {
            var usuarios = new List<UsuarioDao>();

            try
            {
                usuarios = usuarioBusiness.Listar(usuarioDao);

                return Json(usuarios, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(usuarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(usuarios, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AlterarSenha(UsuarioDao usuarioDao)
        {
            var usuarios = new List<UsuarioDao>();

            try
            {
                usuarioBusiness.AlterarSenha(usuarioDao);

                usuarios = usuarioBusiness.Listar(new UsuarioDao());
                
                return Json(new { Sucesso = true, Mensagem = "Senha alterada com sucesso", Erro = string.Empty, Lista = usuarios }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao alterar senha", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Sair()
        {
            Out();

            return RedirectToAction("Login", "Usuario");
        }
    }
}
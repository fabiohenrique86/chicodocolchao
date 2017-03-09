using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ChicoDoColchao.Dao;

namespace ChicoDoColchao.Controllers
{
    public class BaseController : Controller
    {
        public UsuarioDao UsuarioLogado()
        {
            UsuarioDao usuarioDao = new UsuarioDao();

            try
            {
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") == null)
                {
                    return null;
                }

                usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);

                return usuarioDao;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void In(UsuarioDao usuarioDao)
        {
            try
            {
                var cookieName = "ChicoDoColchao_Usuario";
                HttpCookie httpCookie = Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
                httpCookie.Value = JsonConvert.SerializeObject(usuarioDao);
                httpCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(httpCookie);
            }
            catch (Exception)
            {

            }
        }

        public void Out()
        {
            try
            {
                if (Request.Cookies["ChicoDoColchao_Usuario"] != null)
                {
                    Response.Cookies["ChicoDoColchao_Usuario"].Expires = DateTime.Now.AddDays(-1);
                }

                if (Request.Cookies["ChicoDoColchao_Loja"] != null)
                {
                    Response.Cookies["ChicoDoColchao_Loja"].Expires = DateTime.Now.AddDays(-1);
                }
            }
            catch (Exception)
            {

            }
        }

        public void SelectStore(LojaDao lojaDao)
        {
            try
            {
                if (Request.Cookies["ChicoDoColchao_Loja"] != null)
                {
                    Response.Cookies["ChicoDoColchao_Loja"].Expires = DateTime.Now.AddDays(-1);
                }

                var cookieName = "ChicoDoColchao_Loja";
                HttpCookie httpCookie = Request.Cookies[cookieName] ?? new HttpCookie(cookieName);
                httpCookie.Value = JsonConvert.SerializeObject(lojaDao);
                httpCookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(httpCookie);
            }
            catch (Exception)
            {

            }
        }

        public bool SessaoAtivaEPerfilValidado(out string tela)
        {
            tela = "~/Usuario/Login";

            try
            {
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    tela = "~/Menu/Index";
                    
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                    var controller = Request.RequestContext.RouteData.Values["controller"].ToString();
                    var action = Request.RequestContext.RouteData.Values["action"].ToString();

                    if (usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Gerencial)
                    {
                        return true;
                    }

                    if (Request.Cookies.Get("ChicoDoColchao_Loja") == null)
                    {
                        tela = "~/Loja/Seleciona";
                        return false;
                    }

                    if (controller == "Menu" && action == "Index")
                    {
                        return true;
                    }

                    if (controller == "Pedido" || controller == "Cliente" || controller == "Produto")
                    {
                        if (action == "Cadastro" || action == "Lista" || action == "Comanda")
                        {
                            return true;
                        }
                    }

                    return false;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
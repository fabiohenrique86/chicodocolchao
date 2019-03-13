using System;
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
            catch (Exception ex)
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
                usuarioDao.Senha = string.Empty;
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
                    else if (usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Vendedor && Request.Cookies.Get("ChicoDoColchao_Loja") == null)
                    {
                        // verifica se alguma loja está selecionada. Se não, não deve permitir o acesso
                        tela = "~/Loja/Seleciona";
                        return false;
                    }

                    // verifica se está na tela de menu
                    if (controller == "Menu" && action == "Index")
                    {
                        return true;
                    }

                    if (usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Vendedor)
                    {
                        if (controller == "Pedido")
                        {
                            if (action == "Cadastro" || action == "Lista" || action == "Comanda" || action == "Troca")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Cliente")
                        {
                            if (action == "Cadastro" || action == "Lista")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Produto")
                        {
                            if (action == "Lista")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "MovimentoDeCaixa")
                        {
                            return true;
                        }
                        else if (controller == "Orcamento")
                        {
                            if (action == "Cadastro" || action == "Lista" || action == "Comanda")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Relatorio")
                        {
                            if (action == "Estoque")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Deposito)
                    {
                        if (controller == "Pedido")
                        {
                            if (action == "Lista" || action == "Comanda" || action == "CalendarioDeEntrega")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Cliente")
                        {
                            if (action == "Lista")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Produto")
                        {
                            if (action == "Lista")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (controller == "Relatorio")
                        {
                            if (action == "Estoque")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
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

        public ActionResult Download(string caminho, string arquivo, string tipo)
        {
            try
            {
                var bytes = System.IO.File.ReadAllBytes(caminho);
                return File(bytes, tipo, arquivo);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                System.IO.File.Delete(caminho);
            }
        }
    }
}
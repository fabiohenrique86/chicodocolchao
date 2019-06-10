using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class OrcamentoController : BaseController
    {
        private ConsultorBusiness consultorBusiness;
        private LojaBusiness lojaBusiness;
        private OrcamentoBusiness orcamentoBusiness;

        public OrcamentoController()
        {
            consultorBusiness = new ConsultorBusiness();
            lojaBusiness = new LojaBusiness();
            orcamentoBusiness = new OrcamentoBusiness();
        }

        public ActionResult Cadastro()
        {
            var orcamentoDao = new OrcamentoDao();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                // filtra os consultores por loja logada, se existir
                //var consultorDao = new ConsultorDao();
                //if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                //{
                //    var lojaDao = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                //    consultorDao.LojaDao.Clear();
                //    consultorDao.LojaDao.Add(new LojaDao() { LojaID = lojaDao.LojaID });
                //}

                // filtra os consultores por consultado logado
                var consultorDao = new ConsultorDao();
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                    if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                    {
                        consultorDao.FuncionarioID = usuarioDao.UsuarioID;
                    }
                }

                orcamentoDao.ConsultorDao = consultorBusiness.Listar(consultorDao);

                orcamentoDao.LojaDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });
            }
            catch (Exception ex)
            {

            }

            return View(orcamentoDao);
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

        public ActionResult Comanda(int orcamentoId)
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var orcamentoDao = orcamentoBusiness.Listar(new OrcamentoDao() { OrcamentoID = orcamentoId }).FirstOrDefault();

            if (orcamentoDao == null)
            {
                return Content(string.Format("Orçamento {0} não encontrado", orcamentoId));
            }

            var bytes = orcamentoBusiness.Comanda(orcamentoDao);

            return new FileContentResult(bytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult Incluir(OrcamentoDao orcamentoDao)
        {
            try
            {
                orcamentoDao.DataOrcamento = DateTime.Now;
                orcamentoDao.Ativo = true;

                int orcamentoID = orcamentoBusiness.Incluir(orcamentoDao);

                return Json(new { Sucesso = true, Mensagem = string.Format("Orçamento {0} cadastrado com sucesso!", orcamentoID), OrcamentoID = orcamentoID }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Orçamento não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(OrcamentoDao orcamentoDao)
        {
            var orcamentosDao = new List<OrcamentoDao>();

            try
            {
                // filtra os pedidos por loja selecionada [Loja.LojaID]
                //if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                //{
                //    var loja = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                //    if (loja != null)
                //    {
                //        orcamentoDao.LojaDao.Clear();
                //        orcamentoDao.LojaDao.Add(new LojaDao() { LojaID = loja.LojaID });
                //    }
                //}

                // filtra os consultores por consultado logado
                if (Request.Cookies.Get("ChicoDoColchao_Usuario") != null)
                {
                    var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                    if (usuarioDao != null && usuarioDao.TipoUsuarioDao?.TipoUsuarioID == TipoUsuarioDao.ETipoUsuario.Vendedor.GetHashCode())
                    {
                        orcamentoDao.ConsultorDao.Add(new ConsultorDao() { FuncionarioID = usuarioDao.UsuarioID });
                    }
                }

                orcamentosDao = orcamentoBusiness.Listar(orcamentoDao);

                return Json(orcamentosDao, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(orcamentosDao, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(orcamentosDao, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
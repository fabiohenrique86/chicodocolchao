using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class OrcamentoController : BaseController
    {
        private FuncionarioBusiness funcionarioBusiness;
        private LojaBusiness lojaBusiness;
        private OrcamentoBusiness orcamentoBusiness;

        public OrcamentoController()
        {
            funcionarioBusiness = new FuncionarioBusiness();
            lojaBusiness = new LojaBusiness();
            orcamentoBusiness = new OrcamentoBusiness();
        }

        public ActionResult Cadastro()
        {
            OrcamentoDao orcamentoDao = new OrcamentoDao();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                // filtra os funcionario por loja logada, se existir
                var funcionarioDao = new FuncionarioDao();
                if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                {
                    var lojaDao = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                    funcionarioDao.LojaDao.Clear();
                    funcionarioDao.LojaDao.Add(new LojaDao() { LojaID = lojaDao.LojaID });
                }
                orcamentoDao.FuncionarioDao = funcionarioBusiness.Listar(funcionarioDao);

                orcamentoDao.LojaDao = lojaBusiness.Listar(new LojaDao());

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
        
        [HttpPost]
        public JsonResult Incluir(OrcamentoDao orcamentoDao)
        {
            try
            {
                orcamentoDao.DataOrcamento = DateTime.Now;
                orcamentoDao.Ativo = true;

                int orcamentoID = orcamentoBusiness.Incluir(orcamentoDao);

                return Json(new { Sucesso = true, Mensagem = string.Format("Orçamento {0} cadastrado com sucesso!", orcamentoID) }, JsonRequestBehavior.AllowGet);
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
            List<OrcamentoDao> orcamentosDao = new List<OrcamentoDao>();

            try
            {
                // filtra os pedidos por loja selecionada [Loja.LojaID]
                if (Request.Cookies.Get("ChicoDoColchao_Loja") != null)
                {
                    var loja = JsonConvert.DeserializeObject<LojaDao>(Request.Cookies.Get("ChicoDoColchao_Loja").Value);
                    if (loja != null)
                    {
                        orcamentoDao.LojaDao.Clear();
                        orcamentoDao.LojaDao.Add(new LojaDao() { LojaID = loja.LojaID });
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
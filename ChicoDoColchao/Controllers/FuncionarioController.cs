using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class FuncionarioController : BaseController
    {
        private FuncionarioBusiness funcionarioBusiness;
        private LojaBusiness lojaBusiness;

        public FuncionarioController()
        {
            funcionarioBusiness = new FuncionarioBusiness();
            lojaBusiness = new LojaBusiness();
        }

        public ActionResult Cadastro()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            FuncionarioDao funcionarioDao = new FuncionarioDao();

            funcionarioDao.LojaDao = lojaBusiness.Listar(new LojaDao());

            return View(funcionarioDao);
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
        public JsonResult Incluir(FuncionarioDao funcionarioDao)
        {
            try
            {
                funcionarioBusiness.Incluir(funcionarioDao);

                return Json(new { Sucesso = true, Mensagem = "Funcionário cadastrado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Funcionário não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(FuncionarioDao funcionarioDao)
        {
            List<FuncionarioDao> funcionarios = new List<FuncionarioDao>();

            try
            {
                funcionarios = funcionarioBusiness.Listar(funcionarioDao);

                return Json(funcionarios, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(funcionarios, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(funcionarios, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Excluir(FuncionarioDao funcionarioDao)
        {
            try
            {
                funcionarioBusiness.Excluir(funcionarioDao);

                var funcionarios = funcionarioBusiness.Listar(new FuncionarioDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = $"Funcionário {funcionarioDao.Numero} excluído com sucesso!", Lista = funcionarios }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Funcionário não excluído. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
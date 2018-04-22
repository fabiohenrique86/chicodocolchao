using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class ProdutoController : BaseController
    {
        private ProdutoBusiness produtoBusiness;
        private CategoriaBusiness categoriaBusiness;
        private LojaBusiness lojaBusiness;

        public ProdutoController()
        {
            produtoBusiness = new ProdutoBusiness();
            categoriaBusiness = new CategoriaBusiness();
            lojaBusiness = new LojaBusiness();
        }

        public ActionResult Cadastro(HttpPostedFileBase arquivo = null)
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var produtoDao = new ProdutoDao();

            produtoDao.CategoriaDao = categoriaBusiness.Listar(new CategoriaDao());
            produtoDao.LojaDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            if (arquivo == null)
            {
                produtoDao.Erro = false;
                produtoDao.Mensagem = string.Empty;
                return View("Cadastro", produtoDao);
            }

            if (arquivo.InputStream.Length <= 0)
            {
                produtoDao.Erro = true;
                produtoDao.Mensagem = "Arquivo XLSX é obrigatório";
                return View("Cadastro", produtoDao);
            }

            var extensao = arquivo.FileName.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            if (string.IsNullOrEmpty(extensao) || (!extensao.Contains("xls") && !extensao.Contains("xlsx") && !extensao.Contains("XLS") && !extensao.Contains("XLSX")))
            {
                produtoDao.Erro = true;
                produtoDao.Mensagem = "Arquivo não tem extensão XLSX";
                return View("Cadastro", produtoDao);
            }

            List<string> retorno = produtoBusiness.Importar(arquivo.InputStream);

            if (retorno != null && retorno.Count() > 0)
            {
                produtoDao.Erro = true;
                produtoDao.Mensagem = string.Join("*", retorno);
                return View("Cadastro", produtoDao);
            }

            produtoDao.Erro = false;
            produtoDao.Mensagem = "Planilha XLSX importada com sucesso";
            return View(produtoDao);
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

        public ActionResult Transferencia()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            var lojaDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });

            return View(lojaDao);
        }

        [HttpPost]
        public JsonResult Incluir(ProdutoDao produtoDao)
        {
            try
            {
                produtoBusiness.Incluir(produtoDao);

                return Json(new { Sucesso = true, Mensagem = "Produto cadastrado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não cadastrado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Atualizar(ProdutoDao produtoDao)
        {
            try
            {
                produtoBusiness.Atualizar(produtoDao);

                return Json(new { Sucesso = true, Mensagem = "Produto atualizado com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não atualizado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Excluir(ProdutoDao produtoDao)
        {
            try
            {
                produtoBusiness.Excluir(produtoDao);

                var produtos = produtoBusiness.Listar(new ProdutoDao() { Ativo = true });

                return Json(new { Sucesso = true, Mensagem = "Produto excluído com sucesso!", Produtos = produtos }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não excluído. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Transferir(int? lojaOrigemId, int? lojaDestinoId, List<ProdutoDao> produtosDao)
        {
            try
            {
                produtoBusiness.Transferir(lojaOrigemId.GetValueOrDefault(), lojaDestinoId.GetValueOrDefault(), produtosDao);

                return Json(new { Sucesso = true, Mensagem = "Produtos transferidos com sucesso!" }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não transferido. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(ProdutoDao produtoDao)
        {
            try
            {
                produtoDao.Ativo = true;
                var produtos = produtoBusiness.Listar(produtoDao);

                return new JsonResult { Data = produtos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não listado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarAutocomplete(string term)
        {
            try
            {
                ProdutoDao produtoDao = new ProdutoDao() { Ativo = true };

                long numero = 0;
                long.TryParse(string.Join("", System.Text.RegularExpressions.Regex.Split(term, @"[^\d]")), out numero);

                string descricao = new string(term.Where(x => !char.IsDigit(x)).ToArray());

                if (numero > 0 && string.IsNullOrEmpty(descricao))
                {
                    produtoDao.Numero = numero;
                }
                else
                {
                    produtoDao.Descricao = term.Trim();
                }

                var produtos = produtoBusiness.Listar(produtoDao);

                return new JsonResult { Data = produtos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não listado. Tente novamente.", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
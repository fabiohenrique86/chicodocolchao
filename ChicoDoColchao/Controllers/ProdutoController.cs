using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class ProdutoController : BaseController
    {
        private ProdutoBusiness produtoBusiness;
        private LinhaBusiness linhaBusiness;
        private LojaBusiness lojaBusiness;

        public ProdutoController()
        {
            produtoBusiness = new ProdutoBusiness();
            linhaBusiness = new LinhaBusiness();
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

            ProdutoDao produtoDao = new ProdutoDao();

            produtoDao.LinhaDao = linhaBusiness.Listar(new LinhaDao());

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

            List<LojaDao> lojaDao = new List<LojaDao>();

            lojaDao = lojaBusiness.Listar(new LojaDao());

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
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
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
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Produto não transferido. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(ProdutoDao produtoDao)
        {
            try
            {
                var produtos = produtoBusiness.Listar(produtoDao);

                return new JsonResult { Data = produtos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarAutocomplete(string term)
        {
            try
            {
                ProdutoDao produtoDao = new ProdutoDao();

                long numero = 0;
                long.TryParse(string.Join("", System.Text.RegularExpressions.Regex.Split(term, @"[^\d]")), out numero);

                string descricao = new string(term.Where(x => !char.IsDigit(x)).ToArray());

                if (numero > 0)
                {
                    produtoDao.Numero = numero;
                }
                else if (!string.IsNullOrEmpty(descricao))
                {
                    produtoDao.Descricao = descricao.Trim();
                }

                var produtos = produtoBusiness.Listar(produtoDao);

                return new JsonResult { Data = produtos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult ListarPreco(List<ProdutoDao> produtosDao)
        {
            try
            {
                var produtos = new List<ProdutoDao>();

                foreach (var produto in produtosDao)
                {
                    produtos.Add(produtoBusiness.Listar(produto).FirstOrDefault());
                }

                return new JsonResult { Data = produtos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Importar(HttpPostedFileBase arquivo)
        {
            try
            {
                ProdutoDao produtoDao = new ProdutoDao();
                                
                if (arquivo == null || arquivo.InputStream.Length <= 0)
                {
                    return RedirectToAction("ResultadoImportacao", new { e = true, m = "Arquivo XLSX é obrigatório" });
                }

                if (arquivo.ContentType != "application/octet-stream")
                {
                    return RedirectToAction("ResultadoImportacao", new { e = true, m = "Arquivo não tem extensão XLSX" });
                }

                List<string> retorno = produtoBusiness.Importar(arquivo.InputStream);

                if (retorno != null && retorno.Count() > 0)
                {
                    return RedirectToAction("ResultadoImportacao", new { e = true, m = string.Join("*", retorno) });
                }

                return RedirectToAction("ResultadoImportacao", new { e = false, m = "Planilha importada com sucesso" });
            }
            catch (BusinessException ex)
            {
                return RedirectToAction("ResultadoImportacao", new { e = true, m = ex.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("ResultadoImportacao", new { e = true, m = "Ocorreu um erro na importação da planilha. Tente novamente" });
            }
        }

        public ActionResult ResultadoImportacao(bool e, string m)
        {
            ViewBag.Erro = e;
            ViewBag.Mensagem = m;

            return View();
        }
    }
}
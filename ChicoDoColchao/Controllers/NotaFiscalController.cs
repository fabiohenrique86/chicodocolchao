using System;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using System.Collections.Generic;
using System.Web;

namespace ChicoDoColchao.Controllers
{
    public class NotaFiscalController : BaseController
    {
        private NotaFiscalBusiness notaFiscalBusiness;

        public NotaFiscalController()
        {
            notaFiscalBusiness = new NotaFiscalBusiness();
        }

        public ActionResult Cadastro(bool e = false, string m = "")
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            if (!string.IsNullOrEmpty(m))
            {
                ViewBag.Erro = e;
                ViewBag.Mensagem = m;
            }

            return View();
        }

        [HttpPost]
        public ActionResult ImportarXML(List<HttpPostedFileBase> arquivos)
        {
            int qtdNFeImportada = 0;

            try
            {
                if (arquivos == null)
                {
                    return RedirectToAction("Cadastro", new { e = true, m = "É necessário selecionar os arquivos de NF-e para importar" });
                }

                bool ok = true;
                foreach (var item in arquivos)
                {
                    if (item == null || item.ContentLength <= 0)
                    {
                        ok = false;
                    }
                }

                if (!ok)
                {
                    return RedirectToAction("Cadastro", new { e = true, m = "É necessário selecionar os arquivos de NF-e para importar" });
                }

                NotaFiscalDao notaFiscalDao = new NotaFiscalDao();

                foreach (var arquivo in arquivos)
                {
                    notaFiscalDao.Arquivo.Add(arquivo.InputStream);
                }

                if (notaFiscalDao.Arquivo != null && notaFiscalDao.Arquivo.Count > 0)
                {
                    qtdNFeImportada = notaFiscalBusiness.ImportarXML(notaFiscalDao);
                }

                return RedirectToAction("Cadastro", new { e = false, m = string.Format("{0} NFes importadas com sucesso. {1} ocorreram algum tipo de erro e não foram importadas", qtdNFeImportada, notaFiscalDao.Arquivo.Count - qtdNFeImportada) });
            }
            catch (BusinessException ex)
            {
                return RedirectToAction("Cadastro", new { e = true, m = ex.Message });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Cadastro", new { e = true, m = "Ocoreu algum erro ao importar as NFes. Tente novamente" });
            }
        }
    }
}
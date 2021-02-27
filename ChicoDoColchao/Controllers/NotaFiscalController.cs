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

        public ActionResult Cadastro(List<HttpPostedFileBase> arquivos = null)
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            int qtdNFeImportada = 0;
            var mensagemErro = new List<string>();
            var mensagemSucesso = new List<string>();
            var nfDao = new NotaFiscalDao();

            try
            {
                if (arquivos == null)
                {
                    nfDao.MensagemErro = string.Empty;
                    nfDao.MensagemSucesso = string.Empty;
                    return View("Cadastro", nfDao);
                }

                bool ok = true;
                foreach (var item in arquivos)
                {
                    if (item == null || item.ContentLength <= 0)
                        ok = false;
                }

                if (!ok)
                {
                    nfDao.MensagemErro = "É necessário selecionar os arquivos de NF-e para importar";
                    nfDao.MensagemSucesso = string.Empty;
                    return View("Cadastro", nfDao);
                }

                var notaFiscalDao = new NotaFiscalDao();

                foreach (var arquivo in arquivos)
                    notaFiscalDao.Arquivo.Add(arquivo.InputStream);

                if (notaFiscalDao.Arquivo != null && notaFiscalDao.Arquivo.Count > 0)
                    notaFiscalBusiness.ImportarXML(notaFiscalDao, out mensagemErro, out mensagemSucesso, out qtdNFeImportada);

                if (mensagemErro != null && mensagemErro.Count > 0)
                    nfDao.MensagemErro = string.Join("*", mensagemErro);

                if (mensagemSucesso != null && mensagemSucesso.Count > 0)
                    nfDao.MensagemSucesso = string.Join("*", mensagemSucesso);

                return View("Cadastro", nfDao);
            }
            catch (BusinessException ex)
            {
                nfDao.MensagemErro = ex.Message;
                nfDao.MensagemSucesso = string.Empty;

                return View("Cadastro", nfDao);
            }
            catch (Exception ex)
            {
                nfDao.MensagemErro = "Ocoreu um erro ao importar as NFes. Tente novamente";
                nfDao.MensagemSucesso = string.Empty;

                return View("Cadastro", nfDao);
            }
        }
    }
}
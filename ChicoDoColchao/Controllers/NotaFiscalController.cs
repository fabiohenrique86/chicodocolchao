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
            var mensagem = new List<string>();
            var nfDao = new NotaFiscalDao();

            try
            {
                if (arquivos == null)
                {
                    nfDao.Erro = false;
                    nfDao.Mensagem = string.Empty;
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
                    nfDao.Erro = true;
                    nfDao.Mensagem = "É necessário selecionar os arquivos de NF-e para importar";
                    return View("Cadastro", nfDao);
                }

                var notaFiscalDao = new NotaFiscalDao();

                foreach (var arquivo in arquivos)
                    notaFiscalDao.Arquivo.Add(arquivo.InputStream);

                if (notaFiscalDao.Arquivo != null && notaFiscalDao.Arquivo.Count > 0)
                    notaFiscalBusiness.ImportarXML(notaFiscalDao, out mensagem, out qtdNFeImportada);

                if (mensagem != null && mensagem.Count > 0)
                {
                    nfDao.Erro = true;
                    nfDao.Mensagem = string.Join("*", mensagem);
                    return View("Cadastro", nfDao);
                }

                nfDao.Erro = false;
                nfDao.Mensagem = string.Format("{0} XMLS importados com sucesso", qtdNFeImportada);

                return View("Cadastro", nfDao);
            }
            catch (BusinessException ex)
            {
                nfDao.Erro = false;
                nfDao.Mensagem = ex.Message;

                return View("Cadastro", nfDao);
            }
            catch (Exception ex)
            {
                nfDao.Erro = false;
                nfDao.Mensagem = "Ocoreu um erro ao importar as NFes. Tente novamente";

                return View("Cadastro", nfDao);
            }
        }
    }
}
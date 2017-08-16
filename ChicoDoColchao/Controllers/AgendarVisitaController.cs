using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ChicoDoColchao.Business;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Controllers
{
    public class AgendarVisitaController : BaseController
    {
        private LojaBusiness lojaBusiness;
        private EmailBusiness emailBusiness;

        public AgendarVisitaController()
        {
            lojaBusiness = new LojaBusiness();
            emailBusiness = new EmailBusiness();
        }

        public ActionResult Index()
        {
            List<LojaDao> lojasDao = new List<LojaDao>();

            try
            {
                lojasDao = lojaBusiness.Listar(new LojaDao());
            }
            catch (Exception ex)
            {

            }

            return View(lojasDao);
        }

        [HttpPost]
        public JsonResult Agendar(LojaDao lojaDao, string nome, string email, string telefone, string data, string periodo)
        {
            var sucesso = true;
            var mensagem = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(nome))
                {
                    throw new BusinessException("Nome é obrigatório");
                }

                if (string.IsNullOrEmpty(email))
                {
                    throw new BusinessException("E-mail é obrigatório");
                }

                if (string.IsNullOrEmpty(telefone))
                {
                    throw new BusinessException("Telefone é obrigatório");
                }

                if (lojaDao == null || lojaDao.LojaID <= 0 || string.IsNullOrEmpty(lojaDao.NomeFantasia))
                {
                    throw new BusinessException("Loja é obrigatório");
                }

                if (string.IsNullOrEmpty(data))
                {
                    throw new BusinessException("Data é obrigatório");
                }

                if (string.IsNullOrEmpty(periodo))
                {
                    throw new BusinessException("Período é obrigatório");
                }
                
                EmailDao emailDao = new EmailDao();

                emailDao.Titulo = "Agendamento de visita";
                emailDao.Assunto = string.Format("Agendamento de visita - {0}", nome.Trim());
                emailDao.Remetente = "contato@chicodocolchao.com.br";
                emailDao.Destinatario = "contato@chicodocolchao.com.br";
                emailDao.Mensagem = string.Format("{0}, agendou uma visita na data {1} pela {2} para a loja {3}.<br/><br/>E-mail para retorno {4} e o telefone {5}.", nome.Trim(), data, periodo, lojaDao.NomeFantasia.Trim(), email.Trim(), telefone.Trim());

                emailBusiness.Enviar(emailDao);

                sucesso = true;
                mensagem = "Visita agendada com sucesso.";
            }
            catch (BusinessException ex)
            {
                sucesso = false;
                mensagem = ex.Message;
            }
            catch (Exception ex)
            {
                sucesso = false;
                mensagem = "Ocorreu um erro ao agendar visita. Por favor, tente novamente";
            }

            return Json(new { Sucesso = sucesso, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }
    }
}
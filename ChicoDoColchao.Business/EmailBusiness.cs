using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Net.Mail;

namespace ChicoDoColchao.Business
{
    public class EmailBusiness
    {
        LogRepository logRepository;

        public EmailBusiness()
        {
            logRepository = new LogRepository();
        }

        private void ValidarEnviar(EmailDao emailDao)
        {

            if (string.IsNullOrEmpty(emailDao.Remetente))
            {
                throw new BusinessException("Remetente é obrigatório");
            }

            if (string.IsNullOrEmpty(emailDao.Destinatario))
            {
                throw new BusinessException("Destinatário é obrigatório");
            }

            if (string.IsNullOrEmpty(emailDao.Assunto))
            {
                throw new BusinessException("Assunto é obrigatório");
            }
            
            if (string.IsNullOrEmpty(emailDao.Mensagem))
            {
                throw new BusinessException("Mensagem é obrigatório");
            }
        }
        
        public void Enviar(EmailDao emailDao)
        {
            try
            {
                ValidarEnviar(emailDao);

                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient();

                mail.From = new MailAddress("contato@chicodocolchao.com.br", emailDao.Titulo);
                mail.To.Add(emailDao.Destinatario);
                mail.Subject = emailDao.Assunto;
                mail.Body = emailDao.Mensagem;
                mail.IsBodyHtml = true;

                foreach (var anexo in emailDao.Anexo)
                {
                    mail.Attachments.Add(anexo);
                }

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw;
            }
        }
    }
}

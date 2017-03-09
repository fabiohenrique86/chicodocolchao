using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business
{
    public class ContatoBusiness
    {
        private EmailBusiness emailBusiness;
        LogRepository logRepository;

        public ContatoBusiness()
        {
            logRepository = new LogRepository();
            emailBusiness = new EmailBusiness();
        }

        private void ValidarEnviar(ContatoDao contatoDao)
        {
            if (contatoDao == null)
            {
                throw new BusinessException("Contato é obrigatório");
            }
            
            if (string.IsNullOrEmpty(contatoDao.Nome))
            {
                throw new BusinessException("Nome é obrigatório");
            }

            if (string.IsNullOrEmpty(contatoDao.Email))
            {
                throw new BusinessException("Email é obrigatório");
            }

            if (contatoDao.AssuntoId <= 0)
            {
                throw new BusinessException("Assunto é obrigatório");
            }

            if (string.IsNullOrEmpty(contatoDao.Mensagem))
            {
                throw new BusinessException("Mensagem é obrigatório");
            }
        }
        
        public void Enviar(ContatoDao contatoDao)
        {
            try
            {
                ValidarEnviar(contatoDao);

                EmailDao emailDao = new EmailDao();

                emailDao.Titulo = "Chico do Colchão";
                emailDao.Assunto = contatoDao.Assunto.Trim();
                emailDao.Remetente = contatoDao.Email.Trim();
                emailDao.Destinatario = "contato@chicodocolchao.com.br";
                emailDao.Mensagem = contatoDao.Mensagem + "<br/><br/>" + contatoDao.Nome.Trim() + "<br/>" + contatoDao.Email.Trim();

                emailBusiness.Enviar(emailDao);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}

using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business
{
    public class AtendimentoDeliveryBusiness
    {
        private EmailBusiness emailBusiness;
        LogRepository logRepository;

        public AtendimentoDeliveryBusiness()
        {
            logRepository = new LogRepository();
            emailBusiness = new EmailBusiness();
        }

        private void ValidarEnviar(AtendimentoDeliveryDao atendimentoDeliveryDao)
        {
            if (atendimentoDeliveryDao == null)
            {
                throw new BusinessException("Atendimento Delivery é obrigatório");
            }

            if (string.IsNullOrEmpty(atendimentoDeliveryDao.Nome))
            {
                throw new BusinessException("Nome é obrigatório");
            }

            if (string.IsNullOrEmpty(atendimentoDeliveryDao.Email))
            {
                throw new BusinessException("Email é obrigatório");
            }
        }

        public void Solicitar(AtendimentoDeliveryDao atendimentoDeliveryDao)
        {
            try
            {
                ValidarEnviar(atendimentoDeliveryDao);

                EmailDao emailDao = new EmailDao();

                emailDao.Titulo = "Chico do Colchão";
                emailDao.Assunto = string.Format("Atendimento Delivery - {0}", atendimentoDeliveryDao.Nome);
                emailDao.Remetente = atendimentoDeliveryDao.Email.Trim();
                emailDao.Destinatario = "contato@chicodocolchao.com.br";
                emailDao.Mensagem = "Uma solicitação de atendimento delivery foi feita por " + atendimentoDeliveryDao.Nome.Trim() + ".<br/>"
                                    + "Os detalhes seguem abaixo: <br/><br/>"
                                    + "Telefone Fixo: " + (string.IsNullOrEmpty(atendimentoDeliveryDao.TelefoneFixo) ? "Não informado" : atendimentoDeliveryDao.TelefoneFixo) + "<br/>"
                                    + "Telefone Celular: " + (string.IsNullOrEmpty(atendimentoDeliveryDao.TelefoneCelular) ? "Não informado" : atendimentoDeliveryDao.TelefoneCelular) + "<br/>"
                                    + "Endereço: " + (string.IsNullOrEmpty(atendimentoDeliveryDao.Endereco) ? "Não informado" : atendimentoDeliveryDao.Endereco) + "<br/>"
                                    + "Bairro: " + (string.IsNullOrEmpty(atendimentoDeliveryDao.Bairro) ? "Não informado" : atendimentoDeliveryDao.Bairro) + "<br/>"
                                    + "<br/><br/>" + atendimentoDeliveryDao.Nome + "<br/>" + atendimentoDeliveryDao.Email.Trim();

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

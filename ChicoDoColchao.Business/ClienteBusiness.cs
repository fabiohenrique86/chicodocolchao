using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class ClienteBusiness
    {
        ClienteRepository clienteRepository;
        LogRepository logRepository;

        public ClienteBusiness()
        {
            clienteRepository = new ClienteRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(ClienteDao clienteDao)
        {
            if (clienteDao == null)
            {
                throw new BusinessException("Cliente é obrigatório");
            }

            if (string.IsNullOrEmpty(clienteDao.Cpf) && string.IsNullOrEmpty(clienteDao.Cnpj))
            {
                throw new BusinessException("Cpf ou CNPJ é obrigatório");
            }
            else if (!string.IsNullOrEmpty(clienteDao.Cpf) && !string.IsNullOrEmpty(clienteDao.Cnpj))
            {
                throw new BusinessException("Informe Cpf ou CNPJ");
            }

            if (!string.IsNullOrEmpty(clienteDao.Cpf))
            {
                if (string.IsNullOrEmpty(clienteDao.Nome))
                {
                    throw new BusinessException("Nome é obrigatório");
                }

                if (clienteDao.DataNascimento == null || clienteDao.DataNascimento == DateTime.MinValue)
                {
                    throw new BusinessException("Data de Nascimento é obrigatório");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(clienteDao.NomeFantasia))
                {
                    throw new BusinessException("Nome Fantasia é obrigatório");
                }

                if (string.IsNullOrEmpty(clienteDao.RazaoSocial))
                {
                    throw new BusinessException("Razão Social é obrigatório");
                }
            }

            if (string.IsNullOrEmpty(clienteDao.Email))
            {
                throw new BusinessException("E-mail é obrigatório");
            }

            if (string.IsNullOrEmpty(clienteDao.TelefoneResidencial) && string.IsNullOrEmpty(clienteDao.TelefoneResidencial2) && string.IsNullOrEmpty(clienteDao.TelefoneCelular) && string.IsNullOrEmpty(clienteDao.TelefoneCelular2))
            {
                throw new BusinessException("Um telefone é obrigatório");
            }

            if (clienteDao.EstadoDao == null || clienteDao.EstadoDao.Count <= 0 || clienteDao.EstadoDao.FirstOrDefault().EstadoID <= 0)
            {
                throw new BusinessException("Estado é obrigatório");
            }

            if (string.IsNullOrEmpty(clienteDao.Cidade))
            {
                throw new BusinessException("Cidade é obrigatório");
            }

            if (string.IsNullOrEmpty(clienteDao.Logradouro))
            {
                throw new BusinessException("Logradouro é obrigatório");
            }

            if (string.IsNullOrEmpty(clienteDao.Bairro))
            {
                throw new BusinessException("Bairro é obrigatório");
            }

            if (clienteDao.Numero == null || clienteDao.Numero <= 0)
            {
                throw new BusinessException("Número é obrigatório");
            }

            if (clienteRepository.Listar(new Cliente()
            {
                Cpf = string.IsNullOrEmpty(clienteDao.Cpf) ? string.Empty : clienteDao.Cpf.Replace(".", "").Replace("-", ""),
                Cnpj = string.IsNullOrEmpty(clienteDao.Cnpj) ? string.Empty : clienteDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""),
            }).FirstOrDefault() != null)
            {
                throw new BusinessException(string.Format("CPF ou CNPJ {0} já cadastrado", string.IsNullOrEmpty(clienteDao.Cpf) ? clienteDao.Cnpj : clienteDao.Cpf));
            }
        }

        public List<ClienteDao> Listar(ClienteDao clienteDao)
        {
            try
            {
                return clienteRepository.Listar(clienteDao.ToBd()).Select(x => x.ToApp()).ToList();
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

        public List<ClienteDao> ListarAutocomplete(ClienteDao clienteDao)
        {
            try
            {
                return clienteRepository.ListarAutocomplete(clienteDao.ToBd()).Select(x => x.ToApp()).ToList();
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

        public void Incluir(ClienteDao clienteDao)
        {
            try
            {
                ValidarIncluir(clienteDao);

                clienteRepository.Incluir(clienteDao.ToBd());
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

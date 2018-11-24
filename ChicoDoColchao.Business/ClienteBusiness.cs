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

        private void ValidarAtualizar(ClienteDao clienteDao, out Cliente cliente)
        {
            if (clienteDao == null)
            {
                throw new BusinessException("Cliente é obrigatório");
            }

            if (clienteDao.ClienteID <= 0)
            {
                throw new BusinessException("ClienteID é obrigatório");
            }

            cliente = clienteRepository.Listar(new Cliente() { ClienteID = clienteDao.ClienteID }).FirstOrDefault();

            if (cliente == null)
            {
                throw new BusinessException(string.Format("Cliente {0} não encontrado", clienteDao.ClienteID));
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

        public void Atualizar(ClienteDao clienteDao)
        {
            try
            {
                Cliente cliente;

                ValidarAtualizar(clienteDao, out cliente);

                if (!string.IsNullOrEmpty(cliente.Cpf))
                {
                    if (!string.IsNullOrEmpty(clienteDao.Nome))
                    {
                        cliente.Nome = clienteDao.Nome.Trim();
                    }

                    if (!string.IsNullOrEmpty(clienteDao.Cpf))
                    {
                        cliente.Cpf = clienteDao.Cpf.Replace(".", "").Replace("-", "");
                    }

                    if (clienteDao.DataNascimento.GetValueOrDefault() != DateTime.MinValue)
                    {
                        cliente.DataNascimento = clienteDao.DataNascimento.GetValueOrDefault();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(clienteDao.NomeFantasia))
                    {
                        cliente.NomeFantasia = clienteDao.NomeFantasia.Trim();
                    }

                    if (!string.IsNullOrEmpty(clienteDao.Cnpj))
                    {
                        cliente.Cnpj = clienteDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                    }

                    if (!string.IsNullOrEmpty(clienteDao.RazaoSocial))
                    {
                        cliente.RazaoSocial = clienteDao.RazaoSocial.Trim();
                    }
                }

                if (!string.IsNullOrEmpty(clienteDao.Email))
                {
                    cliente.Email = clienteDao.Email.Trim();
                }

                if (!string.IsNullOrEmpty(clienteDao.TelefoneResidencial))
                {
                    cliente.TelefoneResidencial = clienteDao.TelefoneResidencial.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(clienteDao.TelefoneResidencial2))
                {
                    cliente.TelefoneResidencial2 = clienteDao.TelefoneResidencial2.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(clienteDao.TelefoneCelular))
                {
                    cliente.TelefoneCelular = clienteDao.TelefoneCelular.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(clienteDao.TelefoneCelular2))
                {
                    cliente.TelefoneCelular2 = clienteDao.TelefoneCelular2.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(clienteDao.Cep))
                {
                    cliente.Cep = clienteDao.Cep.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (clienteDao.EstadoDao != null && clienteDao.EstadoDao.FirstOrDefault() != null && clienteDao.EstadoDao.FirstOrDefault().EstadoID > 0)
                {
                    cliente.EstadoID = clienteDao.EstadoDao.FirstOrDefault().EstadoID;
                }

                if (!string.IsNullOrEmpty(clienteDao.Cidade))
                {
                    cliente.Cidade = clienteDao.Cidade.Trim();
                }

                if (!string.IsNullOrEmpty(clienteDao.Logradouro))
                {
                    cliente.Logradouro = clienteDao.Logradouro.Trim();
                }

                if (!string.IsNullOrEmpty(clienteDao.Bairro))
                {
                    cliente.Bairro = clienteDao.Bairro.Trim();
                }

                if (clienteDao.Numero.GetValueOrDefault() > 0)
                {
                    cliente.Numero = clienteDao.Numero.GetValueOrDefault();
                }

                if (!string.IsNullOrEmpty(clienteDao.Complemento))
                {
                    cliente.Complemento = clienteDao.Complemento.Trim();
                }

                clienteRepository.Alterar(cliente);
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

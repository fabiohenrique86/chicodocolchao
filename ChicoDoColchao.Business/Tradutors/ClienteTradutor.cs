using System;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class ClienteTradutor
    {
        public static Cliente ToBd(this ClienteDao clienteDao)
        {
            Cliente cliente = new Cliente();

            cliente.ClienteID = clienteDao.ClienteID;

            if (!string.IsNullOrEmpty(clienteDao.Cpf))
            {
                cliente.Cpf = clienteDao.Cpf.Replace(".", "").Replace("-", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.Cnpj))
            {
                cliente.Cnpj = clienteDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.Nome))
            {
                cliente.Nome = clienteDao.Nome.Trim();
            }

            if (!string.IsNullOrEmpty(clienteDao.Email))
            {
                cliente.Email = clienteDao.Email.Trim();
            }

            cliente.DataNascimento = clienteDao.DataNascimento;

            if (!string.IsNullOrEmpty(clienteDao.NomeFantasia))
            {
                cliente.NomeFantasia = clienteDao.NomeFantasia.Trim();
            }

            if (!string.IsNullOrEmpty(clienteDao.RazaoSocial))
            {
                cliente.RazaoSocial = clienteDao.RazaoSocial.Trim();
            }

            if (!string.IsNullOrEmpty(clienteDao.TelefoneResidencial))
            {
                cliente.TelefoneResidencial = clienteDao.TelefoneResidencial.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.TelefoneCelular))
            {
                cliente.TelefoneCelular = clienteDao.TelefoneCelular.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.TelefoneResidencial2))
            {
                cliente.TelefoneResidencial2 = clienteDao.TelefoneResidencial2.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.TelefoneCelular2))
            {
                cliente.TelefoneCelular2 = clienteDao.TelefoneCelular2.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.Cep))
            {
                cliente.Cep = clienteDao.Cep.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!string.IsNullOrEmpty(clienteDao.Cidade))
            {
                cliente.Cidade = clienteDao.Cidade.Trim();
            }

            var estado = clienteDao.EstadoDao.FirstOrDefault();
            if (estado != null)
            {
                //cliente.Estado = new Estado() { EstadoID = estado.EstadoID };
                cliente.EstadoID = estado.EstadoID;
            }

            if (!string.IsNullOrEmpty(clienteDao.Logradouro))
            {
                cliente.Logradouro = clienteDao.Logradouro.Trim();
            }

            if (!string.IsNullOrEmpty(clienteDao.Bairro))
            {
                cliente.Bairro = clienteDao.Bairro.Trim();
            }

            if (!string.IsNullOrEmpty(clienteDao.PontoReferencia))
            {
                cliente.PontoReferencia = clienteDao.PontoReferencia.Trim();
            }

            cliente.Numero = clienteDao.Numero.HasValue ? clienteDao.Numero.GetValueOrDefault() : Convert.ToInt16(0);

            if (!string.IsNullOrEmpty(clienteDao.Complemento))
            {
                cliente.Complemento = clienteDao.Complemento.Trim();
            }

            cliente.Ativo = clienteDao.Ativo;

            return cliente;
        }

        public static ClienteDao ToApp(this Cliente cliente)
        {
            ClienteDao clienteDao = new ClienteDao();

            clienteDao.ClienteID = cliente.ClienteID;

            if (!string.IsNullOrEmpty(cliente.Cpf))
            {
                clienteDao.Cpf = Convert.ToInt64(cliente.Cpf).ToString(@"###\.###\.###\-##");
            }

            if (!string.IsNullOrEmpty(cliente.Cnpj))
            {
                clienteDao.Cnpj = Convert.ToInt64(cliente.Cnpj).ToString(@"##\.###\.###\/####\-##");
            }

            if (!string.IsNullOrEmpty(cliente.Email))
            {
                clienteDao.Email = cliente.Email;
            }

            clienteDao.Nome = cliente.Nome;
            clienteDao.DataNascimento = cliente.DataNascimento;
            clienteDao.NomeFantasia = cliente.NomeFantasia;
            clienteDao.RazaoSocial = cliente.RazaoSocial;

            if (!string.IsNullOrEmpty(cliente.TelefoneResidencial))
            {
                if (cliente.TelefoneResidencial.Length > 10)
                {
                    clienteDao.TelefoneResidencial = Convert.ToInt64(cliente.TelefoneResidencial).ToString("(##) #####-####");
                }
                else
                {
                    clienteDao.TelefoneResidencial = Convert.ToInt64(cliente.TelefoneResidencial).ToString("(##) ####-####");
                }
            }

            if (!string.IsNullOrEmpty(cliente.TelefoneResidencial2))
            {
                if (cliente.TelefoneResidencial2.Length > 10)
                {
                    clienteDao.TelefoneResidencial2 = Convert.ToInt64(cliente.TelefoneResidencial2).ToString("(##) #####-####");
                }
                else
                {
                    clienteDao.TelefoneResidencial2 = Convert.ToInt64(cliente.TelefoneResidencial2).ToString("(##) #####-####");
                }
            }

            if (!string.IsNullOrEmpty(cliente.TelefoneCelular))
            {
                if (cliente.TelefoneCelular.Length > 10)
                {
                    clienteDao.TelefoneCelular = Convert.ToInt64(cliente.TelefoneCelular).ToString("(##) #####-####");
                }
                else
                {
                    clienteDao.TelefoneCelular = Convert.ToInt64(cliente.TelefoneCelular).ToString("(##) #####-####");
                }
            }

            if (!string.IsNullOrEmpty(cliente.TelefoneCelular2))
            {
                if (cliente.TelefoneCelular2.Length > 10)
                {
                    clienteDao.TelefoneCelular2 = Convert.ToInt64(cliente.TelefoneCelular2).ToString("(##) #####-####");
                }
                else
                {
                    clienteDao.TelefoneCelular2 = Convert.ToInt64(cliente.TelefoneCelular2).ToString("(##) #####-####");
                }
            }

            if (!string.IsNullOrEmpty(cliente.Cep))
            {
                clienteDao.Cep = Convert.ToInt64(cliente.Cep).ToString("#####-###");
            }

            clienteDao.Cidade = cliente.Cidade;
            clienteDao.EstadoDao.Add(new EstadoDao() { EstadoID = cliente.Estado.EstadoID, Sigla = cliente.Estado.Sigla, Nome = cliente.Estado.Nome });
            clienteDao.Logradouro = cliente.Logradouro;
            clienteDao.Bairro = cliente.Bairro;
            clienteDao.PontoReferencia = cliente.PontoReferencia;
            clienteDao.Numero = cliente.Numero;
            clienteDao.Complemento = cliente.Complemento;
            clienteDao.Ativo = cliente.Ativo;

            return clienteDao;
        }
    }
}

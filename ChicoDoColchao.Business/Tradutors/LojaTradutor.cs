using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class LojaTradutor
    {
        public static Loja ToBd(this LojaDao lojaDao)
        {
            var loja = new Loja();

            loja.LojaID = lojaDao.LojaID;

            if (!string.IsNullOrEmpty(lojaDao.Cnpj))
            {
                loja.Cnpj = lojaDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            }

            loja.NomeFantasia = lojaDao.NomeFantasia;
            loja.RazaoSocial = lojaDao.RazaoSocial;

            if (!string.IsNullOrEmpty(lojaDao.Telefone))
            {
                loja.Telefone = lojaDao.Telefone.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            loja.Ativo = lojaDao.Ativo;
            loja.Deposito = lojaDao.Deposito;
            loja.Bairro = lojaDao.Bairro;
            loja.Logradouro = lojaDao.Logradouro;
            loja.Numero = lojaDao.Numero;
            loja.Complemento = lojaDao.Complemento;

            if (!string.IsNullOrEmpty(lojaDao.Cep))
            {
                loja.Cep = lojaDao.Cep.Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            return loja;
        }

        public static LojaDao ToApp(this Loja loja)
        {
            var lojaDao = new LojaDao();

            lojaDao.LojaID = loja.LojaID;

            if (!string.IsNullOrEmpty(loja.Cnpj))
            {
                lojaDao.Cnpj = Convert.ToInt64(loja.Cnpj).ToString(@"##\.###\.###\/####\-##");
            }

            if (!string.IsNullOrEmpty(loja.NomeFantasia))
            {
                lojaDao.NomeFantasia = loja.NomeFantasia;
            }

            if (!string.IsNullOrEmpty(loja.RazaoSocial))
            {
                lojaDao.RazaoSocial = loja.RazaoSocial;
            }

            if (!string.IsNullOrEmpty(loja.Telefone))
            {
                if (loja.Telefone.Length > 10)
                {
                    lojaDao.Telefone = Convert.ToInt64(loja.Telefone).ToString("(##) #####-####");
                }
                else
                {
                    lojaDao.Telefone = Convert.ToInt64(loja.Telefone).ToString("(##) ####-####");
                }
            }

            lojaDao.Ativo = loja.Ativo;
            lojaDao.Deposito = loja.Deposito;
            lojaDao.Bairro = loja.Bairro;
            lojaDao.Logradouro = loja.Logradouro;
            lojaDao.Numero = loja.Numero;
            lojaDao.Complemento = loja.Complemento;

            if (!string.IsNullOrEmpty(loja.Cep))
            {
                lojaDao.Cep = Convert.ToUInt64(loja.Cep).ToString(@"00000\-000");
            }

            return lojaDao;
        }
    }
}

using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class LojaTradutor
    {
        public static Loja ToBd(this LojaDao lojaDao)
        {
            Loja loja = new Loja();

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

            return loja;
        }

        public static LojaDao ToApp(this Loja loja)
        {
            LojaDao lojaDao = new LojaDao();

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

            return lojaDao;
        }
    }
}

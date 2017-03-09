using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class FuncionarioTradutor
    {
        public static Funcionario ToBd(this FuncionarioDao funcionarioDao)
        {
            Funcionario funcionario = new Funcionario();

            funcionario.FuncionarioID = funcionarioDao.FuncionarioID;
            funcionario.Numero = funcionarioDao.Numero.GetValueOrDefault();
            if (funcionarioDao.LojaDao.FirstOrDefault() != null)
            {
                funcionario.LojaID = funcionarioDao.LojaDao.FirstOrDefault().LojaID;
                funcionario.Loja = new Loja() { LojaID = funcionarioDao.LojaDao.FirstOrDefault().LojaID };
            }
            funcionario.Nome = funcionarioDao.Nome;
            if (!string.IsNullOrEmpty(funcionarioDao.Telefone))
            {
                funcionario.Telefone = funcionarioDao.Telefone.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }
            funcionario.Email = funcionarioDao.Email;
            funcionario.Ativo = funcionarioDao.Ativo;

            return funcionario;
        }

        public static FuncionarioDao ToApp(this Funcionario funcionario)
        {
            FuncionarioDao funcionarioDao = new FuncionarioDao();

            funcionarioDao.FuncionarioID = funcionario.FuncionarioID;
            funcionarioDao.Numero = funcionario.Numero;
            funcionarioDao.LojaDao.Add(new LojaDao()
            {
                LojaID = funcionario.Loja.LojaID,
                NomeFantasia = funcionario.Loja.NomeFantasia
            });
            funcionarioDao.Nome = funcionario.Nome;
            if (!string.IsNullOrEmpty(funcionario.Telefone))
            {
                if (funcionario.Telefone.Length > 10)
                {
                    funcionarioDao.Telefone = Convert.ToInt64(funcionario.Telefone).ToString("(##) #####-####");
                }
                else
                {
                    funcionarioDao.Telefone = Convert.ToInt64(funcionario.Telefone).ToString("(##) ####-####");
                }
            }
            funcionarioDao.Email = funcionario.Email;
            funcionarioDao.Ativo = funcionario.Ativo;

            return funcionarioDao;
        }
    }
}

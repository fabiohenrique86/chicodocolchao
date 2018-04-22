using System;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class ConsultorTradutor
    {
        public static Funcionario ToBd(this ConsultorDao consultorDao)
        {
            var funcionario = new Funcionario();

            funcionario.FuncionarioID = consultorDao.FuncionarioID;
            funcionario.Numero = consultorDao.Numero.GetValueOrDefault();
            if (consultorDao.LojaDao.FirstOrDefault() != null)
            {
                funcionario.LojaID = consultorDao.LojaDao.FirstOrDefault().LojaID;
                funcionario.Loja = new Loja() { LojaID = consultorDao.LojaDao.FirstOrDefault().LojaID };
            }
            funcionario.Nome = consultorDao.Nome;
            if (!string.IsNullOrEmpty(consultorDao.Telefone))
            {
                funcionario.Telefone = consultorDao.Telefone.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }
            funcionario.Email = consultorDao.Email;
            funcionario.Ativo = consultorDao.Ativo;

            return funcionario;
        }

        public static ConsultorDao ToApp(this Funcionario funcionario)
        {
            var consultorDao = new ConsultorDao();

            consultorDao.FuncionarioID = funcionario.FuncionarioID;
            consultorDao.Numero = funcionario.Numero;
            consultorDao.LojaDao.Add(new LojaDao()
            {
                LojaID = funcionario.Loja.LojaID,
                NomeFantasia = funcionario.Loja.NomeFantasia
            });
            consultorDao.Nome = funcionario.Nome;
            if (!string.IsNullOrEmpty(funcionario.Telefone))
            {
                if (funcionario.Telefone.Length > 10)
                {
                    consultorDao.Telefone = Convert.ToInt64(funcionario.Telefone).ToString("(##) #####-####");
                }
                else
                {
                    consultorDao.Telefone = Convert.ToInt64(funcionario.Telefone).ToString("(##) ####-####");
                }
            }
            consultorDao.Email = funcionario.Email;
            consultorDao.Ativo = funcionario.Ativo;

            return consultorDao;
        }
    }
}

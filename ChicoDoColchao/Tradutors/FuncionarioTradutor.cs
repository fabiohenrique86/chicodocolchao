using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Tradutors
{
    public static class FuncionarioTradutor
    {
        public static FuncionarioDao ToDao(this FuncionarioModel funcionarioModel)
        {
            FuncionarioDao funcionarioDao = new FuncionarioDao();

            funcionarioDao.Numero = funcionarioModel.Numero;
            funcionarioDao.LojaDao.LojaID = funcionarioModel.LojaID;
            funcionarioDao.Nome = funcionarioModel.Nome;
            funcionarioDao.Telefone = funcionarioModel.Telefone;
            funcionarioDao.Email = funcionarioModel.Email;
            funcionarioDao.Ativo = funcionarioModel.Ativo;

            return funcionarioDao;
        }

        public static FuncionarioModel ToModel(this FuncionarioDao funcionarioDao)
        {
            FuncionarioModel funcionarioModel = new FuncionarioModel();

            funcionarioModel.Numero = funcionarioDao.Numero;
            funcionarioModel.LojaID = funcionarioDao.LojaDao.LojaID;
            funcionarioModel.Nome = funcionarioDao.Nome;
            funcionarioModel.Telefone = funcionarioDao.Telefone;
            funcionarioModel.Email = funcionarioDao.Email;
            funcionarioModel.Ativo = funcionarioDao.Ativo;

            return funcionarioModel;
        }
    }
}
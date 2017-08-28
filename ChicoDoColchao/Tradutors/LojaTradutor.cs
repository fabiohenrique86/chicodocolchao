using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Tradutors
{
    public static class LojaTradutor
    {
        public static LojaDao ToDao(this LojaModel LojaModel)
        {
            LojaDao LojaDao = new LojaDao();

            LojaDao.LojaID = LojaModel.LojaID;
            LojaDao.Cnpj = LojaModel.Cnpj;
            LojaDao.NomeFantasia = LojaModel.NomeFantasia;
            LojaDao.RazaoSocial = LojaModel.RazaoSocial;
            LojaDao.Telefone = LojaModel.Telefone;
            LojaDao.Ativo = LojaModel.Ativo;

            return LojaDao;
        }

        public static LojaModel ToModel(this LojaDao LojaDao)
        {
            LojaModel LojaModel = new LojaModel();

            LojaModel.LojaID = LojaDao.LojaID;
            LojaModel.Cnpj = LojaDao.Cnpj;
            LojaModel.NomeFantasia = LojaDao.NomeFantasia;
            LojaModel.RazaoSocial = LojaDao.RazaoSocial;
            LojaModel.Telefone = LojaDao.Telefone;            
            LojaModel.Ativo = LojaDao.Ativo;

            return LojaModel;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class ParcelaTradutor
    {
        public static Parcela ToBd(this ParcelaDao parcelaDao)
        {
            Parcela parcela = new Parcela();

            parcela.ParcelaID = parcelaDao.ParcelaID;
            parcela.Numero = parcelaDao.Numero;
            parcela.Ativo = parcelaDao.Ativo;
            
            return parcela;
        }

        public static ParcelaDao ToApp(this Parcela parcela)
        {
            ParcelaDao parcelaDao = new ParcelaDao();

            parcelaDao.ParcelaID = parcela.ParcelaID;
            parcelaDao.Numero = parcela.Numero;
            parcelaDao.Ativo = parcela.Ativo;

            return parcelaDao;
        }
    }
}

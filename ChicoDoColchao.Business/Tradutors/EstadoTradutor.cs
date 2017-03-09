using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class EstadoTradutor
    {
        public static Estado ToBd(this EstadoDao estadoDao)
        {
            Estado estado = new Estado();

            estado.EstadoID = estadoDao.EstadoID;
            estado.Nome = estadoDao.Nome;
            estado.Sigla = estadoDao.Sigla;
            
            return estado;
        }

        public static EstadoDao ToApp(this Estado estado)
        {
            EstadoDao estadoDao = new EstadoDao();

            estadoDao.EstadoID = estado.EstadoID;
            estadoDao.Nome = estado.Nome;
            estadoDao.Sigla = estado.Sigla;

            return estadoDao;
        }
    }
}

using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class LogTradutor
    {
        public static Log ToBd(this LogDao logDao)
        {
            Log log = new Log();

            log.LogID = logDao.LogID;
            log.Descricao = logDao.Descricao;
            log.DataHora = logDao.DataHora;

            return log;
        }

        public static LogDao ToApp(this Log log)
        {
            LogDao logDao = new LogDao();

            logDao.LogID = log.LogID;
            logDao.Descricao = log.Descricao;
            logDao.DataHora = log.DataHora;

            return logDao;
        }
    }
}

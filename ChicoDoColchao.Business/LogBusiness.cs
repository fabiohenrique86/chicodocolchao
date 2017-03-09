using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class LogBusiness
    {
        LogRepository logRepository;

        public LogBusiness()
        {
            logRepository = new LogRepository();
        }

        public void Incluir(LogDao logDao)
        {
            try
            {
                logRepository.Incluir(logDao.ToBd());
            }
            catch (Exception ex)
            {

            }
        }
    }
}

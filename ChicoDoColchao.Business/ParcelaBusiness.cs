using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class ParcelaBusiness
    {
        ParcelaRepository parcelaRepository;
        LogRepository logRepository;

        public ParcelaBusiness()
        {
            parcelaRepository = new ParcelaRepository();
            logRepository = new LogRepository();
        }
        
        public List<ParcelaDao> Listar(ParcelaDao parcelaDao)
        {
            try
            {
                return parcelaRepository.Listar(parcelaDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }  
    }
}

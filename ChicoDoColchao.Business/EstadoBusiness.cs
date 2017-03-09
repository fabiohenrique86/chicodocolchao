using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class EstadoBusiness
    {
        EstadoRepository estadoRepository;
        LogRepository logRepository;
        
        public EstadoBusiness()
        {
            estadoRepository = new EstadoRepository();
            logRepository = new LogRepository();
        }

        public List<EstadoDao> Listar(EstadoDao estadoDao)
        {
            try
            {
                return estadoRepository.Listar(estadoDao.ToBd()).Select(x => x.ToApp()).ToList();
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

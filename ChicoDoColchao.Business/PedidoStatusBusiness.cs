using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class PedidoStatusBusiness
    {
        PedidoStatusRepository pedidoStatusRepository;
        LogRepository logRepository;

        public PedidoStatusBusiness()
        {
            pedidoStatusRepository = new PedidoStatusRepository();
            logRepository = new LogRepository();
        }
        
        public List<PedidoStatusDao> Listar(PedidoStatusDao pedidoStatusDao)
        {
            try
            {
                return pedidoStatusRepository.Listar(pedidoStatusDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }            
        }  
    }
}

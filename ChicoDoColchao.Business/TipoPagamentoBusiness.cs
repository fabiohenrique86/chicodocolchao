using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class TipoPagamentoBusiness
    {
        TipoPagamentoRepository tipoPagamentoRepository;
        LogRepository logRepository;

        public TipoPagamentoBusiness()
        {
            tipoPagamentoRepository = new TipoPagamentoRepository();
            logRepository = new LogRepository();
        }
        
        public List<TipoPagamentoDao> Listar(TipoPagamentoDao tipoPagamentoDao)
        {
            try
            {
                return tipoPagamentoRepository.Listar(tipoPagamentoDao.ToBd()).Select(x => x.ToApp()).ToList();
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

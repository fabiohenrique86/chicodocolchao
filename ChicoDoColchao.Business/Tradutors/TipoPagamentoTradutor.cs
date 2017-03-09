using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class TipoPagamentoTradutor
    {
        public static TipoPagamento ToBd(this TipoPagamentoDao tipoPagamentoDao)
        {
            TipoPagamento tipoPagamento = new TipoPagamento();

            tipoPagamento.TipoPagamentoID = tipoPagamentoDao.TipoPagamentoID;
            tipoPagamento.Descricao = tipoPagamentoDao.Descricao;
            tipoPagamento.Ativo = tipoPagamentoDao.Ativo;
            
            return tipoPagamento;
        }

        public static TipoPagamentoDao ToApp(this TipoPagamento tipoPagamento)
        {
            TipoPagamentoDao tipoPagamentoDao = new TipoPagamentoDao();

            tipoPagamentoDao.TipoPagamentoID = tipoPagamento.TipoPagamentoID;
            tipoPagamentoDao.Descricao = tipoPagamento.Descricao;
            tipoPagamentoDao.Ativo = tipoPagamento.Ativo;

            return tipoPagamentoDao;
        }
    }
}

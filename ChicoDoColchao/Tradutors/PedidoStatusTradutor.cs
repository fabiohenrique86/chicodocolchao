using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Tradutors
{
    public static class PedidoStatusTradutor
    {
        public static PedidoStatusDao ToDao(this PedidoStatusModel pedidoStatusModel)
        {
            PedidoStatusDao pedidoStatusDao = new PedidoStatusDao();

            pedidoStatusDao.PedidoStatusID = pedidoStatusModel.PedidoStatusID;
            pedidoStatusDao.Descricao = pedidoStatusModel.Descricao;
            pedidoStatusDao.Ativo = pedidoStatusModel.Ativo;

            return pedidoStatusDao;
        }

        public static PedidoStatusModel ToModel(this PedidoStatusDao pedidoStatusDao)
        {
            PedidoStatusModel pedidoStatusModel = new PedidoStatusModel();

            pedidoStatusModel.PedidoStatusID = pedidoStatusDao.PedidoStatusID;
            pedidoStatusModel.Descricao = pedidoStatusDao.Descricao;
            pedidoStatusModel.Ativo = pedidoStatusDao.Ativo;

            return pedidoStatusModel;
        }
    }
}
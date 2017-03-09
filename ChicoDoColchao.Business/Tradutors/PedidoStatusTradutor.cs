using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class PedidoStatusTradutor
    {
        public static PedidoStatus ToBd(this PedidoStatusDao pedidoStatusDao)
        {
            PedidoStatus pedidoStatus = new PedidoStatus();

            pedidoStatus.PedidoStatusID = pedidoStatusDao.PedidoStatusID;
            pedidoStatus.Descricao = pedidoStatusDao.Descricao;
            pedidoStatus.Ativo = pedidoStatusDao.Ativo;
            
            return pedidoStatus;
        }

        public static PedidoStatusDao ToApp(this PedidoStatus pedidoStatus)
        {
            PedidoStatusDao pedidoStatusDao = new PedidoStatusDao();

            pedidoStatusDao.PedidoStatusID = pedidoStatus.PedidoStatusID;
            pedidoStatusDao.Descricao = pedidoStatus.Descricao;
            pedidoStatusDao.Ativo = pedidoStatus.Ativo;

            return pedidoStatusDao;
        }
    }
}

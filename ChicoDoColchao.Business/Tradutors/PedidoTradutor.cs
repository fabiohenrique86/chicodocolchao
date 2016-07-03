using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class PedidoTradutor
    {
        public static Pedido ToBD(this PedidoDao pedidoDao)
        {
            Pedido pedido = new Pedido();

            pedido.PedidoID = pedidoDao.PedidoID;
            pedido.Cliente.ClienteID = pedidoDao.ClienteDao.ClienteID;
            pedido.DataEntrega = pedidoDao.DataEntrega;
            pedido.DataPedido = pedidoDao.DataPedido;
            pedido.Funcionario.FuncionarioID = pedidoDao.FuncionarioDao.FuncionarioID;
            pedido.Loja.LojaID = pedidoDao.LojaOrigemDao.LojaID;
            pedido.Loja1.LojaID = pedidoDao.LojaSaidaDao.LojaID;
            pedido.NomeCarreto = pedidoDao.NomeCarreto;
            pedido.Numero = pedidoDao.Numero;
            pedido.Observacao = pedidoDao.Observacao;

            foreach (var item in pedidoDao.PedidoProdutoDao)
            {
                PedidoProduto pedidoProduto = new PedidoProduto();

                pedidoProduto.PedidoID = item.PedidoID;
                pedidoProduto.ProdutoID = item.ProdutoID;
                pedidoProduto.Quantidade = item.Quantidade;
                pedidoProduto.Medida = item.Medida;
                pedidoProduto.Preco = item.Preco;

                pedido.PedidoProduto.Add(pedidoProduto);
            }
            
            foreach (var item in pedidoDao.PedidoTipoPagamentoDao)
            {
                PedidoTipoPagamento pedidoTipoPagamento = new PedidoTipoPagamento();

                pedidoTipoPagamento.PedidoID = item.PedidoID;
                pedidoTipoPagamento.TipoPagamentoID = item.TipoPagamentoDao.TipoPagamentoID;
                pedidoTipoPagamento.ParcelaID = item.ParcelaDao.ParcelaID;
                pedidoTipoPagamento.ValorPago = item.ValorPago;

                pedido.PedidoTipoPagamento.Add(pedidoTipoPagamento);
            }

            pedido.Numero = pedidoDao.Numero;

            return pedido;
        }

        public static PedidoDao ToAPP(this Pedido pedido)
        {
            PedidoDao pedidoDao = new PedidoDao();

            return pedidoDao;
        }
    }
}

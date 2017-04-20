using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;

namespace ChicoDoColchao.Repository
{
    public class PedidoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public PedidoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(Pedido pedido)
        {
            pedido.DataPedido = DateTime.Now;

            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Added;

            // se status do pedido for "Retirado na Loja", afeta o estoque
            if (pedido.PedidoStatus.PedidoStatusID == 2)
            {
                // atualiza o estoque da loja de saída
                foreach (var pedidoProduto in pedido.PedidoProduto)
                {
                    var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.Loja.LojaID);

                    if (lojaProduto != null)
                    {
                        lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade - pedidoProduto.Quantidade);
                    }
                }
            }

            chicoDoColchaoEntities.Entry(pedido.Cliente).State = EntityState.Detached;
            chicoDoColchaoEntities.Entry(pedido.Funcionario).State = EntityState.Detached;
            chicoDoColchaoEntities.Entry(pedido.PedidoStatus).State = EntityState.Detached;
            chicoDoColchaoEntities.Entry(pedido.Loja1).State = EntityState.Detached;
            chicoDoColchaoEntities.Entry(pedido.Loja).State = EntityState.Detached;

            chicoDoColchaoEntities.SaveChanges();

            return pedido.PedidoID;
        }

        public List<Pedido> Listar(Pedido pedido)
        {
            IQueryable<Pedido> query = chicoDoColchaoEntities.Pedido;

            if (pedido.PedidoID > 0)
            {
                query = query.Where(x => x.PedidoID == pedido.PedidoID);
            }

            if (pedido.ClienteID > 0)
            {
                query = query.Where(x => x.ClienteID == pedido.ClienteID);
            }

            if (pedido.DataPedido != DateTime.MinValue)
            {
                query = query.Where(x => x.DataPedido.Day == pedido.DataPedido.Day && x.DataPedido.Month == pedido.DataPedido.Month && x.DataPedido.Year == pedido.DataPedido.Year);
            }

            if (pedido.Loja1 != null)
            {
                if (pedido.Loja1.LojaID > 0)
                {
                    query = query.Where(x => x.Loja1.LojaID == pedido.Loja1.LojaID);
                }
            }

            if (pedido.PedidoStatus != null && pedido.PedidoStatus.PedidoStatusID > 0)
            {
                query = query.Where(x => x.PedidoStatusID == pedido.PedidoStatus.PedidoStatusID);
            }
            
            return query.OrderByDescending(x => x.PedidoID).ToList();
        }

        public void Atualizar(Pedido pedido)
        {
            var p = chicoDoColchaoEntities.Pedido.Where(x => x.PedidoID == pedido.PedidoID).FirstOrDefault();
            if (p != null)
            {
                p.DataEntrega = pedido.DataEntrega;
                chicoDoColchaoEntities.Entry(p).State = EntityState.Modified;
                chicoDoColchaoEntities.SaveChanges();
            }

            //chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;
            //chicoDoColchaoEntities.SaveChanges();
        }

        public void Cancelar(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;

            // atualiza o estoque da loja
            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.Loja.LojaID);

                if (lojaProduto != null)
                {
                    lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade + pedidoProduto.Quantidade);
                }
            }

            // seta o status do pedido para cancelado
            //pedido.PedidoStatusID = pedido.PedidoStatus.PedidoStatusID;

            chicoDoColchaoEntities.SaveChanges();
        }

        public void Entregar(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;

            // atualiza o estoque da loja
            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.Loja.LojaID);

                if (lojaProduto != null)
                {
                    lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade - pedidoProduto.Quantidade);
                }
            }

            // seta o status do pedido para entregue
            //pedido.PedidoStatusID = pedido.PedidoStatus.PedidoStatusID;

            chicoDoColchaoEntities.SaveChanges();
        }
    }
}
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
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Added;

            // se status do pedido for "Retirado na Loja", afeta o estoque
            if (pedido.PedidoStatusID == 2)
            {
                // atualiza o estoque da loja de saída
                foreach (var pedidoProduto in pedido.PedidoProduto)
                {
                    var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.LojaSaidaID);

                    if (lojaProduto != null)
                    {
                        lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade - pedidoProduto.Quantidade);
                    }
                }
            }

            if (pedido.Cliente != null)
            {
                chicoDoColchaoEntities.Entry(pedido.Cliente).State = EntityState.Detached;
            }

            if (pedido.Funcionario != null)
            {
                chicoDoColchaoEntities.Entry(pedido.Funcionario).State = EntityState.Detached;
            }

            if (pedido.PedidoStatus != null)
            {
                chicoDoColchaoEntities.Entry(pedido.PedidoStatus).State = EntityState.Detached;
            }

            if (pedido.LojaOrigem != null)
            {
                chicoDoColchaoEntities.Entry(pedido.LojaOrigem).State = EntityState.Detached;
            }

            if (pedido.LojaSaida != null)
            {
                chicoDoColchaoEntities.Entry(pedido.LojaSaida).State = EntityState.Detached;
            }

            if (pedido.UsuarioPedido != null)
            {
                chicoDoColchaoEntities.Entry(pedido.UsuarioPedido).State = EntityState.Detached;
            }

            if (pedido.UsuarioCancelamento != null)
            {
                chicoDoColchaoEntities.Entry(pedido.UsuarioCancelamento).State = EntityState.Detached;
            }

            chicoDoColchaoEntities.SaveChanges();

            return pedido.PedidoID;
        }

        public List<Pedido> Listar(Pedido pedido, bool top, int take)
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

            if (pedido.LojaOrigem != null && pedido.LojaOrigem.LojaID > 0)
            {
                query = query.Where(x => x.LojaOrigem.LojaID == pedido.LojaOrigem.LojaID);
            }
            else if (pedido.LojaID > 0)
            {
                query = query.Where(x => x.LojaOrigem.LojaID == pedido.LojaID);
            }

            if (pedido.PedidoStatus != null && pedido.PedidoStatus.PedidoStatusID > 0)
            {
                query = query.Where(x => x.PedidoStatusID == pedido.PedidoStatus.PedidoStatusID);
            }
            else if (pedido.PedidoStatusID > 0)
            {
                query = query.Where(x => x.PedidoStatusID == pedido.PedidoStatusID);
            }

            if (top)
            {
                var lista = query.Include(x => x.Cliente.Estado)
                    .Include(x => x.Funcionario)
                    .Include(x => x.LojaOrigem)
                    .Include(x => x.LojaSaida.LojaProduto)
                    .Include(x => x.PedidoProduto.Select(w => w.Produto.Medida))
                    .Include(x => x.PedidoProduto.Select(w => w.Produto.Categoria))
                    .Include(x => x.PedidoProduto.Select(w => w.UsuarioBaixa))
                    .Include(x => x.PedidoProduto.Select(w => w.UsuarioEntrega))
                    .Include(x => x.PedidoStatus)
                    .Include(x => x.PedidoTipoPagamento.Select(w => w.TipoPagamento))
                    .Include(x => x.PedidoTipoPagamento.Select(w => w.Parcela))
                    .Include(x => x.UsuarioPedido)
                    .Include(x => x.UsuarioCancelamento)
                    .OrderByDescending(x => x.PedidoID)
                    .Take(take)
                    .ToList();

                return lista;
            }
            else
            {
                return query.Include(x => x.Cliente.Estado)
                    .Include(x => x.Funcionario)
                    .Include(x => x.LojaOrigem)
                    .Include(x => x.LojaSaida.LojaProduto)
                    .Include(x => x.PedidoProduto.Select(w => w.Produto.Medida))
                    .Include(x => x.PedidoProduto.Select(w => w.Produto.Categoria))
                    .Include(x => x.PedidoProduto.Select(w => w.UsuarioBaixa))
                    .Include(x => x.PedidoProduto.Select(w => w.UsuarioEntrega))
                    .Include(x => x.PedidoStatus)
                    .Include(x => x.PedidoTipoPagamento.Select(w => w.TipoPagamento))
                    .Include(x => x.PedidoTipoPagamento.Select(w => w.Parcela))                    
                    .Include(x => x.UsuarioPedido)
                    .Include(x => x.UsuarioCancelamento)
                    .OrderByDescending(x => x.PedidoID)
                    .ToList();
            }
        }

        public void Atualizar(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();
        }

        public void Cancelar(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;

            // atualiza o estoque da loja de saída
            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.LojaSaida.LojaID);

                if (lojaProduto != null)
                {
                    lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade + pedidoProduto.Quantidade);
                }
            }

            chicoDoColchaoEntities.SaveChanges();
        }

        public void DarBaixa(Pedido pedido, int pedidoStatusId)
        {
            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();

            // atualiza o status do pedido, se não houverem mais produtos pendentes de baixa
            var produtoPendente = chicoDoColchaoEntities.PedidoProduto.Count(x => x.PedidoID == pedido.PedidoID && x.UsuarioBaixaID == null) > 0 ? true : false;

            if (!produtoPendente)
            {
                pedido.PedidoStatusID = pedidoStatusId;
                chicoDoColchaoEntities.SaveChanges();
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using System.Data.Entity.Core.Objects;

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

            if (pedido.PedidoStatusID == Dao.PedidoStatusDao.EPedidoStatus.RetiradoNaLoja.GetHashCode()) // se status do pedido for "Retirado na Loja", afeta o estoque
            {
                // atualiza o estoque da loja de saída
                foreach (var pedidoProduto in pedido.PedidoProduto)
                {
                    // assume a "data entrega" e "data baixa", a mesma do pedido
                    pedidoProduto.UsuarioEntregaID = pedido.UsuarioPedidoID;
                    pedidoProduto.DataEntrega = pedido.DataPedido;
                    pedidoProduto.UsuarioBaixaID = pedido.UsuarioPedidoID;
                    pedidoProduto.DataBaixa = pedido.DataPedido;

                    var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedido.LojaSaidaID);

                    if (lojaProduto != null)
                    {
                        lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade - pedidoProduto.Quantidade);
                    }
                }
            }

            if (pedido.Cliente != null)
                chicoDoColchaoEntities.Entry(pedido.Cliente).State = EntityState.Detached;

            if (pedido.Funcionario != null)
                chicoDoColchaoEntities.Entry(pedido.Funcionario).State = EntityState.Detached;

            if (pedido.PedidoStatus != null)
                chicoDoColchaoEntities.Entry(pedido.PedidoStatus).State = EntityState.Detached;

            if (pedido.LojaOrigem != null)
                chicoDoColchaoEntities.Entry(pedido.LojaOrigem).State = EntityState.Detached;

            if (pedido.LojaSaida != null)
                chicoDoColchaoEntities.Entry(pedido.LojaSaida).State = EntityState.Detached;

            if (pedido.UsuarioPedido != null)
                chicoDoColchaoEntities.Entry(pedido.UsuarioPedido).State = EntityState.Detached;

            if (pedido.UsuarioCancelamento != null)
                chicoDoColchaoEntities.Entry(pedido.UsuarioCancelamento).State = EntityState.Detached;

            chicoDoColchaoEntities.SaveChanges();

            return pedido.PedidoID;
        }

        public List<Pedido> Listar(Pedido pedido, bool top, int take, DateTime? dataPedidoInicio = null, DateTime? dataPedidoFim = null, DateTime? dataEntregaInicio = null, DateTime? dataEntregaFim = null)
        {
            IQueryable<Pedido> query = chicoDoColchaoEntities.Pedido;

            if (pedido.PedidoID > 0)
                query = query.Where(x => x.PedidoID == pedido.PedidoID);

            if (pedido.ClienteID > 0)
                query = query.Where(x => x.ClienteID == pedido.ClienteID);

            if (pedido.FuncionarioID > 0)
                query = query.Where(x => x.FuncionarioID == pedido.FuncionarioID);

            if (pedido.DataPedido != DateTime.MinValue)
                query = query.Where(x => x.DataPedido.Day == pedido.DataPedido.Day && x.DataPedido.Month == pedido.DataPedido.Month && x.DataPedido.Year == pedido.DataPedido.Year);

            if (pedido.LojaOrigem != null && pedido.LojaOrigem.LojaID > 0)
                query = query.Where(x => x.LojaOrigem.LojaID == pedido.LojaOrigem.LojaID);
            else if (pedido.LojaID > 0)
                query = query.Where(x => x.LojaOrigem.LojaID == pedido.LojaID);

            if (pedido.LojaSaida != null && pedido.LojaSaida.LojaID > 0)
                query = query.Where(x => x.LojaSaida.LojaID == pedido.LojaSaida.LojaID);
            else if (pedido.LojaSaidaID > 0)
                query = query.Where(x => x.LojaSaida.LojaID == pedido.LojaSaidaID);

            if (pedido.PedidoStatus != null && pedido.PedidoStatus.PedidoStatusID > 0)
                query = query.Where(x => x.PedidoStatusID == pedido.PedidoStatus.PedidoStatusID);
            else if (pedido.PedidoStatusID > 0)
                query = query.Where(x => x.PedidoStatusID == pedido.PedidoStatusID);

            if (dataPedidoInicio.GetValueOrDefault() != DateTime.MinValue && dataPedidoFim.GetValueOrDefault() != DateTime.MinValue)
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataPedido) >= EntityFunctions.TruncateTime(dataPedidoInicio) && EntityFunctions.TruncateTime(x.DataPedido) <= EntityFunctions.TruncateTime(dataPedidoFim));

            if (dataEntregaInicio.GetValueOrDefault() != DateTime.MinValue && dataEntregaFim.GetValueOrDefault() != DateTime.MinValue)
                query = query.Where(x => x.PedidoProduto.Any(w => EntityFunctions.TruncateTime(w.DataEntrega) >= EntityFunctions.TruncateTime(dataEntregaInicio) && EntityFunctions.TruncateTime(w.DataEntrega) <= EntityFunctions.TruncateTime(dataEntregaFim)));

            if (top && take > 0)
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

        public int Trocar(Dao.PedidoDao pedidoDao)
        {
            var pedido = new Pedido();

            pedido.DataPedido = DateTime.Now;
            pedido.ClienteID = pedidoDao.ClienteDao.FirstOrDefault().ClienteID;
            pedido.FuncionarioID = pedidoDao.ConsultorDao.FirstOrDefault().FuncionarioID;
            pedido.LojaID = pedidoDao.LojaDao.FirstOrDefault().LojaID;
            pedido.LojaSaidaID = pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID;
            pedido.UsuarioPedidoID = pedidoDao.UsuarioPedidoDao.UsuarioID;
            pedido.NomeCarreto = pedidoDao.NomeCarreto;
            pedido.ValorFrete = pedidoDao.ValorFrete;
            pedido.TipoPagamentoFreteID = pedidoDao.TipoPagamentoFreteID;
            pedido.Observacao = pedidoDao.Observacao;
            pedido.Desconto = pedidoDao.Desconto;
            pedido.PedidoStatusID = pedidoDao.PedidoStatusDao.FirstOrDefault().PedidoStatusID;
            pedido.PedidoTrocaID = pedidoDao.PedidoID;

            var pedidoOriginal = chicoDoColchaoEntities.Pedido.Include(x => x.PedidoProduto).FirstOrDefault(x => x.PedidoID == pedidoDao.PedidoID);

            // produtos de entrada
            if (pedidoOriginal != null)
            {
                pedidoOriginal.PedidoStatusID = Dao.PedidoStatusDao.EPedidoStatus.Trocado.GetHashCode(); // muda o status do pedido original para 'Trocado'

                foreach (var pp in pedidoDao.PedidoProdutoDao.Where(x => x.Tipo == Dao.PedidoProdutoDao.ETipo.Entrada.GetHashCode() && x.ProdutoID > 0 && x.Quantidade > 0)) // produtos devolvidos ao estoque da loja do pedido original
                {
                    var pedidoProdutoOriginal = pedidoOriginal.PedidoProduto.FirstOrDefault(x => x.ProdutoID == pp.ProdutoID);

                    if (pedidoProdutoOriginal != null)
                    {
                        // muda o status do produto original para 'Trocado'
                        pedidoProdutoOriginal.UsuarioTrocaID = pp.UsuarioTrocaDao.UsuarioID;
                        pedidoProdutoOriginal.DataTroca = pedido.DataPedido;

                        // só volta para o estoque os produtos que já saíram do estoque, ou seja, que foram dados baixas
                        if (pedidoProdutoOriginal.UsuarioBaixaID > 0)
                        {
                            var lp = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pp.ProdutoID && x.LojaID == pedidoOriginal.LojaSaidaID);

                            if (lp != null)
                                lp.Quantidade = Convert.ToInt16(lp.Quantidade + pp.Quantidade);
                        }
                    }
                }
            }

            // produtos de saída
            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao.Where(x => x.Tipo == Dao.PedidoProdutoDao.ETipo.Saida.GetHashCode() && x.ProdutoID > 0 && x.Quantidade > 0))
            {
                var pedidoProduto = new PedidoProduto();

                pedidoProduto.PedidoID = pedidoProdutoDao.PedidoID;
                pedidoProduto.ProdutoID = pedidoProdutoDao.ProdutoID;
                pedidoProduto.Quantidade = pedidoProdutoDao.Quantidade;
                pedidoProduto.Preco = pedidoProdutoDao.Preco;
                pedidoProduto.UsuarioEntregaID = pedido.UsuarioPedidoID;
                pedidoProduto.DataEntrega = pedidoProdutoDao.DataEntrega;

                // se status do pedido for "Retirado na Loja", afeta o estoque
                if (pedidoDao.PedidoStatusDao.FirstOrDefault().PedidoStatusID == Dao.PedidoStatusDao.EPedidoStatus.RetiradoNaLoja.GetHashCode())
                {
                    pedidoProduto.UsuarioEntregaID = pedido.UsuarioPedidoID;
                    pedidoProduto.DataEntrega = pedido.DataPedido;
                    pedidoProduto.UsuarioBaixaID = pedido.UsuarioPedidoID;
                    pedidoProduto.DataBaixa = pedido.DataPedido;

                    var lojaSaidaId = pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID;
                    var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProdutoDao.ProdutoID && x.LojaID == lojaSaidaId);

                    if (lojaProduto != null)
                        lojaProduto.Quantidade = Convert.ToInt16(lojaProduto.Quantidade - pedidoProdutoDao.Quantidade);
                }

                pedido.PedidoProduto.Add(pedidoProduto);
            }

            // pagamentos
            foreach (var pedidoTipoPagamentoDao in pedidoDao.PedidoTipoPagamentoDao)
            {
                var pedidoTipoPagamento = new PedidoTipoPagamento();

                pedidoTipoPagamento.PedidoID = pedidoTipoPagamentoDao.PedidoID;
                pedidoTipoPagamento.TipoPagamentoID = pedidoTipoPagamentoDao.TipoPagamentoDao.TipoPagamentoID;
                pedidoTipoPagamento.ParcelaID = pedidoTipoPagamentoDao.ParcelaDao.ParcelaID;
                pedidoTipoPagamento.ValorPago = pedidoTipoPagamentoDao.ValorPago;
                pedidoTipoPagamento.CV = pedidoTipoPagamentoDao.CV;

                pedido.PedidoTipoPagamento.Add(pedidoTipoPagamento);
            }

            chicoDoColchaoEntities.Entry(pedido).State = EntityState.Added;

            if (pedido.Cliente != null)
                chicoDoColchaoEntities.Entry(pedido.Cliente).State = EntityState.Detached;

            if (pedido.Funcionario != null)
                chicoDoColchaoEntities.Entry(pedido.Funcionario).State = EntityState.Detached;

            if (pedido.PedidoStatus != null)
                chicoDoColchaoEntities.Entry(pedido.PedidoStatus).State = EntityState.Detached;

            if (pedido.LojaOrigem != null)
                chicoDoColchaoEntities.Entry(pedido.LojaOrigem).State = EntityState.Detached;

            if (pedido.LojaSaida != null)
                chicoDoColchaoEntities.Entry(pedido.LojaSaida).State = EntityState.Detached;

            if (pedido.UsuarioPedido != null)
                chicoDoColchaoEntities.Entry(pedido.UsuarioPedido).State = EntityState.Detached;

            if (pedido.UsuarioCancelamento != null)
                chicoDoColchaoEntities.Entry(pedido.UsuarioCancelamento).State = EntityState.Detached;

            chicoDoColchaoEntities.SaveChanges();

            return pedido.PedidoID;
        }
    }
}
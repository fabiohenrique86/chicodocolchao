using ChicoDoColchao.Dao;
using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class RelatorioRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public RelatorioRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public List<ComissaoDao> Comissao(ComissaoDao comissaoDao)
        {
            var query = (from p in chicoDoColchaoEntities.Pedido
                         join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                         join f in chicoDoColchaoEntities.Funcionario on p.FuncionarioID equals f.FuncionarioID
                         where p.DataPedido >= comissaoDao.DataInicio && p.DataPedido <= comissaoDao.DataFim
                         && (comissaoDao.FuncionarioID > 0 ? p.FuncionarioID == comissaoDao.FuncionarioID : 1 == 1)
                         && f.Ativo == true
                         && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                         group ptp by new { f.FuncionarioID, f.Nome } into g1
                         select new ComissaoDao()
                         {
                             FuncionarioID = g1.Key.FuncionarioID,
                             Nome = g1.Key.Nome,
                             LojaID = g1.Select(x => x.Pedido.LojaOrigem.LojaID).FirstOrDefault(),
                             NomeFantasia = g1.Select(x => x.Pedido.LojaOrigem.NomeFantasia).FirstOrDefault(),
                             Venda = g1.Sum(x => x.ValorPago),
                             QtdPedido = g1.Select(x => x.PedidoID).Distinct().Count(),
                             Comissao = g1.Sum(x => x.ValorPago) * 0.05
                         }).OrderBy(x => x.Venda).ToList();

            return query;
        }

        public List<VendaConsultorDao> VendaConsultor(VendaConsultorDao vendaConsultorDao)
        {
            var query = (from f in chicoDoColchaoEntities.Funcionario
                         join p in chicoDoColchaoEntities.Pedido on f.FuncionarioID equals p.FuncionarioID
                         join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                         where p.DataPedido >= vendaConsultorDao.DataInicio && p.DataPedido <= vendaConsultorDao.DataFim
                         && (vendaConsultorDao.FuncionarioID > 0 ? p.FuncionarioID == vendaConsultorDao.FuncionarioID : 1 == 1)
                         && f.Ativo == true
                         && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                         group ptp by new { f.FuncionarioID, f.Nome, DataInicio = System.Data.Entity.DbFunctions.TruncateTime(p.DataPedido) } into g1
                         select new VendaConsultorDao()
                         {
                             FuncionarioID = g1.Key.FuncionarioID,
                             Nome = g1.Key.Nome,
                             VendaDia = g1.Sum(x => x.ValorPago),
                             VendaAcumulada = (from f2 in chicoDoColchaoEntities.Funcionario
                                               join p2 in chicoDoColchaoEntities.Pedido on f2.FuncionarioID equals p2.FuncionarioID
                                               join ptp2 in chicoDoColchaoEntities.PedidoTipoPagamento on p2.PedidoID equals ptp2.PedidoID
                                               where f2.FuncionarioID == g1.Key.FuncionarioID
                                               && p2.DataPedido >= vendaConsultorDao.DataInicio && p2.DataPedido <= vendaConsultorDao.DataFim
                                               && (vendaConsultorDao.FuncionarioID > 0 ? p2.FuncionarioID == vendaConsultorDao.FuncionarioID : 1 == 1)
                                               && f2.Ativo == true
                                               && p2.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                                               && System.Data.Entity.DbFunctions.TruncateTime(g1.Key.DataInicio) >= System.Data.Entity.DbFunctions.TruncateTime(p2.DataPedido)
                                               group ptp2 by f2.FuncionarioID into g2
                                               select g2.Sum(x => x.ValorPago)
                                              ).FirstOrDefault(),
                             DataInicio = g1.Key.DataInicio
                         }).OrderBy(x => x.DataInicio).ThenBy(x => x.VendaDia).ToList();

            return query;
        }

        public List<VendaLojaDao> VendaLoja(VendaLojaDao vendaLojaDao)
        {
            var query = (from l in chicoDoColchaoEntities.Loja
                         join p in chicoDoColchaoEntities.Pedido on l.LojaID equals p.LojaID
                         join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                         where p.DataPedido >= vendaLojaDao.DataInicio && p.DataPedido <= vendaLojaDao.DataFim
                         && (vendaLojaDao.LojaID > 0 ? p.LojaID == vendaLojaDao.LojaID : 1 == 1)
                         && l.Ativo == true
                         && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                         group ptp by new { l.LojaID, l.NomeFantasia, DataInicio = System.Data.Entity.DbFunctions.TruncateTime(p.DataPedido) } into g1
                         select new VendaLojaDao()
                         {
                             LojaID = g1.Key.LojaID,
                             NomeFantasia = g1.Key.NomeFantasia,
                             VendaDia = g1.Sum(x => x.ValorPago),
                             VendaAcumulada = (from l2 in chicoDoColchaoEntities.Loja
                                               join p2 in chicoDoColchaoEntities.Pedido on l2.LojaID equals p2.LojaID
                                               join ptp2 in chicoDoColchaoEntities.PedidoTipoPagamento on p2.PedidoID equals ptp2.PedidoID
                                               where l2.LojaID == g1.Key.LojaID
                                               && p2.DataPedido >= vendaLojaDao.DataInicio && p2.DataPedido <= vendaLojaDao.DataFim
                                               && (vendaLojaDao.LojaID > 0 ? p2.LojaID == vendaLojaDao.LojaID : 1 == 1)
                                               && l2.Ativo == true
                                               && p2.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                                               && System.Data.Entity.DbFunctions.TruncateTime(g1.Key.DataInicio) >= System.Data.Entity.DbFunctions.TruncateTime(p2.DataPedido)
                                               group ptp2 by l2.LojaID into g2
                                               select g2.Sum(x => x.ValorPago)
                                              ).FirstOrDefault(),
                             DataInicio = g1.Key.DataInicio
                         }).OrderBy(x => x.DataInicio).ThenBy(x => x.VendaDia).ToList();

            return query;
        }

        public List<VendaProdutoDao> VendaProduto(VendaProdutoDao vendaProdutoDao)
        {
            if (vendaProdutoDao.ProdutoDao != null && vendaProdutoDao.ProdutoDao.Count() > 0)
            {
                return (from pr in chicoDoColchaoEntities.Produto
                        join me in chicoDoColchaoEntities.Medida on pr.MedidaID equals me.MedidaID
                        join pp in chicoDoColchaoEntities.PedidoProduto on pr.ProdutoID equals pp.ProdutoID
                        join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on pp.PedidoID equals ptp.PedidoID
                        join pe in chicoDoColchaoEntities.Pedido on ptp.PedidoID equals pe.PedidoID
                        join l in chicoDoColchaoEntities.Loja on pe.LojaID equals l.LojaID
                        where pe.DataPedido >= vendaProdutoDao.DataInicio
                        && pe.DataPedido <= vendaProdutoDao.DataFim
                        && pr.Ativo == true
                        && me.Ativo == true
                        && l.Ativo == true
                        && vendaProdutoDao.ProdutoDao.Any(x => x == pr.ProdutoID)
                        && pe.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                        group new { pr.ProdutoID, pr.Numero, Produto = pr.Descricao, me.MedidaID, Medida = me.Descricao, pp.Quantidade, ptp.ValorPago } by new { pr.ProdutoID, pr.Numero, Produto = pr.Descricao, me.MedidaID, Medida = me.Descricao } into g1
                        select new VendaProdutoDao()
                        {
                            ProdutoID = g1.Key.ProdutoID,
                            Numero = g1.Key.Numero,
                            Produto = g1.Key.Produto,
                            MedidaID = g1.Key.MedidaID,
                            Medida = g1.Key.Medida,
                            Venda = g1.Sum(x => x.ValorPago),
                            Quantidade = g1.Sum(x => x.Quantidade)
                        }).OrderByDescending(x => x.Venda).ToList();
            }
            else
            {
                return (from pr in chicoDoColchaoEntities.Produto
                        join me in chicoDoColchaoEntities.Medida on pr.MedidaID equals me.MedidaID
                        join pp in chicoDoColchaoEntities.PedidoProduto on pr.ProdutoID equals pp.ProdutoID
                        join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on pp.PedidoID equals ptp.PedidoID
                        join pe in chicoDoColchaoEntities.Pedido on ptp.PedidoID equals pe.PedidoID
                        join l in chicoDoColchaoEntities.Loja on pe.LojaID equals l.LojaID
                        where pe.DataPedido >= vendaProdutoDao.DataInicio
                        && pe.DataPedido <= vendaProdutoDao.DataFim
                        && pr.Ativo == true
                        && me.Ativo == true
                        && l.Ativo == true
                        && pe.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                        group new { pr.ProdutoID, pr.Numero, Produto = pr.Descricao, me.MedidaID, Medida = me.Descricao, pp.Quantidade, ptp.ValorPago } by new { pr.ProdutoID, pr.Numero, Produto = pr.Descricao, me.MedidaID, Medida = me.Descricao } into g1
                        select new VendaProdutoDao()
                        {
                            ProdutoID = g1.Key.ProdutoID,
                            Numero = g1.Key.Numero,
                            Produto = g1.Key.Produto,
                            MedidaID = g1.Key.MedidaID,
                            Medida = g1.Key.Medida,
                            Venda = g1.Sum(x => x.ValorPago),
                            Quantidade = g1.Sum(x => x.Quantidade)
                        }).OrderByDescending(x => x.Venda).ToList();
            }
        }
    }
}

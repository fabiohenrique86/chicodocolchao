using System.Linq;
using System.Data.Entity;
using ChicoDoColchao.Dao;
using System;

namespace ChicoDoColchao.Repository
{
    public class MenuRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public MenuRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public MenuDao Listar()
        {
            var menuDao = new MenuDao();

            menuDao.PedidoStatusMes = (from p in chicoDoColchaoEntities.Pedido
                                       where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                       join ps in chicoDoColchaoEntities.PedidoStatus on p.PedidoStatusID equals ps.PedidoStatusID
                                       group p by new { ps.PedidoStatusID, ps.Descricao } into g1
                                       select new PedidoStatusMes() { pedidoStatusID = g1.Key.PedidoStatusID, descricaoStatus = g1.Key.Descricao, qtdPedido = g1.Count() }).ToList();

            menuDao.FaturamentoLojaMes = (from p in chicoDoColchaoEntities.Pedido
                                          join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                                          join l in chicoDoColchaoEntities.Loja on p.LojaID equals l.LojaID
                                          where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                          && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                                          group ptp by new { l.LojaID, l.NomeFantasia } into g1
                                          select new FaturamentoLojaMes()
                                          {
                                              lojaId = g1.Key.LojaID,
                                              nomeFantasia = g1.Key.NomeFantasia,
                                              venda = g1.Sum(x => x.ValorPago)
                                          }).OrderByDescending(x => x.venda).ToList();

            menuDao.FaturamentoTipoPagamentoMes = (from p in chicoDoColchaoEntities.Pedido
                                                   join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                                                   join tp in chicoDoColchaoEntities.TipoPagamento on ptp.TipoPagamentoID equals tp.TipoPagamentoID
                                                   where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                                   && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                                                   group ptp by new { ptp.TipoPagamentoID, tp.Descricao } into g1
                                                   select new FaturamentoTipoPagamentoMes()
                                                   {
                                                       tipoPagamentoId = g1.Key.TipoPagamentoID,
                                                       descricao = g1.Key.Descricao,
                                                       venda = g1.Sum(x => x.ValorPago)
                                                   }).OrderByDescending(x => x.venda).ToList();

            menuDao.LojaEstoqueNegativo = (from l in chicoDoColchaoEntities.Loja
                                           join lp in chicoDoColchaoEntities.LojaProduto on l.LojaID equals lp.LojaID
                                           where lp.Quantidade < 0
                                           group lp by new { l.LojaID, l.NomeFantasia } into g1
                                           select new LojaEstoqueNegativo()
                                           {
                                               lojaId = g1.Key.LojaID,
                                               nomeFantasia = g1.Key.NomeFantasia,
                                               qtdTotal = g1.Sum(x => x.Quantidade)
                                           }).OrderBy(x => x.qtdTotal).Take(5).ToList();

            menuDao.NotaFiscalImportadaMes.quantidade = chicoDoColchaoEntities.NotaFiscal.
                                                        Where(x => x.DataCadastro.Month == DateTime.Now.Month && x.DataCadastro.Year == DateTime.Now.Year).
                                                        Count();

            var faturamentoConsultorMes = (from p in chicoDoColchaoEntities.Pedido
                                           join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                                           join f in chicoDoColchaoEntities.Funcionario on p.FuncionarioID equals f.FuncionarioID
                                           where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                           && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                                           group ptp by new { f.FuncionarioID, f.Nome } into g1
                                           select new FaturamentoConsultorMes()
                                           {
                                               funcionarioID = g1.Key.FuncionarioID,
                                               nome = g1.Key.Nome,
                                               venda = g1.Sum(x => x.ValorPago),
                                               qtdPedido = g1.Select(x => x.PedidoID).Distinct().Count()
                                           }).OrderBy(x => x.venda).ToList();

            if (faturamentoConsultorMes != null && faturamentoConsultorMes.Count() > 0)
            {
                menuDao.FaturamentoConsultorMes.Add(faturamentoConsultorMes.FirstOrDefault());
                menuDao.FaturamentoConsultorMes.Add(faturamentoConsultorMes.LastOrDefault());
            }

            return menuDao;
        }
    }
}

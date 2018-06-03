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
                                       select new PedidoStatusMes() { pedidoStatusID = g1.Key.PedidoStatusID, descricaoStatus = g1.Key.Descricao, qtdPedidoMes = g1.Count() }).ToList();
            
            menuDao.FaturamentoLojaMes = (from p in chicoDoColchaoEntities.Pedido
                                          join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                                          join l in chicoDoColchaoEntities.Loja on p.LojaID equals l.LojaID
                                          where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                          group ptp by new { l.LojaID, l.NomeFantasia } into g1
                                          select new FaturamentoLojaMes()
                                          {
                                              lojaId = g1.Key.LojaID,
                                              nomeFantasia = g1.Key.NomeFantasia,
                                              vendaMes = g1.Sum(x => x.ValorPago)
                                          }).OrderByDescending(x => x.vendaMes).ToList();

            menuDao.FaturamentoTipoPagamentoMes = (from p in chicoDoColchaoEntities.Pedido
                                                   join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                                                   join tp in chicoDoColchaoEntities.TipoPagamento on ptp.TipoPagamentoID equals tp.TipoPagamentoID
                                                   where p.DataPedido.Month == DateTime.Now.Month && p.DataPedido.Year == DateTime.Now.Year
                                                   group ptp by new { ptp.TipoPagamentoID, tp.Descricao } into g1
                                                   select new FaturamentoTipoPagamentoMes()
                                                   {
                                                       tipoPagamentoId = g1.Key.TipoPagamentoID,
                                                       descricao = g1.Key.Descricao,
                                                       vendaMes = g1.Sum(x => x.ValorPago)
                                                   }).OrderByDescending(x => x.vendaMes).ToList();

            menuDao.LojaEstoqueNegativo = (from l in chicoDoColchaoEntities.Loja
                                           join lp in chicoDoColchaoEntities.LojaProduto on l.LojaID equals lp.LojaID
                                           where lp.Quantidade < 0
                                           select new LojaEstoqueNegativo()
                                           {
                                               lojaId = l.LojaID,
                                               nomeFantasia = l.NomeFantasia
                                           }).OrderBy(x => x.nomeFantasia).Take(5).ToList();

            menuDao.NotaFiscalImportadaMes.quantidade = chicoDoColchaoEntities.NotaFiscal.
                                                        Where(x => x.DataNotaFiscal.Month == DateTime.Now.Month && x.DataNotaFiscal.Year == DateTime.Now.Year).
                                                        Count();

            return menuDao;
        }
    }
}

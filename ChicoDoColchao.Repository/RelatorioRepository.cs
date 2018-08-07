using ChicoDoColchao.Dao;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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

        public List<VendaConsultorDao> VendaConsultor(VendaConsultorDao vendaDao)
        {
            var query = (from f in chicoDoColchaoEntities.Funcionario                         
                         join p in chicoDoColchaoEntities.Pedido on f.FuncionarioID equals p.FuncionarioID
                         join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                         where p.DataPedido >= vendaDao.DataInicio && p.DataPedido <= vendaDao.DataFim
                         && (vendaDao.FuncionarioID > 0 ? p.FuncionarioID == vendaDao.FuncionarioID : 1 == 1)
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
                                               && p2.DataPedido >= vendaDao.DataInicio && p2.DataPedido <= vendaDao.DataFim
                                               && (vendaDao.FuncionarioID > 0 ? p2.FuncionarioID == vendaDao.FuncionarioID : 1 == 1)
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
            var query = (from p in chicoDoColchaoEntities.Pedido
                         join ptp in chicoDoColchaoEntities.PedidoTipoPagamento on p.PedidoID equals ptp.PedidoID
                         join f in chicoDoColchaoEntities.Funcionario on p.FuncionarioID equals f.FuncionarioID
                         where p.DataPedido >= vendaLojaDao.DataInicio && p.DataPedido <= vendaLojaDao.DataFim
                         && (vendaLojaDao.LojaID > 0 ? p.LojaID == vendaLojaDao.LojaID : 1 == 1)
                         && f.Ativo == true
                         && p.PedidoStatusID != (int)PedidoStatusDao.EPedidoStatus.Cancelado
                         group ptp by new { f.FuncionarioID, f.Nome } into g1
                         select new VendaLojaDao()
                         {
                             LojaID = g1.Select(x => x.Pedido.LojaOrigem.LojaID).FirstOrDefault(),
                             NomeFantasia = g1.Select(x => x.Pedido.LojaOrigem.NomeFantasia).FirstOrDefault()
                         }).ToList();

            return query;
        }
    }
}

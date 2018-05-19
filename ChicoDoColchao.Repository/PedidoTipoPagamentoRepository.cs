using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class PedidoTipoPagamentoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public PedidoTipoPagamentoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }
        
        public List<PedidoTipoPagamento> Listar(PedidoTipoPagamento pedidoTipoPagamento)
        {
            IQueryable<PedidoTipoPagamento> query = chicoDoColchaoEntities.PedidoTipoPagamento;

            if (pedidoTipoPagamento.PedidoTipoPagamentoID > 0)
            {
                query = query.Where(x => x.PedidoTipoPagamentoID == pedidoTipoPagamento.PedidoTipoPagamentoID);
            }

            if (!string.IsNullOrEmpty(pedidoTipoPagamento.CV))
            {
                query = query.Where(x => x.CV == pedidoTipoPagamento.CV);
            }
            
            return query.ToList();
        }
    }
}

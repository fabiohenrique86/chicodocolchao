using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Repository
{
    public class TipoPagamentoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public TipoPagamentoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }
        
        public List<TipoPagamento> Listar(TipoPagamento tipoPagamento)
        {
            IQueryable<TipoPagamento> query = chicoDoColchaoEntities.TipoPagamento;
            
            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

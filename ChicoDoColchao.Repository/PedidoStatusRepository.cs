using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class PedidoStatusRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public PedidoStatusRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }
        
        public List<PedidoStatus> Listar(PedidoStatus pedidoStatus)
        {
            IQueryable<PedidoStatus> query = chicoDoColchaoEntities.PedidoStatus;

            if (pedidoStatus.PedidoStatusID > 0)
            {
                query = query.Where(x => x.PedidoStatusID == pedidoStatus.PedidoStatusID);
            }
            
            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

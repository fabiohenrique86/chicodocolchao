using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class EstadoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public EstadoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }
        
        public List<Estado> Listar(Estado estado)
        {
            IQueryable<Estado> query = chicoDoColchaoEntities.Estado;

            if (estado.EstadoID > 0)
            {
                query = query.Where(x => x.EstadoID == estado.EstadoID);
            }
            
            return query.OrderBy(x => x.Sigla).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class MedidaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public MedidaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(Medida medida)
        {
            medida.Ativo = true;
            chicoDoColchaoEntities.Entry(medida).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();

            return medida.MedidaID;
        }

        public List<Medida> Listar(Medida medida)
        {
            IQueryable<Medida> query = chicoDoColchaoEntities.Medida;

            if (medida.MedidaID > 0)
            {
                query = query.Where(x => x.MedidaID == medida.MedidaID);
            }

            if (!string.IsNullOrEmpty(medida.Descricao))
            {
                query = query.Where(x => x.Descricao.ToLower().Contains(medida.Descricao.ToLower()));
            }

            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

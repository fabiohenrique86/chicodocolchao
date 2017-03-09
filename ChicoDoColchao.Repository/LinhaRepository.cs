using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class LinhaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public LinhaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(Linha linha)
        {
            linha.Ativo = true;
            chicoDoColchaoEntities.Entry(linha).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();

            return linha.LinhaID;
        }

        public List<Linha> Listar(Linha linha)
        {
            IQueryable<Linha> query = chicoDoColchaoEntities.Linha;

            if (linha.LinhaID > 0)
            {
                query = query.Where(x => x.LinhaID == linha.LinhaID);
            }

            if (!string.IsNullOrEmpty(linha.Descricao))
            {
                query = query.Where(x => x.Descricao.ToLower().Equals(linha.Descricao.ToLower()));
            }

            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class CategoriaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public CategoriaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(Categoria categoria)
        {
            categoria.Ativo = true;
            chicoDoColchaoEntities.Entry(categoria).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();

            return categoria.CategoriaID;
        }

        public List<Categoria> Listar(Categoria categoria)
        {
            IQueryable<Categoria> query = chicoDoColchaoEntities.Categoria;

            if (categoria.CategoriaID > 0)
            {
                query = query.Where(x => x.CategoriaID == categoria.CategoriaID);
            }

            if (!string.IsNullOrEmpty(categoria.Descricao))
            {
                query = query.Where(x => x.Descricao.ToLower().Equals(categoria.Descricao.ToLower()));
            }

            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

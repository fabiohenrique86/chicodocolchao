using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class LojaProdutoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public LojaProdutoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(LojaProduto lojaProduto)
        {
            lojaProduto.Ativo = true;

            chicoDoColchaoEntities.Entry(lojaProduto).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();
        }

        public void Atualizar(LojaProduto lojaProduto)
        {
            var lp = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.LojaProdutoID == lojaProduto.LojaProdutoID && x.Ativo == true);

            if (lp != null)
            {
                lp.Quantidade = Convert.ToInt16(lp.Quantidade + lojaProduto.Quantidade);
            }

            chicoDoColchaoEntities.SaveChanges();
        }

        public List<LojaProduto> Listar(LojaProduto lojaProduto)
        {
            IQueryable<LojaProduto> query = chicoDoColchaoEntities.LojaProduto;

            if (lojaProduto.LojaID > 0)
            {
                query = query.Where(x => x.LojaID == lojaProduto.LojaID);
            }

            if (lojaProduto.ProdutoID > 0)
            {
                query = query.Where(x => x.ProdutoID == lojaProduto.ProdutoID);
            }

            query = query.Where(x => x.Ativo);

            return query.ToList();
        }
    }
}

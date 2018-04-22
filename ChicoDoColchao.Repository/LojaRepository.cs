using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class LojaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public LojaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(Loja loja)
        {
            loja.Ativo = true;
            chicoDoColchaoEntities.Entry(loja).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();
        }

        public List<Loja> Listar(Loja loja)
        {
            IQueryable<Loja> query = chicoDoColchaoEntities.Loja;

            if (loja.LojaID > 0)
            {
                query = query.Where(x => x.LojaID == loja.LojaID);
            }

            if (!string.IsNullOrEmpty(loja.Cnpj))
            {
                query = query.Where(x => x.Cnpj.Equals(loja.Cnpj));
            }

            if (loja.Deposito)
            {
                query = query.Where(x => x.Deposito == loja.Deposito);
            }

            if (!string.IsNullOrEmpty(loja.NomeFantasia))
            {
                query = query.Where(x => x.NomeFantasia.Contains(loja.NomeFantasia));
            }

            query = query.Where(x => x.Ativo == loja.Ativo);

            return query.OrderBy(x => x.NomeFantasia).ToList();
        }

        public void Alterar(Loja loja)
        {
            chicoDoColchaoEntities.SaveChanges();
        }

        public void Excluir(Loja loja)
        {
            chicoDoColchaoEntities.Entry(loja).State = EntityState.Modified;

            loja.Ativo = false;

            if (loja.LojaProduto == null || loja.LojaProduto.Count() <= 0)
            {
                loja.LojaProduto = chicoDoColchaoEntities.LojaProduto.Where(x => x.LojaID == loja.LojaID).ToList();
            }

            // seta para inativo todas as produtos dessa loja inativa
            foreach (var lojaProduto in loja.LojaProduto)
            {
                lojaProduto.Ativo = false;
            }

            chicoDoColchaoEntities.SaveChanges();
        }
    }
}

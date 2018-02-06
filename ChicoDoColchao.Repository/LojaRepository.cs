using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

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

            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.NomeFantasia).ToList();
        }
    }
}

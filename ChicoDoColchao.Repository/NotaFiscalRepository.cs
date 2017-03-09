using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class NotaFiscalRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public NotaFiscalRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(NotaFiscal notaFiscal)
        {
            notaFiscal.Ativo = true;
            chicoDoColchaoEntities.Entry(notaFiscal).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();
        }

        public List<NotaFiscal> Listar(NotaFiscal notaFiscal)
        {
            IQueryable<NotaFiscal> query = chicoDoColchaoEntities.NotaFiscal;

            if (notaFiscal.NotaFiscalID > 0)
            {
                query = query.Where(x => x.NotaFiscalID == notaFiscal.NotaFiscalID);
            }

            if (notaFiscal.Numero > 0)
            {
                query = query.Where(x => x.Numero == notaFiscal.Numero);
            }

            query = query.Where(x => x.Ativo);

            return query.OrderByDescending(x => x.DataNotaFiscal).ToList();
        }
    }
}

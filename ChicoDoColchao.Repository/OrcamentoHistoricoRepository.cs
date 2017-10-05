using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class OrcamentoHistoricoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public OrcamentoHistoricoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(OrcamentoHistorico orcamentoHistorico)
        {
            chicoDoColchaoEntities.Entry(orcamentoHistorico).State = EntityState.Added;
            chicoDoColchaoEntities.SaveChanges();

            return orcamentoHistorico.OrcamentoHistoricoID;
        }

        public List<OrcamentoHistorico> Listar(OrcamentoHistorico orcamentoHistorico, bool top, int take)
        {
            IQueryable<OrcamentoHistorico> query = chicoDoColchaoEntities.OrcamentoHistorico;

            if (orcamentoHistorico.OrcamentoHistoricoID > 0)
            {
                query = query.Where(x => x.OrcamentoHistoricoID == orcamentoHistorico.OrcamentoHistoricoID);
            }

            if (orcamentoHistorico.OrcamentoID > 0)
            {
                query = query.Where(x => x.OrcamentoID == orcamentoHistorico.OrcamentoID);
            }

            if (orcamentoHistorico.DataCadastro != DateTime.MinValue)
            {
                query = query.Where(x => x.DataCadastro.Day == orcamentoHistorico.DataCadastro.Day && x.DataCadastro.Month == orcamentoHistorico.DataCadastro.Month && x.DataCadastro.Year == orcamentoHistorico.DataCadastro.Year);
            }
            
            if (top)
            {
                return query.OrderByDescending(x => x.OrcamentoHistoricoID).Take(take).ToList();
            }
            else
            {
                return query.OrderByDescending(x => x.OrcamentoHistoricoID).ToList();
            }
        }
    }
}
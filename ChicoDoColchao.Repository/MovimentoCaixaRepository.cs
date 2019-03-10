using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System;
using System.Data.Entity.Core.Objects;

namespace ChicoDoColchao.Repository
{
    public class MovimentoCaixaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public MovimentoCaixaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(MovimentoCaixa movimentoCaixa)
        {
            chicoDoColchaoEntities.Entry(movimentoCaixa).State = EntityState.Added;
            chicoDoColchaoEntities.SaveChanges();

            return movimentoCaixa.MovimentoCaixaID;
        }

        public void Excluir(MovimentoCaixa movimentoCaixa)
        {
            var mc = chicoDoColchaoEntities.MovimentoCaixa.FirstOrDefault(x => x.MovimentoCaixaID == movimentoCaixa.MovimentoCaixaID);

            chicoDoColchaoEntities.Entry(mc).State = EntityState.Deleted;
            chicoDoColchaoEntities.SaveChanges();
        }

        public void Receber(MovimentoCaixa movimentoCaixa)
        {
            var mc = chicoDoColchaoEntities.MovimentoCaixa.FirstOrDefault(x => x.MovimentoCaixaID == movimentoCaixa.MovimentoCaixaID);

            if (mc != null)
            {
                mc.MovimentoCaixaStatusID = movimentoCaixa.MovimentoCaixaStatusID;
                mc.DataRecebimento = movimentoCaixa.DataRecebimento;
                mc.UsuarioRecebimentoID = movimentoCaixa.UsuarioRecebimentoID;
            }

            chicoDoColchaoEntities.Entry(mc).State = EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();
        }

        public List<MovimentoCaixa> Listar(MovimentoCaixa movimentoCaixa)
        {
            IQueryable<MovimentoCaixa> query = chicoDoColchaoEntities.MovimentoCaixa;

            if (movimentoCaixa.MovimentoCaixaID > 0)
            {
                query = query.Where(x => x.MovimentoCaixaID == movimentoCaixa.MovimentoCaixaID);
            }

            if (movimentoCaixa.Loja != null && movimentoCaixa.Loja.LojaID > 0)
            {
                query = query.Where(x => x.LojaID == movimentoCaixa.Loja.LojaID);
            }
            else if (movimentoCaixa.LojaID > 0)
            {
                query = query.Where(x => x.LojaID == movimentoCaixa.LojaID);
            }

            if (movimentoCaixa.DataMovimento != DateTime.MinValue)
            {
                query = query.Where(x => EntityFunctions.TruncateTime(x.DataMovimento) == EntityFunctions.TruncateTime(movimentoCaixa.DataMovimento));
            }

            return query
                    .Include(x => x.MovimentoCaixaStatus).Include(x => x.Loja).Include(x => x.Usuario)
                    .OrderByDescending(x => x.DataMovimento)
                    .ToList();
        }
    }
}

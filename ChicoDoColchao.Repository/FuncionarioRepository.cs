using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class FuncionarioRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public FuncionarioRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(Funcionario funcionario)
        {
            funcionario.Ativo = true;

            chicoDoColchaoEntities.Entry(funcionario).State = EntityState.Added;
            chicoDoColchaoEntities.Entry(funcionario.Loja).State = EntityState.Detached;

            chicoDoColchaoEntities.SaveChanges();
        }

        public List<Funcionario> Listar(Funcionario funcionario)
        {
            IQueryable<Funcionario> query = chicoDoColchaoEntities.Funcionario;
            
            if (funcionario.FuncionarioID > 0)
            {
                query = query.Where(x => x.FuncionarioID == funcionario.FuncionarioID);
            }

            if (funcionario.Numero > 0)
            {
                query = query.Where(x => x.Numero == funcionario.Numero);
            }

            if (funcionario.Loja != null)
            {
                query = query.Where(x => x.Loja.LojaID == funcionario.Loja.LojaID);
            }

            query = query.Where(x => x.Ativo);

            return query.Include(x => x.Loja).OrderBy(x => x.Nome).ToList();
            //return query.OrderBy(x => x.Nome).ToList();
        }
    }
}

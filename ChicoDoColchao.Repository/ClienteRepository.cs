using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class ClienteRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public ClienteRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public List<Cliente> Listar(Cliente cliente)
        {
            IQueryable<Cliente> query = chicoDoColchaoEntities.Cliente;

            if (cliente.ClienteID > 0)
            {
                query = query.Where(x => x.ClienteID.Equals(cliente.ClienteID));
            }

            if (!string.IsNullOrEmpty(cliente.Cpf))
            {
                query = query.Where(x => x.Cpf.Equals(cliente.Cpf));
            }

            if (!string.IsNullOrEmpty(cliente.Cnpj))
            {
                query = query.Where(x => x.Cnpj.Equals(cliente.Cnpj));
            }

            query = query.Where(x => x.Ativo);

            return query.Include(x => x.Estado).OrderBy(x => x.Nome).ToList();
        }

        public List<Cliente> ListarAutocomplete(Cliente cliente)
        {
            IQueryable<Cliente> query = chicoDoColchaoEntities.Cliente;
            
            query = query.Where(x => x.Cpf.Contains(cliente.Cpf) || x.Cnpj.Contains(cliente.Cnpj));
            query = query.Where(x => x.Ativo);

            return query.Include(x => x.Estado).OrderBy(x => x.Nome).ToList();
        }

        public void Incluir(Cliente cliente)
        {
            cliente.Ativo = true;

            chicoDoColchaoEntities.Entry(cliente).State = EntityState.Added;
            chicoDoColchaoEntities.SaveChanges();
        }
    }
}

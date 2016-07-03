using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Repository
{
    public class PedidoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public PedidoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = System.Data.Entity.EntityState.Added;
            chicoDoColchaoEntities.SaveChanges();
        }

        public List<Pedido> Listar(Pedido pedido)
        {
            IQueryable<Pedido> query = null;

            if (pedido.PedidoID > 0)
            {
                query = chicoDoColchaoEntities.Pedido.Where(x => x.PedidoID == pedido.PedidoID);
            }

            if (pedido.ClienteID > 0)
            {
                query = chicoDoColchaoEntities.Pedido.Where(x => x.ClienteID == pedido.ClienteID);
            }

            return query.OrderByDescending(x => x.PedidoID).ToList();
        }

        public void Atualizar(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = System.Data.Entity.EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();
        }

        public void Excluir(Pedido pedido)
        {
            chicoDoColchaoEntities.Entry(pedido).State = System.Data.Entity.EntityState.Modified;
            chicoDoColchaoEntities.SaveChanges();
        }
    }
}

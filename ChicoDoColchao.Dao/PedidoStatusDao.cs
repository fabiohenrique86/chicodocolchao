using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class PedidoStatusDao
    {
        public PedidoStatusDao()
        {
            PedidoDao = new HashSet<PedidoStatusDao>();
        }

        public int PedidoStatusID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    
        public virtual ICollection<PedidoStatusDao> PedidoDao { get; set; }

        public enum EPedidoStatus
        {
            PrevisaoDeEntrega = 1,
            RetiradoNaLoja = 2,
            Cancelado = 3,
            Entregue = 4
        }
    }
}

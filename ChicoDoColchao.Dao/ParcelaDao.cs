using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class ParcelaDao
    {
        public ParcelaDao()
        {
            PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
        }

        public int ParcelaID { get; set; }
        public int Numero { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }
    }
}

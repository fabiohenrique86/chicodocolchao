using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class ParcelaDao
    {
        public ParcelaDao()
        {
            this.PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
        }

        public int ParcelaID { get; set; }
        public int Numero { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }
    }
}

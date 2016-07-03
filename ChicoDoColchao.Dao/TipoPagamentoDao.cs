using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class TipoPagamentoDao
    {
        public TipoPagamentoDao()
        {
            this.PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
        }

        public int TipoPagamentoID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }
    }
}

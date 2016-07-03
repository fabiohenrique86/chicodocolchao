using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class PedidoStatusDao
    {
        public PedidoStatusDao()
        {
            this.PedidoDao = new HashSet<PedidoDao>();
        }

        public int PedidoStatusID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    
        public virtual ICollection<PedidoDao> PedidoDao { get; set; }
    }
}

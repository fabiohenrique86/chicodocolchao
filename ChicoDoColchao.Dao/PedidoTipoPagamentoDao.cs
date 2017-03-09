using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class PedidoTipoPagamentoDao
    {
        public int PedidoTipoPagamentoID { get; set; }
        public int PedidoID { get; set; }
        //public int TipoPagamentoID { get; set; }
        //public int ParcelaID { get; set; }
        public double ValorPago { get; set; }

        public virtual ParcelaDao ParcelaDao { get; set; }
        public virtual TipoPagamentoDao TipoPagamentoDao { get; set; }
    }
}

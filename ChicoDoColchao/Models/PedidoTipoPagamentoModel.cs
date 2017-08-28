using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class PedidoTipoPagamentoModel
    {
        public int PedidoTipoPagamentoID { get; set; }
        public int PedidoID { get; set; }
        public int TipoPagamentoID { get; set; }
        public int ParcelaID { get; set; }
        public double ValorPago { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class PedidoProdutoModel
    {
        public int PedidoProdutoID { get; set; }
        public int PedidoID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }        
    }
}
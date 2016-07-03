using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
   public class PedidoProdutoDao
    {
        public int PedidoProdutoID { get; set; }
        public int PedidoID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }

        public virtual PedidoDao PedidoDao { get; set; }
        public virtual ProdutoDao ProdutoDao { get; set; }
    }
}

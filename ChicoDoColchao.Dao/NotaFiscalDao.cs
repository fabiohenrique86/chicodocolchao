using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class NotaFiscalDao
    {
        public NotaFiscalDao()
        {
            this.NotaFiscalProdutoDao = new HashSet<NotaFiscalProdutoDao>();
        }

        public int NotaFiscalID { get; set; }
        public int Numero { get; set; }
        public System.DateTime DataNotaFiscal { get; set; }
        public int LojaID { get; set; }
        public Nullable<int> PedidoMaeID { get; set; }
        public bool Ativo { get; set; }

        public virtual LojaDao LojaDao { get; set; }
        public virtual ICollection<NotaFiscalProdutoDao> NotaFiscalProdutoDao { get; set; }
    }
}

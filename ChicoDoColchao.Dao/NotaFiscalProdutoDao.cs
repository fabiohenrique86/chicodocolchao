using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class NotaFiscalProdutoDao
    {
        public int NotaFiscalProdutoID { get; set; }
        public int NotaFiscalID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }

        public virtual NotaFiscalDao NotaFiscalDao { get; set; }
        public virtual ProdutoDao ProdutoDao { get; set; }
    }
}

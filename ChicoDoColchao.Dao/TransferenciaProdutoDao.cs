using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
   public class TransferenciaProdutoDao
    {
        public int TransferenciaProdutoID { get; set; }
        public int TransferenciaID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }

        public virtual ProdutoDao ProdutoDao { get; set; }
        public virtual TransferenciaDao TransferenciaDao { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class TransferenciaDao
    {
        public TransferenciaDao()
        {
            this.TransferenciaProdutoDao = new HashSet<TransferenciaProdutoDao>();
        }

        public int TransferenciaID { get; set; }
        public int LojaOrigemID { get; set; }
        public int LojaDestinoID { get; set; }
        public System.DateTime DataTransferencia { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }

        public virtual LojaDao LojaDao { get; set; }
        public virtual ICollection<TransferenciaProdutoDao> TransferenciaProdutoDao { get; set; }
    }
}

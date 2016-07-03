using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class ProdutoDao
    {
        public ProdutoDao()
        {
            this.LojaProdutoDao = new HashSet<LojaProdutoDao>();
            this.NotaFiscalProdutoDao = new HashSet<NotaFiscalProdutoDao>();
            this.OrcamentoProdutoDao = new HashSet<OrcamentoProdutoDao>();
            this.PedidoProdutoDao = new HashSet<PedidoProdutoDao>();
            this.TransferenciaProdutoDao = new HashSet<TransferenciaProdutoDao>();
        }

        public int ProdutoID { get; set; }
        public long Numero { get; set; }
        public int LinhaID { get; set; }
        public string Descricao { get; set; }
        public int MedidaID { get; set; }
        public short ComissaoFuncionario { get; set; }
        public short ComissaoFranqueado { get; set; }
        public bool Ativo { get; set; }

        public virtual LinhaDao LinhaDao { get; set; }
        public virtual ICollection<LojaProdutoDao> LojaProdutoDao { get; set; }
        public virtual MedidaDao MedidaDao { get; set; }
        public virtual ICollection<NotaFiscalProdutoDao> NotaFiscalProdutoDao { get; set; }
        public virtual ICollection<OrcamentoProdutoDao> OrcamentoProdutoDao { get; set; }
        public virtual ICollection<PedidoProdutoDao> PedidoProdutoDao { get; set; }
        public virtual ICollection<TransferenciaProdutoDao> TransferenciaProdutoDao { get; set; }
    }
}

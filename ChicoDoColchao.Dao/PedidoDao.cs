using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class PedidoDao
    {
        public PedidoDao()
        {
            this.PedidoProdutoDao = new HashSet<PedidoProdutoDao>();
            this.PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
        }

        public int PedidoID { get; set; }
        public int Numero { get; set; }
        public int FuncionarioID { get; set; }
        public int ClienteID { get; set; }
        public System.DateTime DataPedido { get; set; }
        public Nullable<System.DateTime> DataEntrega { get; set; }
        public int LojaOrigemID { get; set; }
        public int LojaSaidaID { get; set; }
        public int PedidoStatusID { get; set; }
        public string Observacao { get; set; }
        public string NomeCarreto { get; set; }
        public Nullable<double> ValorFrete { get; set; }

        public virtual ClienteDao ClienteDao { get; set; }
        public virtual FuncionarioDao FuncionarioDao { get; set; }
        public virtual LojaDao LojaOrigemDao { get; set; }
        public virtual LojaDao LojaSaidaDao { get; set; }
        public virtual PedidoStatusDao PedidoStatusDao { get; set; }
        public virtual ICollection<PedidoProdutoDao> PedidoProdutoDao { get; set; }
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }
    }
}

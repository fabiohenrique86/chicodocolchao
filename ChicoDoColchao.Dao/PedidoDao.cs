using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class PedidoDao
    {
        public PedidoDao()
        {
            ClienteDao = new HashSet<ClienteDao>();
            ConsultorDao = new HashSet<ConsultorDao>();
            LojaSaidaDao = new HashSet<LojaDao>();
            LojaDao = new HashSet<LojaDao>();
            PedidoProdutoDao = new HashSet<PedidoProdutoDao>();
            PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
            PedidoStatusDao = new HashSet<PedidoStatusDao>();
            TipoPagamentoDao = new HashSet<TipoPagamentoDao>();
            UsuarioPedidoDao = new UsuarioDao();
            UsuarioCancelamentoDao = new UsuarioDao();
            OrcamentoDao = new HashSet<OrcamentoDao>();
        }

        public int PedidoID { get; set; }
        public DateTime DataPedido { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string Observacao { get; set; }
        public string NomeCarreto { get; set; }
        public double? ValorFrete { get; set; }
        public double Desconto { get; set; }
        public int? PedidoTrocaID { get; set; }

        public virtual ICollection<ClienteDao> ClienteDao { get; set; }
        public virtual ICollection<ConsultorDao> ConsultorDao { get; set; }
        public virtual ICollection<LojaDao> LojaSaidaDao { get; set; }
        public virtual ICollection<LojaDao> LojaDao { get; set; }
        public virtual ICollection<PedidoStatusDao> PedidoStatusDao { get; set; }
        public virtual ICollection<PedidoProdutoDao> PedidoProdutoDao { get; set; }
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }
        public virtual ICollection<TipoPagamentoDao> TipoPagamentoDao { get; set; }
        public virtual ICollection<OrcamentoDao> OrcamentoDao { get; set; }
        public virtual UsuarioDao UsuarioPedidoDao { get; set; }
        public virtual UsuarioDao UsuarioCancelamentoDao { get; set; }
        
        public string DataPedidoInicio { get; set; }
        public string DataPedidoFim { get; set; }

        public string DataEntregaInicio { get; set; }
        public string DataEntregaFim { get; set; }
    }
}

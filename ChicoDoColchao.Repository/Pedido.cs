//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChicoDoColchao.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedido()
        {
            this.PedidoTroca = new HashSet<Pedido>();
            this.PedidoProduto = new HashSet<PedidoProduto>();
            this.PedidoTipoPagamento = new HashSet<PedidoTipoPagamento>();
            this.Orcamento = new HashSet<Orcamento>();
        }
    
        public int PedidoID { get; set; }
        public int FuncionarioID { get; set; }
        public System.DateTime DataPedido { get; set; }
        public int LojaSaidaID { get; set; }
        public int PedidoStatusID { get; set; }
        public string Observacao { get; set; }
        public string NomeCarreto { get; set; }
        public Nullable<double> ValorFrete { get; set; }
        public int ClienteID { get; set; }
        public int LojaID { get; set; }
        public double Desconto { get; set; }
        public Nullable<int> UsuarioPedidoID { get; set; }
        public Nullable<System.DateTime> DataCancelamento { get; set; }
        public Nullable<int> UsuarioCancelamentoID { get; set; }
        public Nullable<int> PedidoTrocaID { get; set; }
        public Nullable<int> TipoPagamentoFreteID { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        public virtual Funcionario Funcionario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> PedidoTroca { get; set; }
        public virtual Pedido Pedido2 { get; set; }
        public virtual PedidoStatus PedidoStatus { get; set; }
        public virtual Usuario UsuarioCancelamento { get; set; }
        public virtual Usuario UsuarioPedido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedidoProduto> PedidoProduto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedidoTipoPagamento> PedidoTipoPagamento { get; set; }
        public virtual Loja LojaOrigem { get; set; }
        public virtual Loja LojaSaida { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orcamento> Orcamento { get; set; }
    }
}

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
    
    public partial class Produto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Produto()
        {
            this.LojaProduto = new HashSet<LojaProduto>();
            this.OrcamentoProduto = new HashSet<OrcamentoProduto>();
            this.PedidoProduto = new HashSet<PedidoProduto>();
            this.TransferenciaProduto = new HashSet<TransferenciaProduto>();
            this.NotaFiscalProduto = new HashSet<NotaFiscalProduto>();
        }
    
        public int ProdutoID { get; set; }
        public long Numero { get; set; }
        public int CategoriaID { get; set; }
        public string Descricao { get; set; }
        public int MedidaID { get; set; }
        public short ComissaoFuncionario { get; set; }
        public short ComissaoFranqueado { get; set; }
        public bool Ativo { get; set; }
        public double Preco { get; set; }
    
        public virtual Categoria Categoria { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LojaProduto> LojaProduto { get; set; }
        public virtual Medida Medida { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrcamentoProduto> OrcamentoProduto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PedidoProduto> PedidoProduto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TransferenciaProduto> TransferenciaProduto { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotaFiscalProduto> NotaFiscalProduto { get; set; }
    }
}

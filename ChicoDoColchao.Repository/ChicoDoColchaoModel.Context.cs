﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ChicoDoColchaoEntities : DbContext
    {
        public ChicoDoColchaoEntities()
            : base("name=ChicoDoColchaoEntities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<Cliente> Cliente { get; set; }
        public virtual DbSet<Estado> Estado { get; set; }
        public virtual DbSet<Funcionario> Funcionario { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<LojaProduto> LojaProduto { get; set; }
        public virtual DbSet<Medida> Medida { get; set; }
        public virtual DbSet<OrcamentoProduto> OrcamentoProduto { get; set; }
        public virtual DbSet<Parcela> Parcela { get; set; }
        public virtual DbSet<PedidoProduto> PedidoProduto { get; set; }
        public virtual DbSet<PedidoStatus> PedidoStatus { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<TipoPagamento> TipoPagamento { get; set; }
        public virtual DbSet<TipoUsuario> TipoUsuario { get; set; }
        public virtual DbSet<Transferencia> Transferencia { get; set; }
        public virtual DbSet<TransferenciaProduto> TransferenciaProduto { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<OrcamentoHistorico> OrcamentoHistorico { get; set; }
        public virtual DbSet<PedidoTipoPagamento> PedidoTipoPagamento { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<Loja> Loja { get; set; }
        public virtual DbSet<MovimentoCaixaStatus> MovimentoCaixaStatus { get; set; }
        public virtual DbSet<MovimentoCaixa> MovimentoCaixa { get; set; }
        public virtual DbSet<Orcamento> Orcamento { get; set; }
        public virtual DbSet<NotaFiscal> NotaFiscal { get; set; }
        public virtual DbSet<NotaFiscalProduto> NotaFiscalProduto { get; set; }
    }
}

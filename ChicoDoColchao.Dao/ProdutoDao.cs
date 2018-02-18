using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class ProdutoDao
    {
        public ProdutoDao()
        {
            CategoriaDao = new HashSet<CategoriaDao>();
            LojaProdutoDao = new HashSet<LojaProdutoDao>();
            LojaDao = new HashSet<LojaDao>();
            MedidaDao = new MedidaDao();
            NotaFiscalProdutoDao = new HashSet<NotaFiscalProdutoDao>();
            OrcamentoProdutoDao = new HashSet<OrcamentoProdutoDao>();
            PedidoProdutoDao = new HashSet<PedidoProdutoDao>();
            TransferenciaProdutoDao = new HashSet<TransferenciaProdutoDao>();
        }

        public int ProdutoID { get; set; }
        public long? Numero { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public short? ComissaoFuncionario { get; set; }
        public short? ComissaoFranqueado { get; set; }
        public bool Ativo { get; set; }
        public int Quantidade { get; set; }
        public bool Erro { get; set; }
        public string Mensagem { get; set; }

        // propriedades somente da classe
        public double PrecoAtacado { get { return Preco * 2.2; } }
        public double PrecoAVista { get { return Preco * 2.3; } }
        public double PrecoAte10 { get { return Preco * 2.5; } }
        public double PrecoNormal { get { return Preco * 3.1; } }
        // propriedades somente da classe

        public virtual ICollection<CategoriaDao> CategoriaDao { get; set; }
        public virtual ICollection<LojaProdutoDao> LojaProdutoDao { get; set; }
        public virtual ICollection<LojaDao> LojaDao { get; set; }
        public virtual MedidaDao MedidaDao { get; set; }
        public virtual ICollection<NotaFiscalProdutoDao> NotaFiscalProdutoDao { get; set; }
        public virtual ICollection<OrcamentoProdutoDao> OrcamentoProdutoDao { get; set; }
        public virtual ICollection<PedidoProdutoDao> PedidoProdutoDao { get; set; }
        public virtual ICollection<TransferenciaProdutoDao> TransferenciaProdutoDao { get; set; }
    }
}

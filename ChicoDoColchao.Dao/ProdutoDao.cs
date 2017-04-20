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
            MedidaDao = new MedidaDao();
            NotaFiscalProdutoDao = new HashSet<NotaFiscalProdutoDao>();
            OrcamentoProdutoDao = new HashSet<OrcamentoProdutoDao>();
            PedidoProdutoDao = new HashSet<PedidoProdutoDao>();
            TransferenciaProdutoDao = new HashSet<TransferenciaProdutoDao>();
        }

        public int ProdutoID { get; set; }
        public Nullable<long> Numero { get; set; }
        public string Descricao { get; set; }
        public double Preco { get; set; }
        public Nullable<short> ComissaoFuncionario { get; set; }
        public Nullable<short> ComissaoFranqueado { get; set; }
        public bool Ativo { get; set; }
        public int Quantidade { get; set; }
        public bool Erro { get; set; }
        public string Mensagem { get; set; }

        public virtual ICollection<CategoriaDao> CategoriaDao { get; set; }
        public virtual ICollection<LojaProdutoDao> LojaProdutoDao { get; set; }
        public virtual MedidaDao MedidaDao { get; set; }
        public virtual ICollection<NotaFiscalProdutoDao> NotaFiscalProdutoDao { get; set; }
        public virtual ICollection<OrcamentoProdutoDao> OrcamentoProdutoDao { get; set; }
        public virtual ICollection<PedidoProdutoDao> PedidoProdutoDao { get; set; }
        public virtual ICollection<TransferenciaProdutoDao> TransferenciaProdutoDao { get; set; }
    }
}

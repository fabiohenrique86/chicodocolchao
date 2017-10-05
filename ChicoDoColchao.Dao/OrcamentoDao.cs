using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class OrcamentoDao
    {
        public OrcamentoDao()
        {
            OrcamentoProdutoDao = new HashSet<OrcamentoProdutoDao>();
            OrcamentoHistoricoDao = new HashSet<OrcamentoHistoricoDao>();
            FuncionarioDao = new HashSet<FuncionarioDao>();
            LojaDao = new HashSet<LojaDao>();
        }

        public int OrcamentoID { get; set; }
        public DateTime DataOrcamento { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
        public double Desconto { get; set; }

        public virtual ICollection<FuncionarioDao> FuncionarioDao { get; set; }
        public virtual ClienteDao ClienteDao { get; set; }        
        public virtual ICollection<LojaDao> LojaDao { get; set; }
        public virtual ICollection<OrcamentoProdutoDao> OrcamentoProdutoDao { get; set; }
        public virtual ICollection<OrcamentoHistoricoDao> OrcamentoHistoricoDao { get; set; }
        public virtual PedidoDao PedidoDao { get; set; }
    }
}

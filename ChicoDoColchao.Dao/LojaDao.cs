using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class LojaDao
    {
        public LojaDao()
        {
            this.FuncionarioDao = new HashSet<FuncionarioDao>();
            this.LojaProdutoDao = new HashSet<LojaProdutoDao>();
            this.NotaFiscalDao = new HashSet<NotaFiscalDao>();
            this.OrcamentoDao = new HashSet<OrcamentoDao>();
            this.PedidoDao = new HashSet<PedidoDao>();
            this.TransferenciaDao = new HashSet<TransferenciaDao>();
        }

        public int LojaID { get; set; }
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<FuncionarioDao> FuncionarioDao { get; set; }
        public virtual ICollection<LojaProdutoDao> LojaProdutoDao { get; set; }
        public virtual ICollection<NotaFiscalDao> NotaFiscalDao { get; set; }
        public virtual ICollection<OrcamentoDao> OrcamentoDao { get; set; }
        public virtual ICollection<PedidoDao> PedidoDao { get; set; }
        public virtual ICollection<TransferenciaDao> TransferenciaDao { get; set; }
    }
}

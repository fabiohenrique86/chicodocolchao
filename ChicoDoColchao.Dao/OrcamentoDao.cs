using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class OrcamentoDao
    {
        public OrcamentoDao()
        {
            this.OrcamentoProdutoDao = new HashSet<OrcamentoProdutoDao>();
        }

        public int OrcamentoID { get; set; }
        public int Numero { get; set; }
        public int LojaID { get; set; }
        public int FuncionarioID { get; set; }
        public System.DateTime DataOrcamento { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }

        public virtual FuncionarioDao FuncionarioDao { get; set; }
        public virtual LojaDao LojaDao { get; set; }
        public virtual ICollection<OrcamentoProdutoDao> OrcamentoProdutoDao { get; set; }
    }
}

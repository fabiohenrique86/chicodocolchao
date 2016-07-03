using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class FuncionarioDao
    {
        public FuncionarioDao()
        {
            this.OrcamentoDao = new HashSet<OrcamentoDao>();
            this.PedidoDao = new HashSet<PedidoDao>();
        }

        public int FuncionarioID { get; set; }
        public int Numero { get; set; }
        public int LojaID { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public virtual LojaDao LojaDao { get; set; }
        public virtual ICollection<OrcamentoDao> OrcamentoDao { get; set; }
        public virtual ICollection<PedidoDao> PedidoDao { get; set; }
    }
}

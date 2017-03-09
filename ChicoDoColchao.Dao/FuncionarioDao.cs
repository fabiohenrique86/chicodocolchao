using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChicoDoColchao.Dao
{
    public class FuncionarioDao
    {
        public FuncionarioDao()
        {
            //this.OrcamentoDao = new HashSet<OrcamentoDao>();
            //this.PedidoDao = new HashSet<PedidoDao>();
            this.LojaDao = new HashSet<LojaDao>();
        }

        public int FuncionarioID { get; set; }
        public int? Numero { get; set; }
        //public int LojaID { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<LojaDao> LojaDao { get; set; }
        //public virtual ICollection<OrcamentoDao> OrcamentoDao { get; set; }
        //public virtual ICollection<PedidoDao> PedidoDao { get; set; }
    }
}

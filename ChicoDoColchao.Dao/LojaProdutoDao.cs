using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class LojaProdutoDao
    {
        public int LojaProdutoID { get; set; }
        public int LojaID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public bool Ativo { get; set; }

        public virtual LojaDao LojaDao { get; set; }
        public virtual ProdutoDao ProdutoDao { get; set; }
    }
}

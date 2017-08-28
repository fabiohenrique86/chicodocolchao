using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class LinhaDao
    {
        public LinhaDao()
        {
            this.ProdutoDao = new HashSet<ProdutoDao>();
        }

        public int LinhaID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<ProdutoDao> ProdutoDao { get; set; }
    }
}

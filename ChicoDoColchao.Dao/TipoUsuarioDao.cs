using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Dao
{
    public class TipoUsuarioDao
    {
        public TipoUsuarioDao()
        {
            this.UsuarioDao = new HashSet<UsuarioDao>();
        }

        public int TipoUsuarioID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<UsuarioDao> UsuarioDao { get; set; }
    }
}

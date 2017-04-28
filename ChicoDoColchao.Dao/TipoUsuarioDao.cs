using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class TipoUsuarioDao
    {
        public TipoUsuarioDao()
        {
            UsuarioDao = new HashSet<UsuarioDao>();
        }

        public int TipoUsuarioID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        
        public ICollection<UsuarioDao> UsuarioDao { get; set; }

        public enum ETipoUsuario
        {
            Gerencial = 1,
            Vendedor = 2,
            Deposito = 3
        }
    }
}

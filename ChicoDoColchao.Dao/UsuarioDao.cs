namespace ChicoDoColchao.Dao
{
    public class UsuarioDao
    {
        public UsuarioDao()
        {
            TipoUsuarioDao = new TipoUsuarioDao();
        }

        public int UsuarioID { get; set; }
        public int TipoUsuarioID { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }

        public virtual TipoUsuarioDao TipoUsuarioDao { get; set; }
    }
}

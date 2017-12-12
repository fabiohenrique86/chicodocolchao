using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class FuncionarioDao
    {
        public FuncionarioDao()
        {
            this.LojaDao = new HashSet<LojaDao>();
        }

        public int FuncionarioID { get; set; }
        public int? Numero { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<LojaDao> LojaDao { get; set; }
    }
}

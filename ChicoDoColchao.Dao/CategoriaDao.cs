using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class CategoriaDao
    {
        public CategoriaDao()
        {
            ProdutoDao = new HashSet<ProdutoDao>();
        }

        public int CategoriaID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }

        public virtual ICollection<ProdutoDao> ProdutoDao { get; set; }
    }
}

using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class CategoriaTradutor
    {
        public static Categoria ToBd(this CategoriaDao categoriaDao)
        {
            Categoria categoria = new Categoria();

            categoria.CategoriaID = categoriaDao.CategoriaID;
            if (!string.IsNullOrEmpty(categoriaDao.Descricao))
            {
                categoria.Descricao = categoriaDao.Descricao.Trim();
            }
            categoria.Ativo = categoriaDao.Ativo;

            return categoria;
        }

        public static CategoriaDao ToApp(this Categoria categoria)
        {
            CategoriaDao categoriaDao = new CategoriaDao();

            categoriaDao.CategoriaID = categoria.CategoriaID;
            if (!string.IsNullOrEmpty(categoria.Descricao))
            {
                categoriaDao.Descricao = categoria.Descricao.Trim();
            }
            categoriaDao.Ativo = categoria.Ativo;

            return categoriaDao;
        }
    }
}

using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class LinhaTradutor
    {
        public static Linha ToBd(this LinhaDao linhaDao)
        {
            Linha linha = new Linha();

            linha.LinhaID = linhaDao.LinhaID;
            if (!string.IsNullOrEmpty(linhaDao.Descricao))
            {
                linha.Descricao = linhaDao.Descricao.Trim();
            }
            linha.Ativo = linhaDao.Ativo;

            return linha;
        }

        public static LinhaDao ToApp(this Linha linha)
        {
            LinhaDao linhaDao = new LinhaDao();

            linhaDao.LinhaID = linha.LinhaID;
            if (!string.IsNullOrEmpty(linha.Descricao))
            {
                linhaDao.Descricao = linha.Descricao.Trim();
            }
            linhaDao.Ativo = linha.Ativo;

            return linhaDao;
        }
    }
}

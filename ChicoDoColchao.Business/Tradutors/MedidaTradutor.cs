using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class MedidaTradutor
    {
        public static Medida ToBd(this MedidaDao medidaDao)
        {
            Medida medida = new Medida();

            medida.MedidaID = medidaDao.MedidaID;
            if (!string.IsNullOrEmpty(medidaDao.Descricao))
            {
                medida.Descricao = medidaDao.Descricao.Replace(" ", "").Trim();
            }
            medida.Ativo = medidaDao.Ativo;

            return medida;
        }

        public static MedidaDao ToApp(this Medida medida)
        {
            MedidaDao medidaDao = new MedidaDao();

            medidaDao.MedidaID = medida.MedidaID;
            if (!string.IsNullOrEmpty(medida.Descricao))
            {
                medidaDao.Descricao = medida.Descricao.Replace(" ", "").Trim();
            }
            medidaDao.Ativo = medida.Ativo;

            return medidaDao;
        }
    }
}

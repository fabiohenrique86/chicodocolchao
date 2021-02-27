using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class NotaFiscalTradutor
    {
        public static NotaFiscal ToBd(this NotaFiscalDao notaFiscalDao)
        {
            NotaFiscal notaFiscal = new NotaFiscal();

            notaFiscal.NotaFiscalID = notaFiscalDao.NotaFiscalID;
            notaFiscal.Numero = notaFiscalDao.Numero;
            notaFiscal.DataCadastro = notaFiscalDao.DataCadastro;
            notaFiscal.DataEmissao = notaFiscalDao.DataEmissao;

            if (notaFiscalDao.LojaDao != null && notaFiscalDao.LojaDao.LojaID > 0)
                notaFiscal.LojaID = notaFiscalDao.LojaDao.LojaID;

            return notaFiscal;
        }

        public static NotaFiscalDao ToApp(this NotaFiscal notaFiscal)
        {
            NotaFiscalDao notaFiscalDao = new NotaFiscalDao();

            notaFiscalDao.NotaFiscalID = notaFiscal.NotaFiscalID;
            notaFiscalDao.Numero = notaFiscal.Numero;
            notaFiscalDao.DataCadastro = notaFiscal.DataCadastro;
            notaFiscalDao.DataEmissao = notaFiscal.DataEmissao;
            notaFiscalDao.LojaDao = new LojaDao() { LojaID = notaFiscal.LojaID };

            return notaFiscalDao;
        }
    }
}

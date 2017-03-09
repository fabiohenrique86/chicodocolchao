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
            notaFiscal.DataNotaFiscal = notaFiscalDao.DataNotaFiscal;
            notaFiscal.Loja = new Loja() { LojaID = notaFiscalDao.LojaDao.LojaID };
            notaFiscal.PedidoMaeID = notaFiscalDao.PedidoMaeID;
            notaFiscal.Ativo = notaFiscalDao.Ativo;

            return notaFiscal;
        }

        public static NotaFiscalDao ToApp(this NotaFiscal notaFiscal)
        {
            NotaFiscalDao notaFiscalDao = new NotaFiscalDao();

            notaFiscalDao.NotaFiscalID = notaFiscal.NotaFiscalID;
            notaFiscalDao.Numero = notaFiscal.Numero;
            notaFiscalDao.DataNotaFiscal = notaFiscal.DataNotaFiscal;
            notaFiscalDao.LojaDao = new LojaDao() { LojaID = notaFiscal.LojaID };
            notaFiscalDao.PedidoMaeID = notaFiscal.PedidoMaeID;
            notaFiscalDao.Ativo = notaFiscal.Ativo;

            return notaFiscalDao;
        }
    }
}

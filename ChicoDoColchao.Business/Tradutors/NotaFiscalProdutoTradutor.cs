using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class NotaFiscalProdutoTradutor
    {
        public static NotaFiscalProduto ToBd(this NotaFiscalProdutoDao notaFiscalProdutoDao)
        {
            var notaFiscalProduto = new NotaFiscalProduto();

            notaFiscalProduto.NotaFiscalProdutoID = notaFiscalProdutoDao.NotaFiscalProdutoID;
            notaFiscalProduto.NotaFiscalID = notaFiscalProdutoDao.NotaFiscalID;
            notaFiscalProduto.ProdutoID = notaFiscalProdutoDao.ProdutoDao.ProdutoID;
            notaFiscalProduto.Quantidade = notaFiscalProdutoDao.Quantidade;

            return notaFiscalProduto;
        }

        public static NotaFiscalProdutoDao ToApp(this NotaFiscalProduto notaFiscalProduto)
        {
            var notaFiscalProdutoDao = new NotaFiscalProdutoDao();

            notaFiscalProdutoDao.NotaFiscalProdutoID = notaFiscalProduto.NotaFiscalProdutoID;
            notaFiscalProdutoDao.NotaFiscalID = notaFiscalProduto.NotaFiscalID;
            notaFiscalProdutoDao.ProdutoDao = new ProdutoDao() { ProdutoID = notaFiscalProduto.ProdutoID, Numero = notaFiscalProduto.Produto.Numero };
            notaFiscalProdutoDao.Quantidade = notaFiscalProduto.Quantidade;

            return notaFiscalProdutoDao;
        }
    }
}

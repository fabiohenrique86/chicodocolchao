using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;

namespace ChicoDoColchao.Business
{
    public class NotaFiscalProdutoBusiness
    {
        NotaFiscalProdutoRepository notaFiscalProdutoRepository;
        LogRepository logRepository;

        public NotaFiscalProdutoBusiness()
        {
            notaFiscalProdutoRepository = new NotaFiscalProdutoRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(NotaFiscalProdutoDao notaFiscalProdutoDao)
        {
            if (notaFiscalProdutoDao == null)
                throw new BusinessException("Produto é obrigatório");

            if (notaFiscalProdutoDao.NotaFiscalID <= 0)
                throw new BusinessException("Nota Fiscal é obrigatório");

            if (notaFiscalProdutoDao.ProdutoDao == null || notaFiscalProdutoDao.ProdutoDao.ProdutoID <= 0)
                throw new BusinessException("Produto é obrigatório");
        }

        public void Incluir(NotaFiscalProdutoDao notaFiscalProdutoDao)
        {
            try
            {
                ValidarIncluir(notaFiscalProdutoDao);

                notaFiscalProdutoRepository.Incluir(notaFiscalProdutoDao.ToBd());
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}

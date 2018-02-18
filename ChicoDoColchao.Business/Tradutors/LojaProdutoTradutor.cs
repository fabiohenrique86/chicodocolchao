using System;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class LojaProdutoTradutor
    {
        public static LojaProduto ToBd(this LojaProdutoDao lojaProdutoDao)
        {
            var lojaProduto = new LojaProduto();

            lojaProduto.LojaProdutoID = lojaProdutoDao.LojaProdutoID;
            lojaProduto.LojaID = lojaProdutoDao.LojaID;
            lojaProduto.ProdutoID = lojaProdutoDao.ProdutoID;
            lojaProduto.Quantidade = lojaProdutoDao.Quantidade;
            lojaProduto.Ativo = lojaProdutoDao.Ativo;

            return lojaProduto;
        }

        public static LojaProdutoDao ToApp(this LojaProduto lojaProduto)
        {
            var lojaProdutoDao = new LojaProdutoDao();

            lojaProdutoDao.LojaProdutoID = lojaProduto.LojaProdutoID;
            lojaProdutoDao.LojaID = lojaProduto.LojaID;
            lojaProdutoDao.ProdutoID = lojaProduto.ProdutoID;
            lojaProdutoDao.Quantidade = lojaProduto.Quantidade;
            lojaProdutoDao.Ativo = lojaProduto.Ativo;

            return lojaProdutoDao;
        }
    }
}

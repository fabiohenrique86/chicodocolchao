using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Models;

namespace ChicoDoColchao.Tradutors
{
    public static class ProdutoTradutor
    {
        public static ProdutoDao ToDao(this ProdutoModel produtoModel)
        {
            ProdutoDao produtoDao = new ProdutoDao();

            produtoDao.ProdutoID = produtoModel.ProdutoID;
            produtoDao.Numero = produtoModel.Numero;
            produtoDao.LinhaID = produtoModel.LinhaModel.LinhaID;
            produtoDao.Descricao = produtoModel.Descricao;
            produtoDao.MedidaID = produtoModel.MedidaModel.MedidaID;
            produtoDao.Preco = produtoModel.Preco;
            produtoDao.ComissaoFuncionario = produtoModel.ComissaoFuncionario;
            produtoDao.ComissaoFranqueado = produtoModel.ComissaoFranqueado;
            produtoDao.Ativo = produtoModel.Ativo;

            return produtoDao;
        }

        public static ProdutoModel ToModel(this ProdutoDao produtoDao)
        {
            ProdutoModel produtoModel = new ProdutoModel();

            produtoModel.ProdutoID = produtoDao.ProdutoID;
            produtoModel.Numero = produtoDao.Numero;
            produtoModel.LinhaModel.LinhaID = produtoDao.LinhaDao.LinhaID;
            produtoModel.LinhaModel.Descricao = produtoDao.LinhaDao.Descricao;
            produtoModel.Descricao = produtoDao.Descricao;
            produtoModel.MedidaModel.MedidaID = produtoDao.MedidaDao.MedidaID;
            produtoModel.MedidaModel.Descricao = produtoDao.MedidaDao.Descricao;
            produtoModel.Preco = produtoDao.Preco;
            produtoModel.ComissaoFuncionario = produtoDao.ComissaoFuncionario;
            produtoModel.ComissaoFranqueado = produtoDao.ComissaoFranqueado;
            produtoModel.Ativo = produtoDao.Ativo;

            return produtoModel;
        }
    }
}
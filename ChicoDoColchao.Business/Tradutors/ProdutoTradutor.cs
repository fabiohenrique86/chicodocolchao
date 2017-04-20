using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class ProdutoTradutor
    {
        public static Produto ToBd(this ProdutoDao produtoDao)
        {
            Produto produto = new Produto();

            produto.ProdutoID = produtoDao.ProdutoID;
            produto.Numero = produtoDao.Numero.HasValue ? produtoDao.Numero.Value : 0;
            if (produtoDao.CategoriaDao != null && produtoDao.CategoriaDao.Count() > 0)
            {
                produto.CategoriaID = produtoDao.CategoriaDao.FirstOrDefault().CategoriaID;
            }
            produto.Descricao = produtoDao.Descricao;
            produto.MedidaID = produtoDao.MedidaDao.MedidaID;
            produto.ComissaoFuncionario = produtoDao.ComissaoFuncionario.HasValue ? produtoDao.ComissaoFuncionario.Value : Convert.ToInt16(0);
            produto.ComissaoFranqueado = produtoDao.ComissaoFranqueado.HasValue ? produtoDao.ComissaoFranqueado.Value : Convert.ToInt16(0);
            produto.Ativo = produtoDao.Ativo;
            produto.Preco = produtoDao.Preco;

            foreach (var lojaProdutoDao in produtoDao.LojaProdutoDao)
            {
                LojaProduto lojaProduto = new LojaProduto();

                lojaProduto.ProdutoID = produtoDao.ProdutoID;
                lojaProduto.LojaID = lojaProdutoDao.LojaID;
                lojaProduto.Quantidade = lojaProdutoDao.Quantidade;
                lojaProduto.Ativo = lojaProdutoDao.Ativo;

                produto.LojaProduto.Add(lojaProduto);
            }

            return produto;
        }

        public static ProdutoDao ToApp(this Produto produto)
        {
            ProdutoDao produtoDao = new ProdutoDao();

            produtoDao.ProdutoID = produto.ProdutoID;
            produtoDao.Numero = produto.Numero;
            produtoDao.CategoriaDao.Add(new CategoriaDao() { CategoriaID = produto.Categoria.CategoriaID, Descricao = produto.Categoria.Descricao });
            produtoDao.Descricao = produto.Descricao;
            produtoDao.MedidaDao.MedidaID = produto.Medida.MedidaID;
            produtoDao.MedidaDao.Descricao = produto.Medida.Descricao;
            produtoDao.ComissaoFuncionario = produto.ComissaoFuncionario;
            produtoDao.ComissaoFranqueado = produto.ComissaoFranqueado;
            produtoDao.Ativo = produto.Ativo;
            produtoDao.Preco = produto.Preco;

            foreach (var lojaProduto in produto.LojaProduto.Where(x => x.Ativo).OrderBy(x => x.Loja.NomeFantasia))
            {
                LojaProdutoDao lojaProdutoDao = new LojaProdutoDao();

                lojaProdutoDao.LojaProdutoID = lojaProduto.LojaProdutoID;
                lojaProdutoDao.LojaID = lojaProduto.LojaID;
                lojaProdutoDao.LojaDao = new LojaDao() { LojaID = lojaProduto.LojaID, NomeFantasia = lojaProduto.Loja.NomeFantasia };
                lojaProdutoDao.ProdutoID = lojaProduto.ProdutoID;
                lojaProdutoDao.Quantidade = lojaProduto.Quantidade;
                lojaProdutoDao.Ativo = lojaProduto.Ativo;

                produtoDao.LojaProdutoDao.Add(lojaProdutoDao);
            }

            return produtoDao;
        }
    }
}

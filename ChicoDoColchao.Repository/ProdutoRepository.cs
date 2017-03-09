﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class ProdutoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public ProdutoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(Produto produto)
        {
            produto.Ativo = true;

            chicoDoColchaoEntities.Entry(produto).State = EntityState.Added;
                        
            if (produto.LojaProduto != null)
            {
                var lp = produto.LojaProduto.FirstOrDefault();
                var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.LojaID == lp.LojaID && x.Produto.Numero == produto.Numero && x.Ativo == true);
                // caso produto exista na loja, atualiza a quantidade da loja
                // caso contrário, cadastra-o
                if (lojaProduto == null)
                {
                    //var l = new LojaProduto() { ProdutoID = produto.ProdutoID, LojaID = lp.LojaID, Quantidade = lp.Quantidade, Ativo = true };
                    chicoDoColchaoEntities.Entry(produto.LojaProduto.FirstOrDefault()).State = EntityState.Added;
                    
                }
                else
                {
                    lojaProduto.Quantidade += lp.Quantidade;
                    chicoDoColchaoEntities.Entry(produto.LojaProduto.FirstOrDefault()).State = EntityState.Modified;
                }
            }
            else
            {
                // associa o produto à TODAS as lojas cadastradas
                var lojas = chicoDoColchaoEntities.Loja;
                foreach (var loja in lojas)
                {
                    var lojaProduto = new LojaProduto() { ProdutoID = produto.ProdutoID, LojaID = loja.LojaID, Quantidade = 0, Ativo = true };
                    chicoDoColchaoEntities.Entry(lojaProduto).State = EntityState.Added;
                }
            }
            
            chicoDoColchaoEntities.SaveChanges();
        }

        public void Transferir(int lojaOrigemId, int lojaDestinoId, int produtoId, int quantidade)
        {
            // retira a quantidade da loja de origem
            var lojaProdutoOrigem = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.ProdutoID == produtoId && x.LojaID == lojaOrigemId && x.Ativo == true);
            if (lojaProdutoOrigem != null) { lojaProdutoOrigem.Quantidade = Convert.ToInt16(lojaProdutoOrigem.Quantidade - quantidade); }

            // adiciona a quantidade da loja de destino
            var lojaProdutoDestino = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.ProdutoID == produtoId && x.LojaID == lojaDestinoId && x.Ativo == true);
            if (lojaProdutoDestino != null) { lojaProdutoDestino.Quantidade = Convert.ToInt16(lojaProdutoDestino.Quantidade + quantidade); }

            chicoDoColchaoEntities.SaveChanges();
        }

        public int Atualizar(int lojaDestinoId, long numero, int quantidade)
        {
            int retorno = 0;

            // adiciona a quantidade da loja de destino
            var lojaProdutoDestino = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.Produto.Numero == numero && x.LojaID == lojaDestinoId && x.Ativo == true);
            if (lojaProdutoDestino != null) { lojaProdutoDestino.Quantidade = Convert.ToInt16(lojaProdutoDestino.Quantidade + quantidade); retorno++; }

            chicoDoColchaoEntities.SaveChanges();

            return retorno;
        }

        public List<Produto> Listar(Produto produto, int lojaOrigemId = 0, int lojaDestinoId = 0)
        {
            IQueryable<Produto> query = chicoDoColchaoEntities.Produto;

            if (produto.ProdutoID > 0)
            {
                query = query.Where(x => x.ProdutoID == produto.ProdutoID);
            }

            if (produto.Numero > 0)
            {
                query = query.Where(x => x.Numero == produto.Numero);
            }

            if (!string.IsNullOrEmpty(produto.Descricao))
            {
                query = query.Where(x => x.Descricao.Contains(produto.Descricao));
            }

            if (lojaOrigemId > 0)
            {
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaOrigemId));
            }

            if (lojaDestinoId > 0)
            {
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaDestinoId));
            }

            query = query.Where(x => x.Ativo == true);

            return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

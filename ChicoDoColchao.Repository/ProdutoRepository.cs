using System;
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

            // caso produto exista na loja, atualiza a quantidade da loja
            // caso contrário, cadastra-o
            if (produto.LojaProduto != null && produto.LojaProduto.Count() > 0)
            {
                var lp = produto.LojaProduto.FirstOrDefault();
                var lojaProduto = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.LojaID == lp.LojaID && x.Produto.Numero == produto.Numero && x.Ativo == true);

                if (lojaProduto == null)
                {
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

        public void Atualizar(Produto produto)
        {
            var p = chicoDoColchaoEntities.Produto.SingleOrDefault(x => x.Numero == produto.Numero);

            if (!string.IsNullOrEmpty(produto.Descricao)) { p.Descricao = produto.Descricao.Trim(); }
            if (produto.CategoriaID > 0) { p.CategoriaID = produto.CategoriaID; }
            if (produto.MedidaID > 0) { p.MedidaID = produto.MedidaID; }
            if (produto.ComissaoFuncionario > 0) { p.ComissaoFuncionario = produto.ComissaoFuncionario; }
            if (produto.ComissaoFranqueado > 0) { p.ComissaoFranqueado = produto.ComissaoFranqueado; }
            if (produto.Preco > 0) { p.Preco = produto.Preco; }

            chicoDoColchaoEntities.Entry(p).State = EntityState.Modified;

            foreach (var lojaProduto in produto.LojaProduto)
            {
                var lp = chicoDoColchaoEntities.LojaProduto.FirstOrDefault(x => x.LojaID == lojaProduto.LojaID && x.Produto.Numero == produto.Numero && x.Ativo == true);

                if (lp != null)
                {
                    lp.Quantidade += lojaProduto.Quantidade;
                    chicoDoColchaoEntities.Entry(lp).State = EntityState.Modified;
                }
            }

            chicoDoColchaoEntities.SaveChanges();
        }

        public void Excluir(Produto produto)
        {
            var p = chicoDoColchaoEntities.Produto.FirstOrDefault(x => x.ProdutoID == produto.ProdutoID);

            if (p != null)
            {
                p.Ativo = false;
                chicoDoColchaoEntities.Entry(p).State = EntityState.Modified;
                chicoDoColchaoEntities.SaveChanges();
            }
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

        public bool Atualizar(int lojaDestinoId, long numero, int quantidade, double preco = 0)
        {
            bool retorno = false;

            // adiciona a quantidade da loja de destino
            var lojaProdutoDestino = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.Produto.Numero == numero && x.LojaID == lojaDestinoId && x.Ativo == true);
            if (lojaProdutoDestino != null)
            {
                lojaProdutoDestino.Quantidade = Convert.ToInt16(lojaProdutoDestino.Quantidade + quantidade);
                retorno = true;
            }

            // atualiza o produto para ativo = true
            var produto = chicoDoColchaoEntities.Produto.Where(x => x.Numero == numero).FirstOrDefault();
            if (produto != null)
            {
                produto.Ativo = true;
                if (preco > 0)
                {
                    produto.Preco = preco;
                }
            }

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

            if (produto.Ativo)
            {
                query = query.Where(x => x.Ativo == produto.Ativo);
            }

            return query.Include(x => x.LojaProduto.Select(w => w.Loja))
                        .Include(x => x.Categoria)
                        .Include(x => x.Medida)
                        .OrderBy(x => x.Descricao).ToList();
            
            // return query.OrderBy(x => x.Descricao).ToList();
        }
    }
}

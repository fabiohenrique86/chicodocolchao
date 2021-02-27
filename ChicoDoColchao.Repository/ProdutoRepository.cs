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

        public int Incluir(Produto produto)
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
                    chicoDoColchaoEntities.Entry(produto.LojaProduto.FirstOrDefault()).State = EntityState.Added;
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

            return produto.ProdutoID;
        }

        public void Atualizar(Produto produto)
        {
            var c1 = new ChicoDoColchaoEntities();
            var p = c1.Produto.SingleOrDefault(x => x.Numero == produto.Numero && x.Ativo == true);

            if (p != null)
            {
                if (!string.IsNullOrEmpty(produto.Descricao))
                    p.Descricao = produto.Descricao.Trim();

                if (produto.CategoriaID > 0)
                    p.CategoriaID = produto.CategoriaID;

                if (produto.MedidaID > 0)
                    p.MedidaID = produto.MedidaID;

                if (produto.ComissaoFuncionario > 0)
                    p.ComissaoFuncionario = produto.ComissaoFuncionario;

                if (produto.ComissaoFranqueado > 0)
                    p.ComissaoFranqueado = produto.ComissaoFranqueado;

                if (produto.Preco > 0)
                    p.Preco = produto.Preco;

                c1.SaveChanges();
            }

            var c2 = new ChicoDoColchaoEntities();
            foreach (var lojaProduto in produto.LojaProduto)
            {
                var lp = c2.LojaProduto.SingleOrDefault(x => x.LojaID == lojaProduto.LojaID && x.Produto.Numero == produto.Numero && x.Ativo == true);

                if (lp != null)
                    lp.Quantidade = Convert.ToInt16(lp.Quantidade + lojaProduto.Quantidade);
            }

            c2.SaveChanges();
        }

        public void Excluir(Produto produto)
        {
            var p = chicoDoColchaoEntities.Produto.FirstOrDefault(x => x.ProdutoID == produto.ProdutoID);

            if (p != null)
            {
                p.Ativo = false;

                if (p.LojaProduto == null || p.LojaProduto.Count() <= 0)
                    p.LojaProduto = chicoDoColchaoEntities.LojaProduto.Where(x => x.ProdutoID == p.ProdutoID).ToList();

                // seta para inativo o produto em todas as lojas
                foreach (var lojaProduto in p.LojaProduto)
                    lojaProduto.Ativo = false;

                chicoDoColchaoEntities.SaveChanges();
            }
        }

        public void Transferir(int lojaOrigemId, int lojaDestinoId, int produtoId, int quantidade)
        {
            // retira a quantidade da loja de origem
            var lojaProdutoOrigem = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.ProdutoID == produtoId && x.LojaID == lojaOrigemId && x.Ativo == true);

            if (lojaProdutoOrigem != null)
                lojaProdutoOrigem.Quantidade = Convert.ToInt16(lojaProdutoOrigem.Quantidade - quantidade);

            // adiciona a quantidade da loja de destino
            var lojaProdutoDestino = chicoDoColchaoEntities.LojaProduto.SingleOrDefault(x => x.ProdutoID == produtoId && x.LojaID == lojaDestinoId && x.Ativo == true);

            if (lojaProdutoDestino != null)
                lojaProdutoDestino.Quantidade = Convert.ToInt16(lojaProdutoDestino.Quantidade + quantidade);

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
                    produto.Preco = preco;
            }

            chicoDoColchaoEntities.SaveChanges();

            return retorno;
        }

        public List<Produto> Listar(Produto produto, int lojaOrigemId = 0, int lojaDestinoId = 0)
        {
            IQueryable<Produto> query = chicoDoColchaoEntities.Produto;

            if (produto.ProdutoID > 0)
                query = query.Where(x => x.ProdutoID == produto.ProdutoID);

            if (produto.Numero > 0)
                query = query.Where(x => x.Numero == produto.Numero);

            if (!string.IsNullOrEmpty(produto.Descricao))
                query = query.Where(x => x.Descricao.Contains(produto.Descricao));

            if (lojaOrigemId > 0)
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaOrigemId));

            if (lojaDestinoId > 0)
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaDestinoId));

            if (produto.Ativo)
                query = query.Where(x => x.Ativo == produto.Ativo);

            var lista = query.Include(x => x.LojaProduto.Select(w => w.Loja))
                        .Include(x => x.Categoria)
                        .Include(x => x.Medida)
                        .OrderBy(x => x.Descricao).ToList();

            // retira os produtos / lojasproduto / lojas que estão inativas
            foreach (var p in lista.ToList())
            {
                if (!p.Ativo)
                {
                    lista.Remove(p);
                    continue;
                }

                foreach (var lp in p.LojaProduto.ToList())
                {
                    if (!lp.Loja.Ativo || !lp.Ativo)
                        p.LojaProduto.Remove(lp);

                    if (lojaOrigemId > 0)
                        if (lp.LojaID != lojaOrigemId)
                            p.LojaProduto.Remove(lp);

                    if (lojaDestinoId > 0)
                        if (lp.LojaID != lojaDestinoId)
                            p.LojaProduto.Remove(lp);
                }
            }

            return lista;
        }

        public Produto Listar(int produtoId = 0, long numero = 0)
        {
            IQueryable<Produto> query = chicoDoColchaoEntities.Produto;

            if (produtoId > 0)
                query = query.Where(x => x.ProdutoID == produtoId);

            if (numero > 0)
                query = query.Where(x => x.Numero == numero);

            var lista = query.Include(x => x.Categoria).Include(x => x.Medida).OrderBy(x => x.Descricao).ToList();

            if (lista.Count() > 1)
                lista.RemoveAll(x => !x.Ativo);

            return lista.FirstOrDefault();
        }

        public void Ativar(int produtoId = 0, long numero = 0)
        {
            //var c1 = new ChicoDoColchaoEntities();
            var p = chicoDoColchaoEntities.Produto.SingleOrDefault(x => x.Numero == numero || x.ProdutoID == produtoId);

            if (p != null)
            {
                p.Ativo = true;
                chicoDoColchaoEntities.SaveChanges();
            }
        }

        public Produto ListarEmLoja(int produtoId = 0, long numero = 0, int lojaOrigemId = 0, int lojaDestinoId = 0)
        {
            IQueryable<Produto> query = chicoDoColchaoEntities.Produto;

            if (produtoId > 0)
                query = query.Where(x => x.ProdutoID == produtoId);

            if (numero > 0)
                query = query.Where(x => x.Numero == numero);

            if (lojaOrigemId > 0)
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaOrigemId));

            if (lojaDestinoId > 0)
                query = query.Where(x => x.LojaProduto.Any(w => w.LojaID == lojaDestinoId));

            var lista = query.Include(x => x.Categoria).Include(x => x.Medida).OrderBy(x => x.Descricao).ToList();

            if (lista.Count() > 1)
                lista.RemoveAll(x => !x.Ativo);

            return lista.FirstOrDefault();
        }
    }
}

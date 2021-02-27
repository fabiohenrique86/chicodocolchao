using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ChicoDoColchao.Repository
{
    public class OrcamentoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public OrcamentoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public int Incluir(Orcamento orcamento)
        {
            chicoDoColchaoEntities.Entry(orcamento).State = EntityState.Added;
            chicoDoColchaoEntities.SaveChanges();

            return orcamento.OrcamentoID;
        }

        public void Atualizar(Orcamento orcamento)
        {
            var o = chicoDoColchaoEntities.Orcamento.SingleOrDefault(x => x.OrcamentoID == orcamento.OrcamentoID);

            o.PedidoID = orcamento.PedidoID;

            chicoDoColchaoEntities.Entry(o).State = EntityState.Modified;

            chicoDoColchaoEntities.SaveChanges();
        }

        public List<Orcamento> Listar(Orcamento orcamento, bool top, int take)
        {
            IQueryable<Orcamento> query = chicoDoColchaoEntities.Orcamento;

            if (orcamento.OrcamentoID > 0)
            {
                query = query.Where(x => x.OrcamentoID == orcamento.OrcamentoID);
            }

            if (!string.IsNullOrEmpty(orcamento.NomeCliente))
            {
                query = query.Where(x => x.NomeCliente.Contains(orcamento.NomeCliente));
            }

            if (orcamento.DataOrcamento != DateTime.MinValue)
            {
                query = query.Where(x => x.DataOrcamento.Day == orcamento.DataOrcamento.Day && x.DataOrcamento.Month == orcamento.DataOrcamento.Month && x.DataOrcamento.Year == orcamento.DataOrcamento.Year);
            }

            if (orcamento.Loja != null && orcamento.Loja.LojaID > 0)
            {
                query = query.Where(x => x.Loja.LojaID == orcamento.Loja.LojaID);
            }
            else if (orcamento.LojaID > 0)
            {
                query = query.Where(x => x.Loja.LojaID == orcamento.LojaID);
            }

            if (orcamento.FuncionarioID > 0)
            {
                query = query.Where(x => x.FuncionarioID == orcamento.FuncionarioID);
            }

            if (top)
            {
                return query
                    //.Include(x => x.Cliente.Estado)
                    .Include(x => x.Loja)
                    .Include(x => x.Funcionario)
                    .Include(x => x.Pedido)
                    .Include(x => x.OrcamentoProduto.Select(w => w.Produto.Categoria))
                    .Include(x => x.OrcamentoProduto.Select(w => w.Produto.Medida))
                    .OrderByDescending(x => x.OrcamentoID)
                    .Take(take)
                    .ToList();
            }
            else
            {
                return query
                    .Include(x => x.OrcamentoHistorico)
                    //.Include(x => x.Cliente.Estado)
                    .Include(x => x.Loja)
                    .Include(x => x.Funcionario)
                    .Include(x => x.Pedido)
                    .Include(x => x.OrcamentoProduto.Select(w => w.Produto.Categoria))
                    .Include(x => x.OrcamentoProduto.Select(w => w.Produto.Medida))
                    .OrderByDescending(x => x.OrcamentoID)
                    .ToList();
            }
        }
    }
}
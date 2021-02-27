using System.Data.Entity;

namespace ChicoDoColchao.Repository
{
    public class NotaFiscalProdutoRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public NotaFiscalProdutoRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }

        public void Incluir(NotaFiscalProduto notaFiscalProduto)
        {
            chicoDoColchaoEntities.Entry(notaFiscalProduto).State = EntityState.Added;

            chicoDoColchaoEntities.SaveChanges();
        }
    }
}

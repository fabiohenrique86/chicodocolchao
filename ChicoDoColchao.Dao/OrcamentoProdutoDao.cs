namespace ChicoDoColchao.Dao
{
    public class OrcamentoProdutoDao
    {
        public int OrcamentoProdutoID { get; set; }
        public int OrcamentoID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public double Preco { get; set; }
        public string Medida { get; set; }

        public virtual OrcamentoDao OrcamentoDao { get; set; }
        public virtual ProdutoDao ProdutoDao { get; set; }
    }
}

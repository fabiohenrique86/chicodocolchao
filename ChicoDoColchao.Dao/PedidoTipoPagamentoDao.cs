namespace ChicoDoColchao.Dao
{
    public class PedidoTipoPagamentoDao
    {
        public int PedidoTipoPagamentoID { get; set; }
        public int PedidoID { get; set; }
        public string CV { get; set; }
        public double ValorPago { get; set; }

        public virtual ParcelaDao ParcelaDao { get; set; }
        public virtual TipoPagamentoDao TipoPagamentoDao { get; set; }
    }
}

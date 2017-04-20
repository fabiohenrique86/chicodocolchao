using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class TipoPagamentoDao
    {
        public TipoPagamentoDao()
        {
            PedidoTipoPagamentoDao = new HashSet<PedidoTipoPagamentoDao>();
        }

        public int TipoPagamentoID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        
        public virtual ICollection<PedidoTipoPagamentoDao> PedidoTipoPagamentoDao { get; set; }

        public enum ETipoPagamento
        {
            Dinheiro = 1,
            CartaoMaster = 2,
            CartaoVisa = 3,
            Cheque = 4
        }
    }
}

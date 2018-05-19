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
            CartaoBancoDoBrasilMasterCard = 2,
            CartaoBancoDoBrasilVisa = 3,
            Cheque = 4,
            CartaoBancoDoBrasilElo = 5,
            CartaoBancoDoBrasilHiperCard = 6,
            CartaoBancoDoBrasilAmericanExpress = 7,
            Crediario = 8,
            CartaoCaixaEconomicaVisa = 9,
            CartaoItauVisa = 10,
            CartaoBradescoVisa = 11,
            CartaoSantanderVisa = 12,
            CartaoCaixaEconomicaMasterCard = 13,
            CartaoItauMasterCard = 14,
            CartaoBradescoMasterCard = 15,
            CartaoSantanderMasterCard = 16,
            CartaoCaixaEconomicaElo = 17,
            CartaoItauElo = 18,
            CartaoBradescoElo = 19,
            CartaoSantanderElo = 20,
            CartaoCaixaEconomicaHiperCard = 21,
            CartaoItauHiperCard = 22,
            CartaoBradescoHiperCard = 23,
            CartaoSantanderHiperCard = 24,
            CartaoCaixaEconomicaAmericanExpress = 25,
            CartaoItauAmericanExpress = 26,
            CartaoBradescoAmericanExpress = 27,
            CartaoSantanderAmericanExpress = 28,
            CartaoOutros = 29
        }
    }
}

using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class MenuDao
    {
        public List<PedidoStatusMes> PedidoStatusMes;
        public List<FaturamentoLojaMes> FaturamentoLojaMes;
        public List<FaturamentoTipoPagamentoMes> FaturamentoTipoPagamentoMes;
        public List<LojaEstoqueNegativo> LojaEstoqueNegativo;
        public NotaFiscalImportadaMes NotaFiscalImportadaMes;
        public List<FaturamentoConsultorMes> FaturamentoConsultorMes;

        public MenuDao()
        {
            PedidoStatusMes = new List<PedidoStatusMes>();
            FaturamentoLojaMes = new List<FaturamentoLojaMes>();
            FaturamentoTipoPagamentoMes = new List<FaturamentoTipoPagamentoMes>();
            LojaEstoqueNegativo = new List<LojaEstoqueNegativo>();
            NotaFiscalImportadaMes = new NotaFiscalImportadaMes();
            FaturamentoConsultorMes = new List<FaturamentoConsultorMes>();
        }
    }

    public class PedidoStatusMes
    {
        public int qtdPedido { get; set; }
        public int pedidoStatusID { get; set; }
        public string descricaoStatus { get; set; }
    }

    public class FaturamentoLojaMes
    {
        public int lojaId { get; set; }
        public string nomeFantasia { get; set; }
        public double venda { get; set; }
    }

    public class FaturamentoTipoPagamentoMes
    {
        public int tipoPagamentoId { get; set; }
        public string descricao { get; set; }
        public double venda { get; set; }
    }

    public class LojaEstoqueNegativo
    {
        public int lojaId { get; set; }
        public string nomeFantasia { get; set; }
        public int qtdTotal { get; set; }
    }

    public class NotaFiscalImportadaMes
    {
        public int quantidade { get; set; }
    }

    public class FaturamentoConsultorMes
    {
        public int funcionarioID { get; set; }
        public string nome { get; set; }
        public int qtdPedido { get; set; }
        public double venda { get; set; }
    }
}

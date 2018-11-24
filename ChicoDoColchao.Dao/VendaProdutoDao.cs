using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class VendaProdutoDao
    {
        public double Venda { get; set; }
        public int Quantidade { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public List<int> ProdutoDao { get; set; }

        public int ProdutoID { get; set; }
        public long Numero { get; set; }
        public string Produto { get; set; }
        public int MedidaID { get; set; }
        public string Medida { get; set; }
    }
}

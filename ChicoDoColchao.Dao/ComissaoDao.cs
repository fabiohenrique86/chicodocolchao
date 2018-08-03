using System;

namespace ChicoDoColchao.Dao
{
    public class ComissaoDao
    {
        public int FuncionarioID { get; set; }
        public string Nome { get; set; }
        public double Venda { get; set; }
        public int QtdPedido { get; set; }
        public double Comissao { get; set; }

        public int LojaID { get; set; }
        public string NomeFantasia { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }        
    }
}

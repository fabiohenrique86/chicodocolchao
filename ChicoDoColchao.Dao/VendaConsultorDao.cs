using System;

namespace ChicoDoColchao.Dao
{
    public class VendaConsultorDao
    {
        public int FuncionarioID { get; set; }
        public string Nome { get; set; }
        public double VendaDia { get; set; }
        public double VendaAcumulada { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime DataFim { get; set; }        
    }
}

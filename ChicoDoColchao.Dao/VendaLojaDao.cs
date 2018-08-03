using System;

namespace ChicoDoColchao.Dao
{
    public class VendaLojaDao
    {
        public int LojaID { get; set; }
        public string NomeFantasia { get; set; }
        public double VendaDia { get; set; }
        public double VendaAcumulada { get; set; }

        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }        
    }
}

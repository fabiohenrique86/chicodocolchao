using System;

namespace ChicoDoColchao.Dao
{
    public class OrcamentoHistoricoDao
    {
        public int OrcamentoHistoricoID { get; set; }
        public int OrcamentoID { get; set; }
        public string Observacao { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}

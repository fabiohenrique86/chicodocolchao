using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class ContatoDao
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int AssuntoId { get; set; }
        public string Assunto { get; set; }
        public string Mensagem { get; set; }
        
        public IEnumerable<AssuntoContatoDao> AssuntoContatoDao = new List<AssuntoContatoDao>
        {
            new AssuntoContatoDao { IdAssunto = 1, DsAssunto = "Dúvida" },
            new AssuntoContatoDao { IdAssunto = 2, DsAssunto = "Sugestão" },
            new AssuntoContatoDao { IdAssunto = 3, DsAssunto = "Elogio" },
            new AssuntoContatoDao { IdAssunto = 4, DsAssunto = "Reclamação" }
        };
    }
}

using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class ColchaoIdealDao
    {
        public string Nome { get; set; }        
        public string Email { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }
        public string TipoId { get; set; }
        public string TamanhoId { get; set; }

        public IEnumerable<TipoColchaoDao> TipoColchaoDao = new List<TipoColchaoDao>
        {
            new TipoColchaoDao { IdTipo = 1, DsTipo = "Firme" },
            new TipoColchaoDao { IdTipo = 2, DsTipo = "Confortável" },
            new TipoColchaoDao { IdTipo = 3, DsTipo = "De Mola" },
            new TipoColchaoDao { IdTipo = 4, DsTipo = "Só Espuma" },
            new TipoColchaoDao { IdTipo = 5, DsTipo = "Ortopédico Madeira" }
        };
                
        public IEnumerable<TamanhoColchaoDao> TamanhoColchaoDao = new List<TamanhoColchaoDao>
        {
            new TamanhoColchaoDao { IdTamanho = 1, DsTamanho = "Infantil (070 X 130)" },
            new TamanhoColchaoDao { IdTamanho = 2, DsTamanho = "Solteiro (088 X 188)" },
            new TamanhoColchaoDao { IdTamanho = 3, DsTamanho = "Viúva (128 X 188)" },
            new TamanhoColchaoDao { IdTamanho = 4, DsTamanho = "Casal Padrão (138 X 188)" },
            new TamanhoColchaoDao { IdTamanho = 5, DsTamanho = "Casal Queen (158 X 198)" },
            new TamanhoColchaoDao { IdTamanho = 6, DsTamanho = "Casal King (186 X 198)" },
            new TamanhoColchaoDao { IdTamanho = 7, DsTamanho = "Casal SuperKing (193 X 203)" }
        };
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Models
{
    public class ColchaoIdealModel
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tipo obrigatória")]
        public string TipoId { get; set; }
        public IEnumerable<TipoModel> Tipos = new List<TipoModel>
        {
            new TipoModel { IdTipo = 1, DsTipo = "Firme" },
            new TipoModel { IdTipo = 2, DsTipo = "Confortável" },
            new TipoModel { IdTipo = 3, DsTipo = "De Mola" },
            new TipoModel { IdTipo = 4, DsTipo = "Só Espuma" },
            new TipoModel { IdTipo = 5, DsTipo = "Ortopédico Madeira" }
        };

        [Required(ErrorMessage = "Tamanho obrigatório")]
        public string TamanhoId { get; set; }
        public IEnumerable<TamanhoModel> Tamanhos = new List<TamanhoModel>
        {
            new TamanhoModel { IdTamanho = 1, DsTamanho = "Infantil (070 X 130)" },
            new TamanhoModel { IdTamanho = 2, DsTamanho = "Solteiro (088 X 188)" },
            new TamanhoModel { IdTamanho = 3, DsTamanho = "Viúva (128 X 188)" },
            new TamanhoModel { IdTamanho = 4, DsTamanho = "Casal Padrão (138 X 188)" },
            new TamanhoModel { IdTamanho = 5, DsTamanho = "Casal Queen (158 X 198)" },
            new TamanhoModel { IdTamanho = 6, DsTamanho = "Casal King (186 X 198)" },
            new TamanhoModel { IdTamanho = 7, DsTamanho = "Casal SuperKing (193 X 203)" }
        };

        [Required(ErrorMessage = "Altura obrigatória")]
        public string Altura { get; set; }

        [Required(ErrorMessage = "Peso obrigatório")]
        public string Peso { get; set; }
    }
}
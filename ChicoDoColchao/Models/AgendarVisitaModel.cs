using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Models
{
    public class AgendarVisitaModel
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Loja obrigatória")]
        public string LojaId { get; set; }
        public IEnumerable<LojaModel> Lojas = new List<LojaModel>
        {
            new LojaModel { IdLoja = 1, DsLoja = "Loja 1" },
            new LojaModel { IdLoja = 2, DsLoja = "Loja 2" },
            new LojaModel { IdLoja = 3, DsLoja = "Loja 3" },
            new LojaModel { IdLoja = 4, DsLoja = "Loja 4" }
        };
        
        [Required(ErrorMessage = "Data/Hora obrigatória")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHora { get; set; }
    }
}
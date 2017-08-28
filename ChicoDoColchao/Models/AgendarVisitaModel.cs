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
            new LojaModel { LojaID = 1, NomeFantasia = "Loja 1" },
            new LojaModel { LojaID = 2, NomeFantasia = "Loja 2" },
            new LojaModel { LojaID = 3, NomeFantasia = "Loja 3" },
            new LojaModel { LojaID = 4, NomeFantasia = "Loja 4" }
        };
        
        [Required(ErrorMessage = "Data/Hora obrigatória")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHora { get; set; }
    }
}
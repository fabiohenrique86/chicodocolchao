using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Models
{
    public class ContatoModel
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Assunto obrigatório")]
        public string AssuntoId { get; set; }
        public IEnumerable<AssuntoModel> Assuntos = new List<AssuntoModel>
        {
            new AssuntoModel { IdAssunto = 1, DsAssunto = "Dúvida" },
            new AssuntoModel { IdAssunto = 2, DsAssunto = "Sugestão" },
            new AssuntoModel { IdAssunto = 3, DsAssunto = "Elogio" },
            new AssuntoModel { IdAssunto = 4, DsAssunto = "Reclamação" }
        };

        [Required(ErrorMessage = "Mensagem obrigatório")]
        public string Mensagem { get; set; }
    }
}
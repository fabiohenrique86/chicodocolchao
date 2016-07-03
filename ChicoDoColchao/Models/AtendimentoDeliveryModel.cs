using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChicoDoColchao.Models
{
    public class AtendimentoDeliveryModel
    {
        [Required(ErrorMessage = "Nome obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone Fixo obrigatório")]
        public string TelefoneFixo { get; set; }

        [Required(ErrorMessage = "Telefone Celular obrigatório")]
        public string TelefoneCelular { get; set; }

        [Required(ErrorMessage = "Endereço obrigatório")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório")]
        public string Bairro { get; set; }
    }
}
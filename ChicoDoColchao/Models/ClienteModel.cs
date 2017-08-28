using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class ClienteModel
    {
        public int ClienteID { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public Nullable<System.DateTime> DataNascimento { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneResidencial2 { get; set; }
        public string TelefoneCelular2 { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string PontoReferencia { get; set; }
        public bool Ativo { get; set; }
    }
}
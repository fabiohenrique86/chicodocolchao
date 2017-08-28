using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class ClienteDao
    {
        public ClienteDao()
        {
            EstadoDao = new HashSet<EstadoDao>();
        }

        public int ClienteID { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string TelefoneResidencial2 { get; set; }
        public string TelefoneCelular2 { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public ICollection<EstadoDao> EstadoDao { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string PontoReferencia { get; set; }
        public bool Ativo { get; set; }
        public short? Numero { get; set; }
        public string Complemento { get; set; }
        public string Email { get; set; }
    }
}

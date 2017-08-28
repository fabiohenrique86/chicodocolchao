using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class FuncionarioModel
    {
        public int FuncionarioID { get; set; }
        public int Numero { get; set; }
        public int LojaID { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
    }
}
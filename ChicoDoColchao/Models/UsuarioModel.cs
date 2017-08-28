using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class UsuarioModel
    {
        public int UsuarioID { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool Ativo { get; set; }
    }
}
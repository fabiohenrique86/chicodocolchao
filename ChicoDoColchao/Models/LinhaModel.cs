using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class LinhaModel
    {
        public int LinhaID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
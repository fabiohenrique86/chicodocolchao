using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class PedidoStatusModel
    {
        public int PedidoStatusID { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
}
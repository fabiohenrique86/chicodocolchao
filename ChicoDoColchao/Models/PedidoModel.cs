using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ChicoDoColchao.Models
{
    public class PedidoModel
    {
        public PedidoModel()
        {
            this.LojaModel = new HashSet<LojaModel>();
            this.ClienteModel = new HashSet<ClienteModel>();
            this.FuncionarioModel = new HashSet<FuncionarioModel>();
            this.PedidoProdutoModel = new HashSet<PedidoProdutoModel>();
            this.PedidoTipoPagamentoModel = new HashSet<PedidoTipoPagamentoModel>();
            this.PedidoStatusModel = new HashSet<PedidoStatusModel>();
        }

        public int PedidoID { get; set; }        
        public Nullable<int> Numero { get; set; }        
        public int FuncionarioID { get; set; }        
        public int ClienteID { get; set; }
        public System.DateTime DataPedido { get; set; }
        public Nullable<System.DateTime> DataEntrega { get; set; }
        public int LojaOrigemID { get; set; }
        public int LojaSaidaID { get; set; }
        public int PedidoStatusID { get; set; }
        public string Observacao { get; set; }
        public string NomeCarreto { get; set; }
        public Nullable<double> ValorFrete { get; set; }

        public ICollection<LojaModel> LojaModel { get; set; }
        public ICollection<ClienteModel> ClienteModel { get; set; }
        public ICollection<FuncionarioModel> FuncionarioModel { get; set; }
        public ICollection<PedidoProdutoModel> PedidoProdutoModel { get; set; }
        public ICollection<PedidoTipoPagamentoModel> PedidoTipoPagamentoModel { get; set; }
        public ICollection<PedidoStatusModel> PedidoStatusModel { get; set; }
    }
}
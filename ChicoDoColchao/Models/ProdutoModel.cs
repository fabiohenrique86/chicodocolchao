using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChicoDoColchao.Models
{
    public class ProdutoModel
    {
        public ProdutoModel()
        {
            this.LinhaModel = new LinhaModel();
            this.MedidaModel = new MedidaModel();
        }

        public int ProdutoID { get; set; }
        public long Numero { get; set; }
        public int LinhaID { get; set; }
        public string Descricao { get; set; }
        public int MedidaID { get; set; }
        public Nullable<double> Preco { get; set; }
        public short ComissaoFuncionario { get; set; }
        public short ComissaoFranqueado { get; set; }
        public bool Ativo { get; set; }

        public virtual LinhaModel LinhaModel { get; set; }
        public virtual MedidaModel MedidaModel { get; set; }
    }
}
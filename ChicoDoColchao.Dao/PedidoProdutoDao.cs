using System;

namespace ChicoDoColchao.Dao
{
   public class PedidoProdutoDao
    {
        public PedidoProdutoDao()
        {
            ProdutoDao = new ProdutoDao();
            UsuarioEntregaDao = new UsuarioDao();
            UsuarioBaixaDao = new UsuarioDao();
        }

        public int PedidoProdutoID { get; set; }
        public int PedidoID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }
        public Nullable<System.DateTime> DataEntrega { get; set; }
        public Nullable<System.DateTime> DataBaixa { get; set; }

        public virtual ProdutoDao ProdutoDao { get; set; }
        public virtual UsuarioDao UsuarioEntregaDao { get; set; }
        public virtual UsuarioDao UsuarioBaixaDao { get; set; }
    }
}

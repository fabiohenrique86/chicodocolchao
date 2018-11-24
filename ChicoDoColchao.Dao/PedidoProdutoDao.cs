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
            UsuarioTrocaDao = new UsuarioDao();
        }

        public int PedidoProdutoID { get; set; }
        public int PedidoID { get; set; }
        public int ProdutoID { get; set; }
        public short Quantidade { get; set; }
        public string Medida { get; set; }
        public double Preco { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataBaixa { get; set; }
        public DateTime? DataTroca { get; set; }
        public int Tipo { get; set; }

        public enum ETipo
        {
            Entrada = 1,
            Saida = 2,
        }

        public virtual ProdutoDao ProdutoDao { get; set; }
        public virtual UsuarioDao UsuarioEntregaDao { get; set; }
        public virtual UsuarioDao UsuarioBaixaDao { get; set; }
        public virtual UsuarioDao UsuarioTrocaDao { get; set; }
    }
}

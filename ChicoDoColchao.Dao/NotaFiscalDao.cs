using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class NotaFiscalDao
    {
        public NotaFiscalDao()
        {
            NotaFiscalProdutoDao = new HashSet<NotaFiscalProdutoDao>();
            Arquivo = new HashSet<System.IO.Stream>();
        }

        public int NotaFiscalID { get; set; }
        public int Numero { get; set; }
        public System.DateTime DataNotaFiscal { get; set; }
        public Nullable<int> PedidoMaeID { get; set; }
        public bool Ativo { get; set; }
        public HashSet<System.IO.Stream> Arquivo { get; set; }

        public LojaDao LojaDao { get; set; }
        public ICollection<NotaFiscalProdutoDao> NotaFiscalProdutoDao { get; set; }
    }
}

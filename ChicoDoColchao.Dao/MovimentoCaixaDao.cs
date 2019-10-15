using System;

namespace ChicoDoColchao.Dao
{
    public class MovimentoCaixaDao
    {
        public MovimentoCaixaDao()
        {
            LojaDao = new LojaDao();
            MovimentoCaixaStatusDao = new MovimentoCaixaStatusDao();
            UsuarioRecebimento = new UsuarioDao();
        }

        public int MovimentoCaixaID { get; set; }
        public DateTime DataMovimento { get; set; }
        public LojaDao LojaDao { get; set; }
        public double? Valor { get; set; }
        public MovimentoCaixaStatusDao MovimentoCaixaStatusDao { get; set; }
        public DateTime? DataRecebimento { get; set; }
        public UsuarioDao UsuarioRecebimento { get; set; }
        public int? NumeroSequencial { get; set; }
    }
}

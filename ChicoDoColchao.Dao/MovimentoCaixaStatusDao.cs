namespace ChicoDoColchao.Dao
{
    public class MovimentoCaixaStatusDao
    {
        public int MovimentoCaixaStatusID { get; set; }
        public string Descricao { get; set; }

        public enum EStatus
        {
            Gerado = 1,
            Recebido = 2,
        }
    }
}

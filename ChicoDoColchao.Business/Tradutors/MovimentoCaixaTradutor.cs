using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class MovimentoCaixaTradutor
    {
        public static MovimentoCaixa ToBd(this MovimentoCaixaDao movimentoCaixaDao)
        {
            var movimentoCaixa = new MovimentoCaixa();

            movimentoCaixa.MovimentoCaixaID = movimentoCaixaDao.MovimentoCaixaID;
            movimentoCaixa.DataMovimento = movimentoCaixaDao.DataMovimento;
            movimentoCaixa.LojaID = movimentoCaixaDao.LojaDao.LojaID;
            movimentoCaixa.Valor = movimentoCaixaDao.Valor;

            if (movimentoCaixaDao.MovimentoCaixaStatusDao != null && movimentoCaixaDao.MovimentoCaixaStatusDao.MovimentoCaixaStatusID > 0)
                movimentoCaixa.MovimentoCaixaStatusID = movimentoCaixaDao.MovimentoCaixaStatusDao.MovimentoCaixaStatusID;

            if (movimentoCaixaDao.DataRecebimento.GetValueOrDefault() != DateTime.MinValue)
                movimentoCaixa.DataRecebimento = movimentoCaixaDao.DataRecebimento;

            if (movimentoCaixaDao.UsuarioRecebimento != null && movimentoCaixaDao.UsuarioRecebimento.UsuarioID > 0)
                movimentoCaixa.UsuarioRecebimentoID = movimentoCaixaDao.UsuarioRecebimento.UsuarioID;

            if (movimentoCaixaDao.NumeroSequencial.GetValueOrDefault() > 0)
                movimentoCaixa.NumeroSequencial = movimentoCaixaDao.NumeroSequencial;

            return movimentoCaixa;
        }

        public static MovimentoCaixaDao ToApp(this MovimentoCaixa movimentoCaixa)
        {
            var movimentoCaixaDao = new MovimentoCaixaDao();

            movimentoCaixaDao.MovimentoCaixaID = movimentoCaixa.MovimentoCaixaID;
            movimentoCaixaDao.DataMovimento = movimentoCaixa.DataMovimento;

            if (movimentoCaixa.Loja != null && movimentoCaixa.Loja.LojaID > 0)
                movimentoCaixaDao.LojaDao = new LojaDao() { LojaID = movimentoCaixa.Loja.LojaID, NomeFantasia = movimentoCaixa.Loja.NomeFantasia };

            movimentoCaixaDao.Valor = movimentoCaixa.Valor;

            if (movimentoCaixa.MovimentoCaixaStatus != null && movimentoCaixa.MovimentoCaixaStatus.MovimentoCaixaStatusID > 0)
                movimentoCaixaDao.MovimentoCaixaStatusDao = new MovimentoCaixaStatusDao() { MovimentoCaixaStatusID = movimentoCaixa.MovimentoCaixaStatus.MovimentoCaixaStatusID, Descricao = movimentoCaixa.MovimentoCaixaStatus.Descricao };

            movimentoCaixaDao.DataRecebimento = movimentoCaixa.DataRecebimento;

            if (movimentoCaixa.Usuario != null && movimentoCaixa.Usuario.UsuarioID > 0)
                movimentoCaixaDao.UsuarioRecebimento = new UsuarioDao() { UsuarioID = movimentoCaixa.Usuario.UsuarioID };

            movimentoCaixaDao.NumeroSequencial = movimentoCaixa.NumeroSequencial;

            return movimentoCaixaDao;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using System.Transactions;

namespace ChicoDoColchao.Business
{
    public class MovimentoCaixaBusiness
    {
        MovimentoCaixaRepository movimentoCaixaRepository;
        LogRepository logRepository;

        public MovimentoCaixaBusiness()
        {
            movimentoCaixaRepository = new MovimentoCaixaRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(MovimentoCaixaDao movimentoCaixaDao)
        {
            if (movimentoCaixaDao == null)
            {
                throw new BusinessException("MovimentoCaixa é obrigatório");
            }

            if (movimentoCaixaDao.DataMovimento == DateTime.MinValue)
            {
                throw new BusinessException("Data do Movimento é obrigatório");
            }

            if (movimentoCaixaDao.LojaDao == null || movimentoCaixaDao.LojaDao.LojaID <= 0)
            {
                throw new BusinessException("Loja é obrigatório");
            }

            if (movimentoCaixaDao.MovimentoCaixaStatusDao == null || movimentoCaixaDao.MovimentoCaixaStatusDao.MovimentoCaixaStatusID <= 0)
            {
                throw new BusinessException("Status é obrigatório");
            }
        }

        private void ValidarExcluir(MovimentoCaixaDao movimentoCaixaDao)
        {
            if (movimentoCaixaDao == null)
            {
                throw new BusinessException("MovimentoCaixa é obrigatório");
            }

            if (movimentoCaixaDao.MovimentoCaixaID <= 0)
            {
                throw new BusinessException("MovimentoCaixaID é obrigatório");
            }
        }

        private void ValidarReceber(MovimentoCaixaDao movimentoCaixaDao)
        {
            if (movimentoCaixaDao == null)
            {
                throw new BusinessException("MovimentoCaixa é obrigatório");
            }

            if (movimentoCaixaDao.MovimentoCaixaID <= 0)
            {
                throw new BusinessException("MovimentoCaixaID é obrigatório");
            }

            if (movimentoCaixaDao.MovimentoCaixaStatusDao == null || movimentoCaixaDao.MovimentoCaixaStatusDao.MovimentoCaixaStatusID <= 0)
            {
                throw new BusinessException("Status é obrigatório");
            }
        }

        public List<MovimentoCaixaDao> Listar(MovimentoCaixaDao movimentoCaixaDao)
        {
            try
            {
                return movimentoCaixaRepository.Listar(movimentoCaixaDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public void Excluir(MovimentoCaixaDao movimentoCaixaDao)
        {
            try
            {
                ValidarExcluir(movimentoCaixaDao);

                movimentoCaixaRepository.Excluir(movimentoCaixaDao.ToBd());
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public int Incluir(MovimentoCaixaDao movimentoCaixaDao)
        {
            try
            {
                int movimentoCaixaId = 0;

                ValidarIncluir(movimentoCaixaDao);

                var movimentoCaixa = Listar(movimentoCaixaDao).FirstOrDefault();

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(10) }))
                {
                    if (movimentoCaixa != null)
                    {
                        movimentoCaixaDao.MovimentoCaixaID = movimentoCaixa.MovimentoCaixaID;

                        Excluir(movimentoCaixaDao);

                        movimentoCaixaDao.MovimentoCaixaID = 0;
                    }

                    movimentoCaixaId = movimentoCaixaRepository.Incluir(movimentoCaixaDao.ToBd());

                    scope.Complete();

                    return movimentoCaixaId;
                }
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public void Receber(MovimentoCaixaDao movimentoCaixaDao)
        {
            try
            {
                ValidarReceber(movimentoCaixaDao);

                movimentoCaixaRepository.Receber(movimentoCaixaDao.ToBd());
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}

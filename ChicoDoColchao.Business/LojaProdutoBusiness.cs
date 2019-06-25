using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class LojaProdutoBusiness
    {
        LojaProdutoRepository lojaProdutoRepository;
        LogRepository logRepository;

        public LojaProdutoBusiness()
        {
            lojaProdutoRepository = new LojaProdutoRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(LojaProdutoDao lojaProdutoDao)
        {
            if (lojaProdutoDao == null)
            {
                throw new BusinessException("lojaProdutoDao é obrigatório");
            }

            if (lojaProdutoDao.LojaID <= 0)
            {
                throw new BusinessException("LojaID é obrigatório");
            }

            if (lojaProdutoDao.ProdutoID <= 0)
            {
                throw new BusinessException("ProdutoID é obrigatório");
            }
        }

        private void ValidarAtualizar(LojaProdutoDao lojaProdutoDao)
        {
            if (lojaProdutoDao == null)
            {
                throw new BusinessException("lojaProdutoDao é obrigatório");
            }

            if (lojaProdutoDao.LojaProdutoID <= 0)
            {
                throw new BusinessException("LojaProdutoID é obrigatório");
            }

            //if (lojaProdutoDao.LojaID <= 0)
            //{
            //    throw new BusinessException("LojaID é obrigatório");
            //}

            //if (lojaProdutoDao.ProdutoID <= 0)
            //{
            //    throw new BusinessException("ProdutoID é obrigatório");
            //}
        }

        public void Incluir(LojaProdutoDao lojaProdutoDao)
        {
            try
            {
                ValidarIncluir(lojaProdutoDao);

                lojaProdutoRepository.Incluir(lojaProdutoDao.ToBd());
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

        public void Atualizar(LojaProdutoDao lojaProdutoDao)
        {
            try
            {
                ValidarAtualizar(lojaProdutoDao);

                lojaProdutoRepository.Atualizar(lojaProdutoDao.ToBd());
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

        public List<LojaProdutoDao> Listar(LojaProdutoDao lojaProdutoDao)
        {
            try
            {
                return lojaProdutoRepository.Listar(lojaProdutoDao.ToBd()).Select(x => x.ToApp()).ToList();
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

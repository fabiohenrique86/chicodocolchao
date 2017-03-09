using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class LojaBusiness
    {
        LojaRepository lojaRepository;
        LogRepository logRepository;

        public LojaBusiness()
        {
            lojaRepository = new LojaRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(LojaDao lojaDao)
        {
            if (lojaDao == null)
            {
                throw new BusinessException("Loja é obrigatório");
            }

            if (string.IsNullOrEmpty(lojaDao.Cnpj))
            {
                throw new BusinessException("CNPJ é obrigatório");
            }

            if (string.IsNullOrEmpty(lojaDao.NomeFantasia))
            {
                throw new BusinessException("Nome Fantasia é obrigatório");
            }

            //if (string.IsNullOrEmpty(lojaDao.RazaoSocial))
            //{
            //    throw new BusinessException("Razão Social é obrigatório");
            //}

            //if (string.IsNullOrEmpty(lojaDao.Telefone))
            //{
            //    throw new BusinessException("Telefone é obrigatório");
            //}

            if (lojaRepository.Listar(new Loja() { Cnpj = string.IsNullOrEmpty(lojaDao.Cnpj) ? string.Empty : lojaDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "") }).FirstOrDefault() != null)
            {
                throw new BusinessException(string.Format("Loja (CNPJ {0}) já cadastrada", lojaDao.Cnpj));
            }
        }

        public void Incluir(LojaDao lojaDao)
        {
            try
            {
                ValidarIncluir(lojaDao);

                lojaRepository.Incluir(lojaDao.ToBd());
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public List<LojaDao> Listar(LojaDao lojaDao)
        {
            try
            {
                return lojaRepository.Listar(lojaDao.ToBd()).Select(x => x.ToApp()).ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }
    }
}

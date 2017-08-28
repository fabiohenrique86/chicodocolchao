using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class LinhaBusiness
    {
        LinhaRepository linhaRepository;
        LogRepository logRepository;

        public LinhaBusiness()
        {
            linhaRepository = new LinhaRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(LinhaDao linhaDao)
        {
            if (linhaDao == null)
            {
                throw new BusinessException("Linha é obrigatório");
            }

            if (string.IsNullOrEmpty(linhaDao.Descricao))
            {
                throw new BusinessException("Descrição é obrigatório");
            }
                        
            if (linhaRepository.Listar(new Linha() { Descricao = linhaDao.Descricao }).FirstOrDefault() != null)
            {
                throw new BusinessException("Linha (Descrição) já cadastrada");
            }
        }

        public int Incluir(LinhaDao linhaDao)
        {
            try
            {
                ValidarIncluir(linhaDao);

                return linhaRepository.Incluir(linhaDao.ToBd());
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

        public List<LinhaDao> Listar(LinhaDao linhaDao)
        {
            try
            {
                return linhaRepository.Listar(linhaDao.ToBd()).Select(x => x.ToApp()).ToList();
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

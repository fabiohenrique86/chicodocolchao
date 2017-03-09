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
    public class MedidaBusiness
    {
        MedidaRepository medidaRepository;
        LogRepository logRepository;

        public MedidaBusiness()
        {
            medidaRepository = new MedidaRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(MedidaDao medidaDao)
        {
            if (medidaDao == null)
            {
                throw new BusinessException("Medida é obrigatório");
            }

            if (string.IsNullOrEmpty(medidaDao.Descricao))
            {
                throw new BusinessException("Descrição é obrigatório");
            }
                        
            if (medidaRepository.Listar(new Medida() { Descricao = medidaDao.Descricao.Replace(" ", "").Trim() }).FirstOrDefault() != null)
            {
                throw new BusinessException("Medida (Descrição) já cadastrada");
            }
        }

        public int Incluir(MedidaDao medidaDao)
        {
            try
            {
                ValidarIncluir(medidaDao);

                return medidaRepository.Incluir(medidaDao.ToBd());
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

        public List<MedidaDao> Listar(MedidaDao medidaDao)
        {
            try
            {
                return medidaRepository.Listar(medidaDao.ToBd()).Select(x => x.ToApp()).ToList();
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

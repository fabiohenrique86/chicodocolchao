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
    public class CategoriaBusiness
    {
        CategoriaRepository categoriaRepository;
        LogRepository logRepository;

        public CategoriaBusiness()
        {
            categoriaRepository = new CategoriaRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(CategoriaDao categoriaDao)
        {
            if (categoriaDao == null)
            {
                throw new BusinessException("Categoria é obrigatório");
            }

            if (string.IsNullOrEmpty(categoriaDao.Descricao))
            {
                throw new BusinessException("Descrição é obrigatório");
            }
                        
            if (categoriaRepository.Listar(new Categoria() { Descricao = categoriaDao.Descricao }).FirstOrDefault() != null)
            {
                throw new BusinessException("Categoria (Descrição) já cadastrada");
            }
        }

        public int Incluir(CategoriaDao categoriaDao)
        {
            try
            {
                ValidarIncluir(categoriaDao);

                return categoriaRepository.Incluir(categoriaDao.ToBd());
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

        public List<CategoriaDao> Listar(CategoriaDao categoriaDao)
        {
            try
            {
                return categoriaRepository.Listar(categoriaDao.ToBd()).Select(x => x.ToApp()).ToList();
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

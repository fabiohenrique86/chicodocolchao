﻿using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class FuncionarioBusiness
    {
        FuncionarioRepository funcionarioRepository;
        LogRepository logRepository;

        public FuncionarioBusiness()
        {
            funcionarioRepository = new FuncionarioRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(FuncionarioDao funcionarioDao)
        {
            if (funcionarioDao == null)
            {
                throw new BusinessException("Funcionário é obrigatório");
            }

            if (funcionarioDao.Numero.GetValueOrDefault() <= 0)
            {
                throw new BusinessException("Número é obrigatório");
            }

            if (funcionarioDao.LojaDao == null || funcionarioDao.LojaDao.Count <= 0 || funcionarioDao.LojaDao.FirstOrDefault().LojaID <= 0)
            {
                throw new BusinessException("Loja é obrigatória");
            }

            if (string.IsNullOrEmpty(funcionarioDao.Nome))
            {
                throw new BusinessException("Nome é obrigatório");
            }

            if (string.IsNullOrEmpty(funcionarioDao.Email))
            {
                throw new BusinessException("E-mail é obrigatório");
            }

            if (string.IsNullOrEmpty(funcionarioDao.Telefone))
            {
                throw new BusinessException("Telefone é obrigatório");
            }

            if (funcionarioRepository.Listar(new Funcionario() { Numero = funcionarioDao.Numero.GetValueOrDefault() }).FirstOrDefault() != null)
            {
                throw new BusinessException(string.Format("Funcionário (Número {0}) já cadastrado", funcionarioDao.Numero.GetValueOrDefault()));
            }
        }

        public void Incluir(FuncionarioDao funcionarioDao)
        {
            try
            {
                ValidarIncluir(funcionarioDao);

                funcionarioRepository.Incluir(funcionarioDao.ToBd());
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

        public List<FuncionarioDao> Listar(FuncionarioDao funcionarioDao)
        {
            try
            {
                return funcionarioRepository.Listar(funcionarioDao.ToBd()).Select(x => x.ToApp()).ToList();
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

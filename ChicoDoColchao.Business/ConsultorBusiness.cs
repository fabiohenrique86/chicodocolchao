using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class ConsultorBusiness
    {
        ConsultorRepository consultorRepository;
        LogRepository logRepository;

        public ConsultorBusiness()
        {
            consultorRepository = new ConsultorRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(ConsultorDao consultorDao)
        {
            if (consultorDao == null)
            {
                throw new BusinessException("Consultor é obrigatório");
            }

            if (consultorDao.Numero.GetValueOrDefault() <= 0)
            {
                throw new BusinessException("Número é obrigatório");
            }

            if (consultorDao.LojaDao == null || consultorDao.LojaDao.Count <= 0 || consultorDao.LojaDao.FirstOrDefault().LojaID <= 0)
            {
                throw new BusinessException("Loja é obrigatória");
            }

            if (string.IsNullOrEmpty(consultorDao.Nome))
            {
                throw new BusinessException("Nome é obrigatório");
            }

            if (string.IsNullOrEmpty(consultorDao.Email))
            {
                throw new BusinessException("E-mail é obrigatório");
            }

            if (string.IsNullOrEmpty(consultorDao.Telefone))
            {
                throw new BusinessException("Telefone é obrigatório");
            }

            if (consultorRepository.Listar(new Funcionario() { Numero = consultorDao.Numero.GetValueOrDefault() }).FirstOrDefault() != null)
            {
                throw new BusinessException(string.Format("Consultor (Número {0}) já cadastrado", consultorDao.Numero.GetValueOrDefault()));
            }
        }

        private void ValidarExcluir(ConsultorDao consultorDao, out Funcionario funcionario)
        {
            if (consultorDao == null)
            {
                throw new BusinessException("Consultor é obrigatório");
            }

            if (consultorDao.FuncionarioID <= 0)
            {
                throw new BusinessException("FuncionarioID é obrigatório");
            }

            funcionario = consultorRepository.Listar(new Funcionario() { FuncionarioID = consultorDao.FuncionarioID }).FirstOrDefault();

            if (funcionario == null)
            {
                throw new BusinessException(string.Format("Consultor {0} não encontrado", consultorDao.FuncionarioID));
            }
        }

        private void ValidarAlterar(ConsultorDao consultorDao, out Funcionario funcionario)
        {
            if (consultorDao == null)
            {
                throw new BusinessException("Consultor é obrigatório");
            }

            if (consultorDao.FuncionarioID <= 0)
            {
                throw new BusinessException("FuncionarioID é obrigatório");
            }

            // se nenhum foi informado
            if (string.IsNullOrEmpty(consultorDao.Nome) && 
                string.IsNullOrEmpty(consultorDao.Email) && 
                string.IsNullOrEmpty(consultorDao.Telefone) && 
                (consultorDao.LojaDao == null || consultorDao.LojaDao.Count(x => x.LojaID > 0) <= 0))
            {
                throw new BusinessException("Infome algum campo a ser atualizado");
            }

            funcionario = consultorRepository.Listar(new Funcionario() { FuncionarioID = consultorDao.FuncionarioID }).FirstOrDefault();

            if (funcionario == null)
            {
                throw new BusinessException(string.Format("Consultor {0} não encontrado", consultorDao.FuncionarioID));
            }
        }

        public void Incluir(ConsultorDao consultorDao)
        {
            try
            {
                ValidarIncluir(consultorDao);

                consultorRepository.Incluir(consultorDao.ToBd());
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

        public List<ConsultorDao> Listar(ConsultorDao consultorDao)
        {
            try
            {
                return consultorRepository.Listar(consultorDao.ToBd()).Select(x => x.ToApp()).ToList();
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

        public void Excluir(ConsultorDao consultorDao)
        {
            try
            {
                Funcionario funcionario;

                ValidarExcluir(consultorDao, out funcionario);

                consultorRepository.Excluir(funcionario);
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

        public void Alterar(ConsultorDao consultorDao)
        {
            try
            {
                Funcionario funcionario;

                ValidarAlterar(consultorDao, out funcionario);

                if (!string.IsNullOrEmpty(consultorDao.Nome))
                {
                    funcionario.Nome = consultorDao.Nome;
                }

                if (!string.IsNullOrEmpty(consultorDao.Email))
                {
                    funcionario.Email = consultorDao.Email;
                }

                if (!string.IsNullOrEmpty(consultorDao.Telefone))
                {
                    funcionario.Telefone = consultorDao.Telefone.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (consultorDao.LojaDao != null && consultorDao.LojaDao.Count(x => x.LojaID > 0) > 0)
                {
                    funcionario.LojaID = consultorDao.LojaDao.FirstOrDefault().LojaID;
                }

                consultorRepository.Alterar(funcionario);
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

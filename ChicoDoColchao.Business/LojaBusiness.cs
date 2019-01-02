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

            //if (string.IsNullOrEmpty(lojaDao.Cnpj))
            //{
            //    throw new BusinessException("CNPJ é obrigatório");
            //}

            if (string.IsNullOrEmpty(lojaDao.NomeFantasia))
            {
                throw new BusinessException("Nome Fantasia é obrigatório");
            }

            if (string.IsNullOrEmpty(lojaDao.Bairro))
            {
                throw new BusinessException("Bairro é obrigatório");
            }

            //if (string.IsNullOrEmpty(lojaDao.RazaoSocial))
            //{
            //    throw new BusinessException("Razão Social é obrigatório");
            //}

            //if (string.IsNullOrEmpty(lojaDao.Telefone))
            //{
            //    throw new BusinessException("Telefone é obrigatório");
            //}
            var l = lojaRepository.Listar(new Loja() { Cnpj = string.IsNullOrEmpty(lojaDao.Cnpj) ? string.Empty : lojaDao.Cnpj.Replace(".", "").Replace("-", "").Replace("/", ""), Ativo = true }).FirstOrDefault();

            if (l != null && !string.IsNullOrEmpty(lojaDao.Cnpj))
            {
                throw new BusinessException(string.Format("Loja (CNPJ {0}) já cadastrada", lojaDao.Cnpj));
            }
        }

        private void ValidarAlterar(LojaDao lojaDao, out Loja loja)
        {
            if (lojaDao == null)
            {
                throw new BusinessException("Loja é obrigatório");
            }

            if (lojaDao.LojaID <= 0)
            {
                throw new BusinessException("LojaID é obrigatório");
            }

            loja = lojaRepository.Listar(new Loja() { LojaID = lojaDao.LojaID, Ativo = true }).FirstOrDefault();

            if (loja == null)
            {
                throw new BusinessException(string.Format("Loja {0} não encontrado", lojaDao.LojaID));
            }

            if (string.IsNullOrEmpty(lojaDao.Cnpj) &&
                string.IsNullOrEmpty(lojaDao.NomeFantasia) &&
                string.IsNullOrEmpty(lojaDao.RazaoSocial) &&
                string.IsNullOrEmpty(lojaDao.Telefone) &&
                string.IsNullOrEmpty(lojaDao.Bairro))
            {
                throw new BusinessException("Infome algum campo a ser atualizado");
            }
        }

        private void ValidarExcluir(LojaDao lojaDao, out Loja loja)
        {
            if (lojaDao == null)
            {
                throw new BusinessException("Loja é obrigatório");
            }

            if (lojaDao.LojaID <= 0)
            {
                throw new BusinessException("LojaID é obrigatório");
            }

            loja = lojaRepository.Listar(new Loja() { LojaID = lojaDao.LojaID, Ativo = true }).FirstOrDefault();

            if (loja == null)
            {
                throw new BusinessException(string.Format("Loja {0} não encontrada", lojaDao.LojaID));
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

        public void Alterar(LojaDao lojaDao)
        {
            try
            {
                Loja loja;

                ValidarAlterar(lojaDao, out loja);

                if (!string.IsNullOrEmpty(lojaDao.Cnpj))
                {
                    loja.Cnpj = lojaDao.Cnpj.Trim().Replace(".", "").Replace("/", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(lojaDao.NomeFantasia))
                {
                    loja.NomeFantasia = lojaDao.NomeFantasia;
                }

                if (!string.IsNullOrEmpty(lojaDao.RazaoSocial))
                {
                    loja.RazaoSocial = lojaDao.RazaoSocial;
                }

                if (!string.IsNullOrEmpty(lojaDao.Telefone))
                {
                    loja.Telefone = lojaDao.Telefone.Trim().Replace(".", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace(" ", "");
                }

                if (!string.IsNullOrEmpty(lojaDao.Bairro))
                {
                    loja.Bairro = lojaDao.Bairro;
                }

                lojaRepository.Alterar(loja);
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

        public void Excluir(LojaDao lojaDao)
        {
            try
            {
                Loja loja;

                ValidarExcluir(lojaDao, out loja);

                lojaRepository.Excluir(loja);
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

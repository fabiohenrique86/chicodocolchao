using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;

namespace ChicoDoColchao.Business
{
    public class RelatorioBusiness
    {
        RelatorioRepository relatorioRepository;
        LogRepository logRepository;

        public RelatorioBusiness()
        {
            relatorioRepository = new RelatorioRepository();
            logRepository = new LogRepository();
        }

        private void ValidarComissao(ComissaoDao comissaoDao)
        {
            if (comissaoDao == null)
            {
                throw new BusinessException("ComissaoDao é obrigatório");
            }

            if (comissaoDao.DataInicio == DateTime.MinValue)
            {
                throw new BusinessException("Data início é obrigatório");
            }

            if (comissaoDao.DataFim == DateTime.MinValue)
            {
                throw new BusinessException("Data fim é obrigatório");
            }
        }

        private void ValidarVendaConsultor(VendaConsultorDao vendaConsultorDao)
        {
            if (vendaConsultorDao == null)
            {
                throw new BusinessException("VendaConsultorDao é obrigatório");
            }

            if (vendaConsultorDao.DataInicio == DateTime.MinValue)
            {
                throw new BusinessException("Data início é obrigatório");
            }

            if (vendaConsultorDao.DataFim == DateTime.MinValue)
            {
                throw new BusinessException("Data fim é obrigatório");
            }
        }

        private void ValidarVendaLoja(VendaLojaDao vendaLojaDao)
        {
            if (vendaLojaDao == null)
            {
                throw new BusinessException("VendaLojaDao é obrigatório");
            }

            if (vendaLojaDao.DataInicio == DateTime.MinValue)
            {
                throw new BusinessException("Data início é obrigatório");
            }

            if (vendaLojaDao.DataFim == DateTime.MinValue)
            {
                throw new BusinessException("Data fim é obrigatório");
            }
        }

        private void ValidarVendaProduto(VendaProdutoDao vendaProdutoDao)
        {
            if (vendaProdutoDao == null)
            {
                throw new BusinessException("VendaProdutoDao é obrigatório");
            }

            if (vendaProdutoDao.DataInicio == DateTime.MinValue)
            {
                throw new BusinessException("Data início é obrigatório");
            }

            if (vendaProdutoDao.DataFim == DateTime.MinValue)
            {
                throw new BusinessException("Data fim é obrigatório");
            }
        }

        public List<ComissaoDao> Comissao(ComissaoDao comissaoDao)
        {
            try
            {
                ValidarComissao(comissaoDao);

                return relatorioRepository.Comissao(comissaoDao);
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

        public List<VendaConsultorDao> VendaConsultor(VendaConsultorDao vendaConsultorDao)
        {
            try
            {
                ValidarVendaConsultor(vendaConsultorDao);

                return relatorioRepository.VendaConsultor(vendaConsultorDao);
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

        public List<VendaLojaDao> VendaLoja(VendaLojaDao vendaLojaDao)
        {
            try
            {
                ValidarVendaLoja(vendaLojaDao);

                return relatorioRepository.VendaLoja(vendaLojaDao);
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

        public List<VendaProdutoDao> VendaProduto(VendaProdutoDao vendaProdutoDao)
        {
            try
            {
                ValidarVendaProduto(vendaProdutoDao);

                return relatorioRepository.VendaProduto(vendaProdutoDao);
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

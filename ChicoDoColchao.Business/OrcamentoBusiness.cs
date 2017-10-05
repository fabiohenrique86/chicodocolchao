using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Business
{
    public class OrcamentoBusiness
    {
        OrcamentoRepository orcamentoRepository;
        OrcamentoHistoricoRepository orcamentoHistoricoRepository;
        LogRepository logRepository;

        public OrcamentoBusiness()
        {
            orcamentoRepository = new OrcamentoRepository();
            orcamentoHistoricoRepository = new OrcamentoHistoricoRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(OrcamentoDao orcamentoDao)
        {
            if (orcamentoDao == null)
            {
                throw new BusinessException("Orçamento é obrigatório");
            }

            if (orcamentoDao.LojaDao == null || orcamentoDao.LojaDao.Count() <= 0 || orcamentoDao.LojaDao.Count(x => x.LojaID <= 0) > 0)
            {
                throw new BusinessException("Loja é obrigatório");
            }

            if (orcamentoDao.FuncionarioDao == null || orcamentoDao.FuncionarioDao.Count() <= 0 || orcamentoDao.FuncionarioDao.Count(x => x.FuncionarioID <= 0) > 0)
            {
                throw new BusinessException("Funcionário é obrigatório");
            }

            if (orcamentoDao.ClienteDao == null || orcamentoDao.ClienteDao.ClienteID <= 0)
            {
                throw new BusinessException("Cliente é obrigatório");
            }

            if (orcamentoDao.DataOrcamento == DateTime.MinValue)
            {
                throw new BusinessException("Data Orcamento é obrigatório");
            }

            if (orcamentoDao.OrcamentoProdutoDao == null || orcamentoDao.OrcamentoProdutoDao.Count() <= 0 || orcamentoDao.OrcamentoProdutoDao.Count(x => x.ProdutoID <= 0) > 0)
            {
                throw new BusinessException("Produto é obrigatório");
            }
        }

        private void ValidarAtualizar(OrcamentoDao orcamentoDao)
        {
            if (orcamentoDao == null)
            {
                throw new BusinessException("Orçamento é obrigatório");
            }            

            if (orcamentoDao.OrcamentoID <= 0)
            {
                throw new BusinessException("OrcamentoID é obrigatório");
            }
        }

        public int Incluir(OrcamentoDao orcamentoDao)
        {
            int orcamentoId = 0;

            try
            {
                ValidarIncluir(orcamentoDao);

                orcamentoId = orcamentoRepository.Incluir(orcamentoDao.ToBd());

                return orcamentoId;
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
            finally
            {
                // insere o histórico do orçamentos
                if (orcamentoId > 0)
                {
                    orcamentoHistoricoRepository.Incluir(new OrcamentoHistorico()
                    {
                        OrcamentoID = orcamentoId,
                        DataCadastro = DateTime.Now,
                        Observacao = "Cadastro do Orçamento"
                    });
                }
            }
        }

        public List<OrcamentoDao> Listar(OrcamentoDao orcamentoDao)
        {
            try
            {
                return orcamentoRepository.Listar(orcamentoDao.ToBd(), true, 50).Select(x => x.ToApp()).ToList();
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

        public void Atualizar(OrcamentoDao orcamentoDao)
        {
            try
            {
                ValidarAtualizar(orcamentoDao);

                orcamentoRepository.Atualizar(orcamentoDao.ToBd());

                // insere o histórico do orçamentos
                orcamentoHistoricoRepository.Incluir(new OrcamentoHistorico()
                {
                    OrcamentoID = orcamentoDao.OrcamentoID,
                    DataCadastro = DateTime.Now,
                    Observacao = "Foi gerada venda do Orçamento"
                });
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

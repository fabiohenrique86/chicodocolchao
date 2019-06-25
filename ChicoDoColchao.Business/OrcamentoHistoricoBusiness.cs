using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Business
{
    public class OrcamentoHistoricoBusiness
    {
        OrcamentoHistoricoRepository orcamentoHistoricoRepository;
        LogRepository logRepository;

        public OrcamentoHistoricoBusiness()
        {
            orcamentoHistoricoRepository = new OrcamentoHistoricoRepository();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            if (orcamentoHistoricoDao == null)
            {
                throw new BusinessException("OrçamentoHistórico é obrigatório");
            }

            if (orcamentoHistoricoDao.OrcamentoID <= 0)
            {
                throw new BusinessException("OrcamentoID é obrigatório");
            }

            if (orcamentoHistoricoDao.DataCadastro == DateTime.MinValue)
            {
                throw new BusinessException("DataCadastro é obrigatório");
            }

            if (string.IsNullOrEmpty(orcamentoHistoricoDao.Observacao))
            {
                throw new BusinessException("Observação é obrigatório");
            }
        }

        public int Incluir(OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            int orcamentoHistoricoId = 0;

            try
            {
                ValidarIncluir(orcamentoHistoricoDao);

                orcamentoHistoricoId = orcamentoHistoricoRepository.Incluir(orcamentoHistoricoDao.ToBd());

                return orcamentoHistoricoId;
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

        public List<OrcamentoHistoricoDao> Listar(OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            try
            {
                return orcamentoHistoricoRepository.Listar(orcamentoHistoricoDao.ToBd(), true, 50).Select(x => x.ToApp()).ToList();
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

using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class OrcamentoHistoricoTradutor
    {
        public static OrcamentoHistorico ToBd(this OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            var orcamentoHistorico = new OrcamentoHistorico();

            orcamentoHistorico.OrcamentoHistoricoID = orcamentoHistoricoDao.OrcamentoHistoricoID;
            orcamentoHistorico.OrcamentoID = orcamentoHistoricoDao.OrcamentoID;
            orcamentoHistorico.DataCadastro = orcamentoHistoricoDao.DataCadastro;
            orcamentoHistorico.Observacao = orcamentoHistoricoDao.Observacao;

            return orcamentoHistorico;
        }

        public static OrcamentoHistoricoDao ToApp(this OrcamentoHistorico orcamentoHistorico)
        {
            var orcamentoHistoricoDao = new OrcamentoHistoricoDao();

            orcamentoHistoricoDao.OrcamentoHistoricoID = orcamentoHistorico.OrcamentoHistoricoID;
            orcamentoHistoricoDao.OrcamentoID = orcamentoHistorico.OrcamentoID;
            orcamentoHistoricoDao.DataCadastro = orcamentoHistorico.DataCadastro;
            orcamentoHistoricoDao.Observacao = orcamentoHistorico.Observacao;

            return orcamentoHistoricoDao;
        }
    }
}

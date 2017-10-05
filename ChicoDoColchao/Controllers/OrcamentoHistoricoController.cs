using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class OrcamentoHistoricoController : BaseController
    {
        private OrcamentoHistoricoBusiness orcamentoHistoricoBusiness;

        public OrcamentoHistoricoController()
        {
            orcamentoHistoricoBusiness = new OrcamentoHistoricoBusiness();
        }

        [HttpPost]
        public JsonResult Incluir(OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            try
            {
                orcamentoHistoricoDao.DataCadastro = DateTime.Now;

                int orcamentohistoricoID = orcamentoHistoricoBusiness.Incluir(orcamentoHistoricoDao);

                var orcamentosHistoricosDao = orcamentoHistoricoBusiness.Listar(new OrcamentoHistoricoDao() { OrcamentoID = orcamentoHistoricoDao.OrcamentoID });

                return Json(new { Sucesso = true, Mensagem = string.Format("Histórico {0} cadastrado com sucesso!", orcamentohistoricoID), Lista = orcamentosHistoricosDao }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro. Histórico não cadastrado. Tente novamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Listar(OrcamentoHistoricoDao orcamentoHistoricoDao)
        {
            List<OrcamentoHistoricoDao> orcamentosHistoricosDao = new List<OrcamentoHistoricoDao>();

            try
            {
                orcamentosHistoricosDao = orcamentoHistoricoBusiness.Listar(orcamentoHistoricoDao);

                return Json(orcamentosHistoricosDao, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(orcamentosHistoricosDao, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(orcamentosHistoricosDao, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
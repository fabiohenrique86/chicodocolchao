using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class RelatorioController : BaseController
    {
        private LojaBusiness lojaBusiness;
        private ProdutoBusiness produtoBusiness;
        private RelatorioBusiness relatorioBusiness;
        private ConsultorBusiness consultorBusiness;

        public RelatorioController()
        {
            lojaBusiness = new LojaBusiness();
            produtoBusiness = new ProdutoBusiness();
            relatorioBusiness = new RelatorioBusiness();
            consultorBusiness = new ConsultorBusiness();
        }

        public ActionResult Estoque()
        {
            var lojasDao = new List<LojaDao>();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                lojasDao = lojaBusiness.Listar(new LojaDao() { Ativo = true });
            }
            catch (Exception ex)
            {

            }

            return View(lojasDao);
        }

        public ActionResult VendaConsultor()
        {
            var consultoresDao = new List<ConsultorDao>();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                consultoresDao = consultorBusiness.Listar(new ConsultorDao() { Ativo = true });
            }
            catch (Exception ex)
            {

            }

            ViewBag.ConsultoresDao = consultoresDao;

            return View();
        }

        public ActionResult VendaLoja()
        {
            var lojasDao = new List<LojaDao>();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                var lojaDao = new LojaDao() { Ativo = true };
                var usuarioDao = JsonConvert.DeserializeObject<UsuarioDao>(Request.Cookies.Get("ChicoDoColchao_Usuario").Value);
                if (usuarioDao.TipoUsuarioDao != null && usuarioDao.TipoUsuarioDao.TipoUsuarioID == (int)TipoUsuarioDao.ETipoUsuario.Externo)
                {
                    lojaDao.Cnpj = "25313803000127";
                }

                lojasDao = lojaBusiness.Listar(lojaDao);
            }
            catch (Exception ex)
            {

            }

            ViewBag.LojasDao = lojasDao;

            return View();
        }

        public ActionResult VendaProduto()
        {
            string tela = "";
            if (!SessaoAtivaEPerfilValidado(out tela))
            {
                Response.Redirect(tela, true);
                return null;
            }

            return View();
        }

        public ActionResult Comissao()
        {
            var consultoresDao = new List<ConsultorDao>();

            try
            {
                string tela = "";
                if (!SessaoAtivaEPerfilValidado(out tela))
                {
                    Response.Redirect(tela, true);
                    return null;
                }

                consultoresDao = consultorBusiness.Listar(new ConsultorDao() { Ativo = true });
            }
            catch (Exception ex)
            {

            }

            ViewBag.ConsultoresDao = consultoresDao;

            return View();
        }

        public JsonResult ListarEstoque(int lojaId = 0, int produtoId = 0)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamIds;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_estoque_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Estoque.rdlc");

                var produtos = new List<dynamic>();
                var produtosDao = produtoBusiness.Listar(new ProdutoDao() { ProdutoID = produtoId }, lojaId);

                foreach (var produto in produtosDao)
                {
                    foreach (var lojaProduto in produto.LojaProdutoDao)
                    {
                        produtos.Add(new
                        {
                            ProdutoID = produto.ProdutoID,
                            Numero = produto.Numero,
                            Descricao = produto.Descricao,
                            LojaID = lojaProduto.LojaID,
                            NomeFantasia = lojaProduto.LojaDao.NomeFantasia,
                            Quantidade = lojaProduto.Quantidade
                        });
                    }
                }
                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_estoque", produtos));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamIds, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar relatório. Tente novamente", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarComissao(int funcionarioId = 0, string dataInicio = null, string dataFim = null)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamIds;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_comissao_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/Comissao.rdlc");

                DateTime dtInicio;
                DateTime.TryParse(dataInicio, out dtInicio);

                DateTime dtFim;
                DateTime.TryParse(dataFim, out dtFim);

                var comissaoDao = relatorioBusiness.Comissao(new ComissaoDao() { FuncionarioID = funcionarioId, DataInicio = dtInicio, DataFim = dtFim });

                var parametros = new List<ReportParameter>();

                parametros.Add(new ReportParameter("p_data_inicio", dataInicio));
                parametros.Add(new ReportParameter("p_data_fim", dataFim));

                viewer.LocalReport.SetParameters(parametros);

                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_comissao", comissaoDao));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamIds, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar relatório. Tente novamente", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarVendaConsultor(int funcionarioId = 0, string dataInicio = null, string dataFim = null)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamIds;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_venda_consultor_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/VendaConsultor.rdlc");

                DateTime dtInicio;
                DateTime.TryParse(dataInicio, out dtInicio);

                DateTime dtFim;
                DateTime.TryParse(dataFim, out dtFim);

                var vendaDao = relatorioBusiness.VendaConsultor(new VendaConsultorDao() { FuncionarioID = funcionarioId, DataInicio = dtInicio, DataFim = dtFim });

                var parametros = new List<ReportParameter>();

                parametros.Add(new ReportParameter("p_data_inicio", dataInicio));
                parametros.Add(new ReportParameter("p_data_fim", dataFim));

                viewer.LocalReport.SetParameters(parametros);

                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_venda_consultor", vendaDao));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamIds, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar relatório. Tente novamente", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ListarVendaLoja(int lojaId = 0, string dataInicio = null, string dataFim = null)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamIds;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_venda_loja_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/VendaLoja.rdlc");

                DateTime dtInicio;
                DateTime.TryParse(dataInicio, out dtInicio);

                DateTime dtFim;
                DateTime.TryParse(dataFim, out dtFim);

                var vendaDao = relatorioBusiness.VendaLoja(new VendaLojaDao() { LojaID = lojaId, DataInicio = dtInicio, DataFim = dtFim });

                var parametros = new List<ReportParameter>();

                parametros.Add(new ReportParameter("p_data_inicio", dataInicio));
                parametros.Add(new ReportParameter("p_data_fim", dataFim));

                viewer.LocalReport.SetParameters(parametros);

                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_venda_loja", vendaDao));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamIds, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar relatório. Tente novamente", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult ListarVendaProduto(string dataInicio = null, string dataFim = null, List<int> produtosDao = null)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamIds;
                string encoding;
                string filenameExtension;

                var arquivo = string.Format("relatorio_venda_produto_{0}.pdf", DateTime.Now.ToString("dd_MM_yyyy_HH_mm"));
                var caminho = string.Format(Server.MapPath("~") + arquivo);
                var tipo = "application/pdf";

                var viewer = new ReportViewer();

                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = Server.MapPath("~/Reports/VendaProduto.rdlc");

                DateTime dtInicio;
                DateTime.TryParse(dataInicio, out dtInicio);

                DateTime dtFim;
                DateTime.TryParse(dataFim, out dtFim);

                var vendaProdutoDao = relatorioBusiness.VendaProduto(new VendaProdutoDao() { DataInicio = dtInicio, DataFim = dtFim, ProdutoDao = produtosDao });

                var parametros = new List<ReportParameter>();

                parametros.Add(new ReportParameter("p_data_inicio", dataInicio));
                parametros.Add(new ReportParameter("p_data_fim", dataFim));

                viewer.LocalReport.SetParameters(parametros);

                viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_venda_produto", vendaProdutoDao));

                viewer.LocalReport.Refresh();

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamIds, out warnings);

                System.IO.File.WriteAllBytes(caminho, bytes);

                return Json(new { Sucesso = true, Mensagem = "Relatório gerado com sucesso", Arquivo = arquivo, Caminho = caminho, Tipo = tipo }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao gerar relatório. Tente novamente", Erro = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
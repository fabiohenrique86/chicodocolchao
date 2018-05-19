using ChicoDoColchao.Business;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Dao;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ChicoDoColchao.Controllers
{
    public class RelatorioController : BaseController
    {
        private LojaBusiness lojaBusiness;
        private ProdutoBusiness produtoBusiness;

        public RelatorioController()
        {
            lojaBusiness = new LojaBusiness();
            produtoBusiness = new ProdutoBusiness();
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

        public JsonResult ListarEstoque(int lojaId = 0, int produtoId = 0)
        {
            try
            {
                Warning[] warnings;
                string mimeType;
                string[] streamids;
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

                var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

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
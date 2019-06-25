using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using Microsoft.Reporting.WebForms;
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

            if (orcamentoDao.ConsultorDao == null || orcamentoDao.ConsultorDao.Count() <= 0 || orcamentoDao.ConsultorDao.Count(x => x.FuncionarioID <= 0) > 0)
            {
                throw new BusinessException("Consultor é obrigatório");
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
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public byte[] Comanda(OrcamentoDao orcamentoDao)
        {
            if (orcamentoDao == null)
            {
                return null;
            }

            Warning[] warnings;
            string mimeType;
            string[] streamids;
            string encoding;
            string filenameExtension;

            var viewer = new ReportViewer();

            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory + "/Reports/OrcamentoComanda.rdlc";

            // parâmetros
            var parametros = new List<ReportParameter>();

            parametros.Add(new ReportParameter("OrcamentoID", orcamentoDao.OrcamentoID.ToString()));
            parametros.Add(new ReportParameter("Observacao", orcamentoDao.Observacao));
            parametros.Add(new ReportParameter("RazaoSocial", orcamentoDao.LojaDao.FirstOrDefault().RazaoSocial));
            parametros.Add(new ReportParameter("Cnpj", orcamentoDao.LojaDao.FirstOrDefault().Cnpj));
            parametros.Add(new ReportParameter("Telefone", orcamentoDao.LojaDao.FirstOrDefault().Telefone));
            parametros.Add(new ReportParameter("Desconto", orcamentoDao.Desconto.ToString()));
            parametros.Add(new ReportParameter("Funcionario", orcamentoDao.ConsultorDao.FirstOrDefault().Nome));
            parametros.Add(new ReportParameter("DataOrcamento", orcamentoDao.DataOrcamento.ToString("dd/MM/yyyy")));            
            parametros.Add(new ReportParameter("TotalOrcamento", Math.Round(orcamentoDao.OrcamentoProdutoDao.Sum(x => x.Preco * x.Quantidade) - orcamentoDao.Desconto, 2).ToString()));

            viewer.LocalReport.SetParameters(parametros);

            // cliente
            var clientesDao = new List<dynamic>();

            clientesDao.Add(new
            {
                ClienteID = orcamentoDao.ClienteDao.ClienteID,
                Cpf = orcamentoDao.ClienteDao.Cpf,
                Cnpj = orcamentoDao.ClienteDao.Cnpj,
                Nome = orcamentoDao.ClienteDao.Nome,
                DataNascimento = orcamentoDao.ClienteDao.DataNascimento,
                NomeFantasia = orcamentoDao.ClienteDao.NomeFantasia,
                RazaoSocial = orcamentoDao.ClienteDao.RazaoSocial,
                TelefoneResidencial = orcamentoDao.ClienteDao.TelefoneResidencial,
                TelefoneCelular = orcamentoDao.ClienteDao.TelefoneCelular,
                TelefoneResidencial2 = orcamentoDao.ClienteDao.TelefoneResidencial2,
                TelefoneCelular2 = orcamentoDao.ClienteDao.TelefoneCelular2,
                Estado = orcamentoDao.ClienteDao.EstadoDao.FirstOrDefault().Nome,
                Cidade = orcamentoDao.ClienteDao.Cidade,
                Logradouro = orcamentoDao.ClienteDao.Logradouro,
                Numero = orcamentoDao.ClienteDao.Numero,
                Bairro = orcamentoDao.ClienteDao.Bairro,
                Complemento = orcamentoDao.ClienteDao.Complemento,
                PontoReferencia = orcamentoDao.ClienteDao.PontoReferencia,
                Email = orcamentoDao.ClienteDao.Email,
                Ativo = orcamentoDao.ClienteDao.Ativo,
                Cep = orcamentoDao.ClienteDao.Cep
            });

            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_cliente", clientesDao));

            // produtos
            var pedidoProdutosDao = new List<dynamic>();
            foreach (var orcamentoProdutoDao in orcamentoDao.OrcamentoProdutoDao)
            {
                pedidoProdutosDao.Add(new
                {
                    Numero = orcamentoProdutoDao.ProdutoDao.Numero,
                    Descricao = orcamentoProdutoDao.ProdutoDao.Descricao,
                    Medida = orcamentoProdutoDao.ProdutoDao.MedidaDao.Descricao,
                    Quantidade = orcamentoProdutoDao.Quantidade,
                    Preco = orcamentoProdutoDao.Preco
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_produto", pedidoProdutosDao));
            
            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return bytes;
        }
    }
}

using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace ChicoDoColchao.Business
{
    public class PedidoBusiness
    {
        PedidoRepository pedidoRepository;
        ProdutoBusiness produtoBusiness;
        EmailBusiness emailBusiness;
        LogRepository logRepository;
        PedidoTipoPagamentoRepository pedidoTipoPagamentoRepository;
        OrcamentoBusiness orcamentoBusiness;

        public PedidoBusiness()
        {
            pedidoRepository = new PedidoRepository();
            produtoBusiness = new ProdutoBusiness();
            emailBusiness = new EmailBusiness();
            logRepository = new LogRepository();
            pedidoTipoPagamentoRepository = new PedidoTipoPagamentoRepository();
            orcamentoBusiness = new OrcamentoBusiness();
        }

        private void ValidarIncluir(PedidoDao pedidoDao)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            var funcionarioDao = pedidoDao.FuncionarioDao.FirstOrDefault();
            if (funcionarioDao == null || funcionarioDao.FuncionarioID <= 0)
            {
                throw new BusinessException("Funcionário é obrigatório");
            }

            var clienteDao = pedidoDao.ClienteDao.FirstOrDefault();
            if (clienteDao == null || clienteDao.ClienteID <= 0)
            {
                throw new BusinessException("Cliente (CPF ou CNPJ) é obrigatório");
            }
            
            var lojaDao = pedidoDao.LojaDao.FirstOrDefault();
            if (lojaDao == null || lojaDao.LojaID <= 0)
            {
                throw new BusinessException("Loja de origem é obrigatório");
            }

            var lojaSaidaDao = pedidoDao.LojaSaidaDao.FirstOrDefault();
            if (lojaSaidaDao == null || lojaSaidaDao.LojaID <= 0)
            {
                throw new BusinessException("Loja de saída é obrigatório");
            }

            var pedidoStatusDao = pedidoDao.PedidoStatusDao.FirstOrDefault();
            if (pedidoStatusDao == null || pedidoStatusDao.PedidoStatusID <= 0)
            {
                throw new BusinessException("Status do pedido é obrigatório");
            }

            if (pedidoDao.UsuarioPedidoDao == null || pedidoDao.UsuarioPedidoDao.UsuarioID <= 0)
            {
                throw new BusinessException("Usúario do pedido é obrigatório");
            }

            if (pedidoDao.PedidoProdutoDao == null || pedidoDao.PedidoProdutoDao.Count <= 0)
            {
                throw new BusinessException("Produto do pedido é obrigatório");
            }

            if (pedidoDao.PedidoTipoPagamentoDao == null || pedidoDao.PedidoTipoPagamentoDao.Count <= 0)
            {
                throw new BusinessException("Tipo de pagamento do pedido é obrigatório");
            }

            if (pedidoDao.DataPedido == null || pedidoDao.DataPedido == DateTime.MinValue)
            {
                throw new BusinessException("Data do pedido é obrigatório");
            }

            // verifica se o total do pedido é igual ao total pago
            var totalPedido = Math.Round(pedidoDao.PedidoProdutoDao.Sum(x => x.Preco * x.Quantidade), 2);
            var totalPago = Math.Round(pedidoDao.PedidoTipoPagamentoDao.Sum(x => x.ValorPago), 2);
            var totalDesconto = Math.Round(pedidoDao.Desconto, 2);

            if (Math.Round(totalPedido - totalDesconto, 2) != totalPago)
            {
                throw new BusinessException("Total do pedido deve ser igual ao total pago");
            }

            // verifica se o produto existe na loja de saída
            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao)
            {
                var produto = produtoBusiness.Listar(new ProdutoDao() { ProdutoID = pedidoProdutoDao.ProdutoID, Ativo = true }, lojaDestinoId: lojaSaidaDao.LojaID).FirstOrDefault();

                if (produto == null)
                {
                    throw new BusinessException(string.Format("Produto {0} não cadastrado na Loja", pedidoProdutoDao.ProdutoDao.Descricao));
                }

                if (pedidoStatusDao.PedidoStatusID == PedidoStatusDao.EPedidoStatus.PrevisaoDeEntrega.GetHashCode())
                {
                    if (pedidoProdutoDao.DataEntrega != null && pedidoProdutoDao.DataEntrega != DateTime.MinValue)
                    {
                        if (pedidoProdutoDao.DataEntrega.GetValueOrDefault().Date < DateTime.Now.Date)
                        {
                            throw new BusinessException(string.Format("Data da entrega do produto {0} não pode ser menor que hoje", pedidoProdutoDao.ProdutoDao.Descricao));
                        }

                        if (pedidoProdutoDao.UsuarioEntregaDao != null && pedidoProdutoDao.UsuarioEntregaDao.UsuarioID <= 0)
                        {
                            throw new BusinessException("Usuário de entrega é obrigatório");
                        }
                    }
                }
            }

            // verifica se o cv informado já existe
            foreach (var pedidoTipoPagamentoDao in pedidoDao.PedidoTipoPagamentoDao.Where(x => x.CV > 0))
            {
                var ptp = pedidoTipoPagamentoRepository.Listar(new PedidoTipoPagamento() { CV = pedidoTipoPagamentoDao.CV });
                if (ptp != null && ptp.Count() > 0)
                {
                    throw new BusinessException(string.Format("CV {0} já cadastrado", pedidoTipoPagamentoDao.CV));
                }
            }
        }

        private void ValidarCancelar(PedidoDao pedidoDao, out Pedido pedido)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }

            if (pedidoDao.DataCancelamento == null || pedidoDao.DataCancelamento == DateTime.MinValue)
            {
                throw new BusinessException("Data de cancelamento é obrigatório");
            }

            if (pedidoDao.UsuarioCancelamentoDao == null || pedidoDao.UsuarioCancelamentoDao.UsuarioID <= 0)
            {
                throw new BusinessException("Usuário é obrigatório");
            }

            pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }, true, 1).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }
        }

        private void ValidarDarBaixa(PedidoDao pedidoDao, out Pedido pedido)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }

            var pedidoStatusDao = pedidoDao.PedidoStatusDao.FirstOrDefault();
            if (pedidoStatusDao == null || pedidoStatusDao.PedidoStatusID <= 0)
            {
                throw new BusinessException("Status do pedido é obrigatório");
            }

            if (pedidoDao.PedidoProdutoDao == null || pedidoDao.PedidoProdutoDao.Count() <= 0)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao)
            {
                if (pedidoProdutoDao.ProdutoID <= 0)
                {
                    throw new BusinessException("Produto é obrigatório");
                }

                if (pedidoProdutoDao.UsuarioBaixaDao == null || pedidoProdutoDao.UsuarioBaixaDao.UsuarioID <= 0)
                {
                    throw new BusinessException("Usuário da baixa é obrigatório");
                }

                if (pedidoProdutoDao.DataBaixa == null || pedidoProdutoDao.DataBaixa == DateTime.MinValue)
                {
                    throw new BusinessException("Data da baixa é obrigatório");
                }
            }

            pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }, true, 10).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }
        }

        private void ValidarAtualizar(PedidoDao pedidoDao, out Pedido pedido)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }

            if (pedidoDao.PedidoProdutoDao == null || pedidoDao.PedidoProdutoDao.Count() <= 0)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }, true, 10).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }

            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao)
            {
                if (pedidoProdutoDao.ProdutoID <= 0 && (pedidoProdutoDao.ProdutoDao == null || pedidoProdutoDao.ProdutoDao.Numero <= 0))
                {
                    throw new BusinessException("Produto é obrigatório");
                }

                if (pedidoProdutoDao.DataEntrega == null || pedidoProdutoDao.DataEntrega == DateTime.MinValue)
                {
                    throw new BusinessException("Data de entrega é obrigatório");
                }

                if (pedidoProdutoDao.DataEntrega.GetValueOrDefault().Date < DateTime.Now.Date)
                {
                    throw new BusinessException("Data de entrega deve ser maior ou igual a data de hoje");
                }

                if (pedidoProdutoDao.DataEntrega.GetValueOrDefault().Date < pedido.DataPedido.Date)
                {
                    throw new BusinessException("Data de entrega deve ser maior ou igual a data do pedido");
                }

                if (pedidoProdutoDao.UsuarioEntregaDao == null || pedidoProdutoDao.UsuarioEntregaDao.UsuarioID <= 0)
                {
                    throw new BusinessException("Usuário de entrega é obrigatório");
                }
            }
        }

        public List<PedidoDao> Listar(PedidoDao pedidoDao)
        {
            try
            {
                return pedidoRepository.Listar(pedidoDao.ToBd(), true, 50).Select(x => x.ToApp()).ToList();
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

        public int Incluir(PedidoDao pedidoDao)
        {
            try
            {
                var email = string.Empty;
                var erro = string.Empty;

                ValidarIncluir(pedidoDao);

                var pedidoId = pedidoRepository.Incluir(pedidoDao.ToBd());

                AtualizarOrcamento(pedidoDao.OrcamentoDao.FirstOrDefault(), pedidoId);
                                
                EnviarComandaPorEmail(pedidoId, out email, out erro);

                return pedidoId;
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

        private void AtualizarOrcamento(OrcamentoDao orcamentoDao, int pedidoId)
        {
            if (orcamentoDao != null)
            {
                orcamentoDao.PedidoDao = new PedidoDao() { PedidoID = pedidoId };
                orcamentoBusiness.Atualizar(orcamentoDao);
            }
        }

        public bool EnviarComandaPorEmail(int pedidoId, out string email, out string erro)
        {
            email = string.Empty;
            erro = string.Empty;

            try
            {
                var pedidoDao = new PedidoRepository().Listar(new Pedido() { PedidoID = pedidoId }, true, 1).Select(x => x.ToApp()).ToList().FirstOrDefault();

                if (pedidoDao == null)
                {
                    erro = $"Pedido {pedidoId} não encontrado";
                    return false;
                }

                if (pedidoDao.ClienteDao.FirstOrDefault() == null || string.IsNullOrEmpty(pedidoDao.ClienteDao.FirstOrDefault().Email))
                {
                    erro = $"E-mail do cliente não encontrado";
                    return false;
                }

                var mensagem = $"Olá {pedidoDao.ClienteDao.FirstOrDefault().Nome.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()}.<br /><br />";
                mensagem += $"Seu pedido {pedidoDao.PedidoID} foi recebido com sucesso!<br /><br />";
                mensagem += "Em anexo, segue todos os detalhes do seu pedido.<br /><br />";
                mensagem += "A Chico do Colchão agradece a preferência!";

                var bytes = Comanda(pedidoDao);
                var stream = new MemoryStream(bytes);

                var emailDao = new EmailDao();

                emailDao.Titulo = "Chico do Colchão";
                emailDao.Assunto = $"Seu Pedido {pedidoDao.PedidoID}";
                emailDao.Remetente = "contato@chicodocolchao.com.br";
                emailDao.Destinatario = pedidoDao.ClienteDao.FirstOrDefault().Email;
                emailDao.Mensagem = mensagem;
                emailDao.Anexo.Add(new Attachment(stream, $"Pedido {pedidoDao.PedidoID}", "application/pdf"));

                emailBusiness.Enviar(emailDao);

                email = pedidoDao.ClienteDao.FirstOrDefault().Email;

                return true;
            }
            catch (Exception ex)
            {
                erro = ex.Message;

                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });
            }

            return false;
        }

        public void Cancelar(PedidoDao pedidoDao)
        {
            try
            {
                Pedido pedido;

                ValidarCancelar(pedidoDao, out pedido);

                pedido.DataCancelamento = pedidoDao.DataCancelamento;
                pedido.UsuarioCancelamentoID = pedidoDao.UsuarioCancelamentoDao.UsuarioID;
                pedido.PedidoStatusID = pedidoDao.PedidoStatusDao.First().PedidoStatusID;

                pedidoRepository.Cancelar(pedido);
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

        public void DarBaixa(PedidoDao pedidoDao)
        {
            try
            {
                Pedido pedido;

                ValidarDarBaixa(pedidoDao, out pedido);

                // pedido produto
                var pedidoProduto = pedido.PedidoProduto.FirstOrDefault(x => x.ProdutoID == pedidoDao.PedidoProdutoDao.FirstOrDefault().ProdutoID);
                pedidoProduto.DataBaixa = pedidoDao.PedidoProdutoDao.FirstOrDefault().DataBaixa;
                pedidoProduto.UsuarioBaixaID = pedidoDao.PedidoProdutoDao.FirstOrDefault().UsuarioBaixaDao.UsuarioID;

                // se não há data de entrega cadastrada, a data e usuário da baixa são iguais a da data e usuario da entrega
                if (pedidoProduto.DataEntrega == null || pedidoProduto.DataEntrega == DateTime.MinValue)
                {
                    pedidoProduto.DataEntrega = pedidoDao.PedidoProdutoDao.FirstOrDefault().DataBaixa;
                    pedidoProduto.UsuarioEntregaID = pedidoDao.PedidoProdutoDao.FirstOrDefault().UsuarioBaixaDao.UsuarioID;
                }

                // loja saída
                var ls = pedido.LojaSaida.LojaProduto.FirstOrDefault(x => x.ProdutoID == pedidoProduto.ProdutoID && x.LojaID == pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID);
                if (ls == null)
                {
                    throw new BusinessException($"Produto {pedidoProduto.ProdutoID} não cadastrado na loja {pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID}");
                }
                ls.Quantidade = Convert.ToInt16(ls.Quantidade - pedidoProduto.Quantidade);

                pedidoRepository.DarBaixa(pedido, pedidoDao.PedidoStatusDao.FirstOrDefault().PedidoStatusID);
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

        public void Atualizar(PedidoDao pedidoDao)
        {
            try
            {
                Pedido pedido;

                ValidarAtualizar(pedidoDao, out pedido);

                var pedidoProduto = pedido.PedidoProduto.FirstOrDefault(x => (x.ProdutoID == pedidoDao.PedidoProdutoDao.FirstOrDefault().ProdutoID || x.Produto.Numero == pedidoDao.PedidoProdutoDao.FirstOrDefault().ProdutoDao.Numero));

                if (pedidoProduto == null)
                {
                    throw new BusinessException("Produto não encontrado");
                }

                pedidoProduto.DataEntrega = pedidoDao.PedidoProdutoDao.FirstOrDefault().DataEntrega;
                pedidoProduto.UsuarioEntregaID = pedidoDao.PedidoProdutoDao.FirstOrDefault().UsuarioEntregaDao.UsuarioID;

                pedidoRepository.Atualizar(pedido);
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

        public byte[] Comanda(PedidoDao pedidoDao)
        {
            if (pedidoDao == null)
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
            viewer.LocalReport.ReportPath = AppDomain.CurrentDomain.BaseDirectory + "/bin/Reports/PedidoComanda.rdlc";

            // parâmetros
            List<ReportParameter> parametros = new List<ReportParameter>();

            parametros.Add(new ReportParameter("PedidoID", pedidoDao.PedidoID.ToString()));
            parametros.Add(new ReportParameter("Observacao", pedidoDao.Observacao));
            parametros.Add(new ReportParameter("RazaoSocial", pedidoDao.LojaDao.FirstOrDefault().RazaoSocial));
            parametros.Add(new ReportParameter("Cnpj", pedidoDao.LojaDao.FirstOrDefault().Cnpj));
            parametros.Add(new ReportParameter("Telefone", pedidoDao.LojaDao.FirstOrDefault().Telefone));
            parametros.Add(new ReportParameter("Desconto", pedidoDao.Desconto.ToString()));
            parametros.Add(new ReportParameter("Funcionario", pedidoDao.FuncionarioDao.FirstOrDefault().Nome));
            parametros.Add(new ReportParameter("DataPedido", pedidoDao.DataPedido.ToString("dd/MM/yyyy")));
            parametros.Add(new ReportParameter("Endereco", pedidoDao.ClienteDao.FirstOrDefault().Logradouro + " " +
                                                            pedidoDao.ClienteDao.FirstOrDefault().Numero + " " +
                                                            pedidoDao.ClienteDao.FirstOrDefault().Bairro + " " +
                                                            pedidoDao.ClienteDao.FirstOrDefault().Cidade + " - " +
                                                            pedidoDao.ClienteDao.FirstOrDefault().EstadoDao.FirstOrDefault().Nome + " CEP: " +
                                                            pedidoDao.ClienteDao.FirstOrDefault().Cep));
            parametros.Add(new ReportParameter("Complemento", pedidoDao.ClienteDao.FirstOrDefault().Complemento));
            parametros.Add(new ReportParameter("PontoReferencia", pedidoDao.ClienteDao.FirstOrDefault().PontoReferencia));
            parametros.Add(new ReportParameter("TotalPedido", Math.Round(pedidoDao.PedidoProdutoDao.Sum(x => x.Preco * x.Quantidade), 2).ToString()));

            viewer.LocalReport.SetParameters(parametros);

            // cliente
            List<dynamic> clienteDao = new List<dynamic>();
            foreach (var item in pedidoDao.ClienteDao)
            {
                clienteDao.Add(new
                {
                    ClienteID = item.ClienteID,
                    Cpf = item.Cpf,
                    Cnpj = item.Cnpj,
                    Nome = item.Nome,
                    DataNascimento = item.DataNascimento,
                    NomeFantasia = item.NomeFantasia,
                    RazaoSocial = item.RazaoSocial,
                    TelefoneResidencial = item.TelefoneResidencial,
                    TelefoneCelular = item.TelefoneCelular,
                    TelefoneResidencial2 = item.TelefoneResidencial2,
                    TelefoneCelular2 = item.TelefoneCelular2,
                    Estado = item.EstadoDao.FirstOrDefault().Nome,
                    Cidade = item.Cidade,
                    Logradouro = item.Logradouro,
                    Numero = item.Numero,
                    Bairro = item.Bairro,
                    Complemento = item.Complemento,
                    PontoReferencia = item.PontoReferencia,
                    Email = item.Email,
                    Ativo = item.Ativo,
                    Cep = item.Cep
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_cliente", clienteDao));

            // produto
            List<dynamic> pedidoProdutoDao = new List<dynamic>();
            foreach (var item in pedidoDao.PedidoProdutoDao)
            {
                pedidoProdutoDao.Add(new
                {
                    Numero = item.ProdutoDao.Numero,
                    Descricao = item.ProdutoDao.Descricao,
                    Medida = item.ProdutoDao.MedidaDao.Descricao,
                    Quantidade = item.Quantidade,
                    DataEntrega = item.DataEntrega,
                    DataBaixa = item.DataBaixa,
                    Preco = item.Preco
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_produto", pedidoProdutoDao));

            // tipo pagamento
            List<dynamic> pedidoTipoPagamentoDao = new List<dynamic>();
            foreach (var item in pedidoDao.PedidoTipoPagamentoDao)
            {
                pedidoTipoPagamentoDao.Add(new
                {
                    Descricao = item.TipoPagamentoDao.Descricao,
                    Numero = item.ParcelaDao.Numero,
                    ValorPago = item.ValorPago,
                    CV = item.CV
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_tipo_pagamento", pedidoTipoPagamentoDao));

            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return bytes;
        }
    }
}

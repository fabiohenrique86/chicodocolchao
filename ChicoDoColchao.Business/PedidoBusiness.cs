using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using System.IO;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;

namespace ChicoDoColchao.Business
{
    public class PedidoBusiness
    {
        PedidoRepository pedidoRepository;
        ProdutoBusiness produtoBusiness;
        EmailBusiness emailBusiness;
        LogRepository logRepository;
        PedidoTipoPagamentoRepository pedidoTipoPagamentoRepository;

        public PedidoBusiness()
        {
            pedidoRepository = new PedidoRepository();
            produtoBusiness = new ProdutoBusiness();
            emailBusiness = new EmailBusiness();
            logRepository = new LogRepository();
            pedidoTipoPagamentoRepository = new PedidoTipoPagamentoRepository();
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
                throw new BusinessException("Cliente (CPF/CNPJ) é obrigatório");
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

            if (pedidoDao.PedidoProdutoDao == null || pedidoDao.PedidoProdutoDao.Count <= 0)
            {
                throw new BusinessException("Produto do pedido é obrigatório");
            }

            if (pedidoDao.PedidoTipoPagamentoDao == null || pedidoDao.PedidoTipoPagamentoDao.Count <= 0)
            {
                throw new BusinessException("Tipo de pagamento do pedido é obrigatório");
            }

            if (pedidoDao.DataEntrega != null && pedidoDao.DataEntrega != DateTime.MinValue)
            {
                // se tem data de entrega, adiciona horas e minutos
                pedidoDao.DataEntrega = pedidoDao.DataEntrega.GetValueOrDefault().AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute);

                if (pedidoDao.DataEntrega < DateTime.Now)
                {
                    throw new BusinessException("Data da entrega do pedido não pode ser menor que hoje");
                }
            }

            // verifica se o total do pedido é igual ao total pago
            var totalPedido = Math.Round(pedidoDao.PedidoProdutoDao.Sum(x => x.Preco * x.Quantidade), 2);
            var totalPago = Math.Round(pedidoDao.PedidoTipoPagamentoDao.Sum(x => x.ValorPago * x.ParcelaDao.ParcelaID), 2);
            var totalDesconto = Math.Round(pedidoDao.Desconto, 2);

            if (Math.Round(totalPedido - totalDesconto, 2) != totalPago)
            {
                throw new BusinessException("Total do pedido deve ser igual ao total pago");
            }

            // verifica se o produto existe na loja de saída
            foreach (var pedidoProduto in pedidoDao.PedidoProdutoDao)
            {
                var produto = produtoBusiness.Listar(new ProdutoDao() { ProdutoID = pedidoProduto.ProdutoID, Ativo = true }, lojaDestinoId: lojaSaidaDao.LojaID).FirstOrDefault();
                if (produto == null)
                {
                    throw new BusinessException(string.Format("Produto {0} não cadastrado na Loja {1}", pedidoProduto.ProdutoDao.Descricao, lojaSaidaDao.NomeFantasia));
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

            pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }
        }

        private void ValidarEntregar(PedidoDao pedidoDao, out Pedido pedido)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }

            if (pedidoDao.DataEntrega == null || pedidoDao.DataEntrega == DateTime.MinValue)
            {
                throw new BusinessException("Data de entrega é obrigatório");
            }

            pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }
        }

        private void ValidarAtualizar(PedidoDao pedidoDao)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }

            var pedido = pedidoRepository.Listar(new Pedido() { PedidoID = pedidoDao.PedidoID }).FirstOrDefault();

            if (pedido == null)
            {
                throw new BusinessException(string.Format("Pedido {0} não encontrado", pedidoDao.PedidoID));
            }

            if (pedidoDao.DataEntrega != null || pedidoDao.DataEntrega != DateTime.MinValue)
            {
                if (pedidoDao.DataEntrega.GetValueOrDefault().Date < DateTime.Now.Date)
                {
                    throw new BusinessException("Data de entrega deve ser maior ou igual a data de hoje");
                }

                if (pedidoDao.DataEntrega.GetValueOrDefault().Date < pedido.DataPedido.Date)
                {
                    throw new BusinessException("Data de entrega deve ser maior ou igual a data do pedido");
                }
            }
        }

        public List<PedidoDao> Listar(PedidoDao pedidoDao)
        {
            try
            {
                return pedidoRepository.Listar(pedidoDao.ToBd()).Select(x => x.ToApp()).ToList();
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
                ValidarIncluir(pedidoDao);

                var pedidoId = pedidoRepository.Incluir(pedidoDao.ToBd());

                pedidoDao = new PedidoRepository().Listar(new Pedido() { PedidoID = pedidoId }).Select(x => x.ToApp()).ToList().FirstOrDefault();

                if (pedidoDao == null)
                {
                    return pedidoId;
                }

                // se houver e-mail do cliente, envia a comanda por email
                if (pedidoDao.ClienteDao.FirstOrDefault() != null && !string.IsNullOrEmpty(pedidoDao.ClienteDao.FirstOrDefault().Email))
                {
                    EnviarComandaPorEmail(pedidoDao);
                }

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

        public void EnviarComandaPorEmail(PedidoDao pedidoDao)
        {
            try
            {
                var cliente = pedidoDao.ClienteDao.FirstOrDefault();

                var mensagem = string.Format("Olá {0}.<br /><br />", cliente.Nome);
                mensagem += string.Format("Seu pedido {0} foi recebido com sucesso!<br /><br />", pedidoDao.PedidoID);
                mensagem += "Em anexo, segue todos os detalhes do seu pedido.<br /><br />";
                mensagem += "A Chico do Colchão agradece a preferência!";

                var bytes = Comanda(pedidoDao);
                Stream stream = new MemoryStream(bytes);

                EmailDao emailDao = new EmailDao();

                emailDao.Titulo = "Chico do Colchão";
                emailDao.Assunto = string.Format("Seu Pedido {0}", pedidoDao.PedidoID);
                emailDao.Remetente = "contato@chicodocolchao.com.br";
                emailDao.Destinatario = cliente.Email;
                emailDao.Mensagem = mensagem;
                emailDao.Anexo.Add(new Attachment(stream, string.Format("Pedido {0}", pedidoDao.PedidoID), "application/pdf"));

                emailBusiness.Enviar(emailDao);
            }
            catch (Exception ex)
            {
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });
            }
        }

        public void Cancelar(PedidoDao pedidoDao)
        {
            try
            {
                Pedido pedido;

                ValidarCancelar(pedidoDao, out pedido);

                pedido.PedidoStatusID = (int)PedidoStatusDao.EPedidoStatus.Cancelado;

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

        public void Entregar(PedidoDao pedidoDao)
        {
            try
            {
                Pedido pedido;

                ValidarEntregar(pedidoDao, out pedido);

                pedido.DataEntrega = pedidoDao.DataEntrega;
                pedido.PedidoStatusID = (int)PedidoStatusDao.EPedidoStatus.Entregue;

                pedidoRepository.Entregar(pedido);
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
                ValidarAtualizar(pedidoDao);

                pedidoRepository.Atualizar(pedidoDao.ToBd());
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
            parametros.Add(new ReportParameter("DataEntrega", pedidoDao.DataEntrega.GetValueOrDefault() == DateTime.MinValue ? "" : pedidoDao.DataEntrega.GetValueOrDefault().ToString("dd/MM/yyyy")));
            parametros.Add(new ReportParameter("Endereco", pedidoDao.ClienteDao.FirstOrDefault().Logradouro + " " + pedidoDao.ClienteDao.FirstOrDefault().Numero + " " + pedidoDao.ClienteDao.FirstOrDefault().Bairro + " " + pedidoDao.ClienteDao.FirstOrDefault().Cidade + " - " + pedidoDao.ClienteDao.FirstOrDefault().EstadoDao.FirstOrDefault().Nome));
            parametros.Add(new ReportParameter("Complemento", pedidoDao.ClienteDao.FirstOrDefault().Complemento));
            parametros.Add(new ReportParameter("PontoReferencia", pedidoDao.ClienteDao.FirstOrDefault().PontoReferencia));
            parametros.Add(new ReportParameter("ValorFrete", pedidoDao.ValorFrete.GetValueOrDefault().ToString())); 

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
                    Ativo = item.Ativo
                });
            }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_cliente", clienteDao));

            // produto
            List<dynamic> pedidoProdutoDao = new List<dynamic>();
            foreach (var item in pedidoDao.PedidoProdutoDao) { pedidoProdutoDao.Add(new { Numero = item.ProdutoDao.Numero, Descricao = item.ProdutoDao.Descricao, Medida = item.ProdutoDao.MedidaDao.Descricao, Quantidade = item.Quantidade }); }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_produto", pedidoProdutoDao));

            // tipo pagamento
            List<dynamic> pedidoTipoPagamentoDao = new List<dynamic>();
            foreach (var item in pedidoDao.PedidoTipoPagamentoDao) { pedidoTipoPagamentoDao.Add(new { Descricao = item.TipoPagamentoDao.Descricao, Numero = item.ParcelaDao.Numero, ValorPago = item.ValorPago, CV = item.CV }); }
            viewer.LocalReport.DataSources.Add(new ReportDataSource("ds_tipo_pagamento", pedidoTipoPagamentoDao));

            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

            return bytes;
        }
    }
}

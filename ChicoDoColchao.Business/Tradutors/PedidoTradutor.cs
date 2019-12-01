using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class PedidoTradutor
    {
        public static Pedido ToBd(this PedidoDao pedidoDao)
        {
            var pedido = new Pedido();

            pedido.PedidoID = pedidoDao.PedidoID;

            var clienteDao = pedidoDao.ClienteDao.FirstOrDefault();
            if (clienteDao != null && clienteDao.ClienteID > 0)
            {
                pedido.ClienteID = clienteDao.ClienteID;
            }

            pedido.DataPedido = pedidoDao.DataPedido;
            pedido.DataCancelamento = pedidoDao.DataCancelamento;

            var consultorDao = pedidoDao.ConsultorDao.FirstOrDefault();
            if (consultorDao != null && consultorDao.FuncionarioID > 0)
            {
                pedido.FuncionarioID = consultorDao.FuncionarioID;
            }

            var lojaDao = pedidoDao.LojaDao.FirstOrDefault();
            if (lojaDao != null && lojaDao.LojaID > 0)
            {
                pedido.LojaID = lojaDao.LojaID;
            }

            var lojaSaidaDao = pedidoDao.LojaSaidaDao.FirstOrDefault();
            if (lojaSaidaDao != null && lojaSaidaDao.LojaID > 0)
                pedido.LojaSaidaID = lojaSaidaDao.LojaID;

            if (pedidoDao.UsuarioPedidoDao != null && pedidoDao.UsuarioPedidoDao.UsuarioID > 0)
                pedido.UsuarioPedidoID = pedidoDao.UsuarioPedidoDao.UsuarioID;

            if (pedidoDao.UsuarioCancelamentoDao != null && pedidoDao.UsuarioCancelamentoDao.UsuarioID > 0)
                pedido.UsuarioCancelamentoID = pedidoDao.UsuarioCancelamentoDao.UsuarioID;

            pedido.NomeCarreto = pedidoDao.NomeCarreto;
            pedido.ValorFrete = pedidoDao.ValorFrete;
            pedido.TipoPagamentoFreteID = pedidoDao.TipoPagamentoFreteID;
            pedido.Observacao = pedidoDao.Observacao;
            pedido.Desconto = pedidoDao.Desconto;
            pedido.PedidoTrocaID = pedidoDao.PedidoTrocaID;

            var pedidoStatusDao = pedidoDao.PedidoStatusDao.FirstOrDefault();
            if (pedidoStatusDao != null && pedidoStatusDao.PedidoStatusID > 0)
                pedido.PedidoStatusID = pedidoStatusDao.PedidoStatusID;

            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao)
            {
                var pedidoProduto = new PedidoProduto();

                pedidoProduto.PedidoID = pedidoProdutoDao.PedidoID;
                pedidoProduto.ProdutoID = pedidoProdutoDao.ProdutoID;
                pedidoProduto.Quantidade = pedidoProdutoDao.Quantidade;
                pedidoProduto.Medida = pedidoProdutoDao.Medida;
                pedidoProduto.Preco = pedidoProdutoDao.Preco;

                pedidoProduto.DataEntrega = pedidoProdutoDao.DataEntrega;
                if (pedidoProdutoDao.UsuarioEntregaDao != null && pedidoProdutoDao.UsuarioEntregaDao.UsuarioID > 0)
                    pedidoProduto.UsuarioEntregaID = pedidoProdutoDao.UsuarioEntregaDao.UsuarioID;

                pedidoProduto.DataBaixa = pedidoProdutoDao.DataBaixa;
                if (pedidoProdutoDao.UsuarioBaixaDao != null && pedidoProdutoDao.UsuarioBaixaDao.UsuarioID > 0)
                    pedidoProduto.UsuarioBaixaID = pedidoProdutoDao.UsuarioBaixaDao.UsuarioID;

                pedidoProduto.DataTroca = pedidoProdutoDao.DataTroca;
                if (pedidoProdutoDao.UsuarioTrocaDao != null && pedidoProdutoDao.UsuarioTrocaDao.UsuarioID > 0)
                    pedidoProduto.UsuarioTrocaID = pedidoProdutoDao.UsuarioTrocaDao.UsuarioID;

                pedido.PedidoProduto.Add(pedidoProduto);
            }

            foreach (var pedidoTipoPagamentoDao in pedidoDao.PedidoTipoPagamentoDao)
            {
                var pedidoTipoPagamento = new PedidoTipoPagamento();

                pedidoTipoPagamento.PedidoID = pedidoTipoPagamentoDao.PedidoID;
                pedidoTipoPagamento.TipoPagamentoID = pedidoTipoPagamentoDao.TipoPagamentoDao.TipoPagamentoID;
                pedidoTipoPagamento.ParcelaID = pedidoTipoPagamentoDao.ParcelaDao.ParcelaID;
                pedidoTipoPagamento.ValorPago = pedidoTipoPagamentoDao.ValorPago;
                pedidoTipoPagamento.CV = pedidoTipoPagamentoDao.CV;

                pedido.PedidoTipoPagamento.Add(pedidoTipoPagamento);
            }

            return pedido;
        }

        public static PedidoDao ToApp(this Pedido pedido)
        {
            var pedidoDao = new PedidoDao();

            pedidoDao.PedidoID = pedido.PedidoID;
            pedidoDao.ClienteDao.Add(new ClienteDao()
            {
                ClienteID = pedido.Cliente.ClienteID,
                Nome = pedido.Cliente.Nome,
                Email = pedido.Cliente.Email,
                DataNascimento = pedido.Cliente.DataNascimento,
                Cpf = string.IsNullOrEmpty(pedido.Cliente.Cpf) ? string.Empty : Convert.ToUInt64(pedido.Cliente.Cpf).ToString(@"000\.000\.000\-00"),
                Cnpj = string.IsNullOrEmpty(pedido.Cliente.Cnpj) ? string.Empty : Convert.ToUInt64(pedido.Cliente.Cnpj).ToString(@"00\.000\.000\/0000\-00"),
                NomeFantasia = pedido.Cliente.NomeFantasia,
                RazaoSocial = pedido.Cliente.RazaoSocial,
                TelefoneResidencial = string.IsNullOrEmpty(pedido.Cliente.TelefoneResidencial) ? string.Empty : pedido.Cliente.TelefoneResidencial.Length > 10 ? Convert.ToInt64(pedido.Cliente.TelefoneResidencial).ToString("(##) #####-####") : Convert.ToInt64(pedido.Cliente.TelefoneResidencial).ToString("(##) ####-####"),
                TelefoneResidencial2 = string.IsNullOrEmpty(pedido.Cliente.TelefoneResidencial2) ? string.Empty : pedido.Cliente.TelefoneResidencial2.Length > 10 ? Convert.ToInt64(pedido.Cliente.TelefoneResidencial2).ToString("(##) #####-####") : Convert.ToInt64(pedido.Cliente.TelefoneResidencial2).ToString("(##) ####-####"),
                TelefoneCelular = string.IsNullOrEmpty(pedido.Cliente.TelefoneCelular) ? string.Empty : pedido.Cliente.TelefoneCelular.Length > 10 ? Convert.ToInt64(pedido.Cliente.TelefoneCelular).ToString("(##) #####-####") : Convert.ToInt64(pedido.Cliente.TelefoneCelular).ToString("(##) ####-####"),
                TelefoneCelular2 = string.IsNullOrEmpty(pedido.Cliente.TelefoneCelular2) ? string.Empty : pedido.Cliente.TelefoneCelular2.Length > 10 ? Convert.ToInt64(pedido.Cliente.TelefoneCelular2).ToString("(##) #####-####") : Convert.ToInt64(pedido.Cliente.TelefoneCelular2).ToString("(##) ####-####"),
                EstadoDao = { new EstadoDao() { EstadoID = pedido.Cliente.Estado.EstadoID, Nome = pedido.Cliente.Estado.Nome, Sigla = pedido.Cliente.Estado.Sigla } },
                Cidade = pedido.Cliente.Cidade,
                Logradouro = pedido.Cliente.Logradouro,
                Numero = pedido.Cliente.Numero,
                PontoReferencia = pedido.Cliente.PontoReferencia,
                Complemento = pedido.Cliente.Complemento,
                Bairro = pedido.Cliente.Bairro,
                Cep = pedido.Cliente.Cep
            });
            pedidoDao.DataPedido = pedido.DataPedido;
            pedidoDao.DataCancelamento = pedido.DataCancelamento;
            pedidoDao.ValorFrete = pedido.ValorFrete;
            pedidoDao.TipoPagamentoFreteID = pedido.TipoPagamentoFreteID;
            pedidoDao.Desconto = pedido.Desconto;
            pedidoDao.PedidoTrocaID = pedido.PedidoTrocaID;

            if (pedido.UsuarioPedido != null)
                pedidoDao.UsuarioPedidoDao = new UsuarioDao() { UsuarioID = pedido.UsuarioPedido.UsuarioID, Login = pedido.UsuarioPedido.Login };

            if (pedido.UsuarioCancelamento != null)
                pedidoDao.UsuarioCancelamentoDao = new UsuarioDao() { UsuarioID = pedido.UsuarioCancelamento.UsuarioID, Login = pedido.UsuarioCancelamento.Login };

            pedidoDao.ConsultorDao.Add(new ConsultorDao()
            {
                FuncionarioID = pedido.Funcionario.FuncionarioID,
                Numero = pedido.Funcionario.Numero,
                Nome = pedido.Funcionario.Nome,
                Email = pedido.Funcionario.Email,
                Telefone = string.IsNullOrEmpty(pedido.Funcionario.Telefone) ? string.Empty : pedido.Funcionario.Telefone.Length > 10 ? Convert.ToInt64(pedido.Funcionario.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.Funcionario.Telefone).ToString("(##) ####-####")
            });

            pedidoDao.LojaDao.Add(new LojaDao()
            {
                LojaID = pedido.LojaOrigem.LojaID,
                Cnpj = string.IsNullOrEmpty(pedido.LojaOrigem.Cnpj) ? string.Empty : Convert.ToUInt64(pedido.LojaOrigem.Cnpj).ToString(@"00\.000\.000\/0000\-00"),
                NomeFantasia = pedido.LojaOrigem.NomeFantasia,
                RazaoSocial = pedido.LojaOrigem.RazaoSocial,
                Telefone = string.IsNullOrEmpty(pedido.LojaOrigem.Telefone) ? string.Empty : pedido.LojaOrigem.Telefone.Length > 10 ? Convert.ToInt64(pedido.LojaOrigem.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.LojaOrigem.Telefone).ToString("(##) ####-####"),
                Bairro = string.IsNullOrEmpty(pedido.LojaOrigem.Bairro) ? string.Empty : pedido.LojaOrigem.Bairro,
                Logradouro = string.IsNullOrEmpty(pedido.LojaOrigem.Logradouro) ? string.Empty : pedido.LojaOrigem.Logradouro,
                Numero = pedido.LojaOrigem.Numero,
                Complemento = string.IsNullOrEmpty(pedido.LojaOrigem.Complemento) ? string.Empty : pedido.LojaOrigem.Complemento,
                Cep = string.IsNullOrEmpty(pedido.LojaOrigem.Cep) ? string.Empty : pedido.LojaOrigem.Cep
            });

            pedidoDao.LojaSaidaDao.Add(new LojaDao()
            {
                LojaID = pedido.LojaSaida.LojaID,
                Cnpj = string.IsNullOrEmpty(pedido.LojaSaida.Cnpj) ? string.Empty : Convert.ToUInt64(pedido.LojaSaida.Cnpj).ToString(@"00\.000\.000\/0000\-00"),
                NomeFantasia = pedido.LojaSaida.NomeFantasia,
                RazaoSocial = pedido.LojaSaida.RazaoSocial,
                Telefone = string.IsNullOrEmpty(pedido.LojaSaida.Telefone) ? string.Empty : pedido.LojaSaida.Telefone.Length > 10 ? Convert.ToInt64(pedido.LojaSaida.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.LojaSaida.Telefone).ToString("(##) ####-####"),
                Bairro = string.IsNullOrEmpty(pedido.LojaSaida.Bairro) ? string.Empty : pedido.LojaOrigem.Bairro,
                Logradouro = string.IsNullOrEmpty(pedido.LojaSaida.Logradouro) ? string.Empty : pedido.LojaOrigem.Logradouro,
                Numero = pedido.LojaSaida.Numero,
                Complemento = string.IsNullOrEmpty(pedido.LojaSaida.Complemento) ? string.Empty : pedido.LojaOrigem.Complemento,
                Cep = string.IsNullOrEmpty(pedido.LojaSaida.Cep) ? string.Empty : pedido.LojaOrigem.Cep
            });

            pedidoDao.NomeCarreto = pedido.NomeCarreto;
            pedidoDao.Observacao = pedido.Observacao;

            pedidoDao.PedidoStatusDao.Add(new PedidoStatusDao()
            {
                PedidoStatusID = pedido.PedidoStatus.PedidoStatusID,
                Descricao = pedido.PedidoStatus.Descricao
            });

            foreach (var pedidoProduto in pedido.PedidoProduto)
            {
                var pedidoProdutoDao = new PedidoProdutoDao();

                pedidoProdutoDao.PedidoID = pedidoProduto.PedidoID;
                pedidoProdutoDao.ProdutoID = pedidoProduto.ProdutoID;
                pedidoProdutoDao.Quantidade = pedidoProduto.Quantidade;
                pedidoProdutoDao.Medida = pedidoProduto.Medida;
                pedidoProdutoDao.DataEntrega = pedidoProduto.DataEntrega;
                pedidoProdutoDao.DataBaixa = pedidoProduto.DataBaixa;
                pedidoProdutoDao.DataTroca = pedidoProduto.DataTroca;
                pedidoProdutoDao.Preco = pedidoProduto.Preco;

                if (pedidoProduto.UsuarioEntrega != null)
                    pedidoProdutoDao.UsuarioEntregaDao = new UsuarioDao() { UsuarioID = pedidoProduto.UsuarioEntrega.UsuarioID, Login = pedidoProduto.UsuarioEntrega.Login };

                if (pedidoProduto.UsuarioBaixa != null)
                    pedidoProdutoDao.UsuarioBaixaDao = new UsuarioDao() { UsuarioID = pedidoProduto.UsuarioBaixa.UsuarioID, Login = pedidoProduto.UsuarioBaixa.Login };

                if (pedidoProduto.UsuarioTroca != null)
                    pedidoProdutoDao.UsuarioTrocaDao = new UsuarioDao() { UsuarioID = pedidoProduto.UsuarioTroca.UsuarioID, Login = pedidoProduto.UsuarioTroca.Login };

                pedidoProdutoDao.ProdutoDao = new ProdutoDao()
                {
                    ProdutoID = pedidoProduto.ProdutoID,
                    Descricao = pedidoProduto.Produto.Descricao,
                    Numero = pedidoProduto.Produto.Numero,
                    MedidaDao = new MedidaDao()
                    {
                        MedidaID = pedidoProduto.Produto.Medida.MedidaID,
                        Descricao = string.IsNullOrEmpty(pedidoProduto.Medida) ? pedidoProduto.Produto.Medida.Descricao : pedidoProduto.Medida
                    },
                    CategoriaDao = new List<CategoriaDao>()
                    {
                        new CategoriaDao() { CategoriaID = pedidoProduto.Produto.Categoria.CategoriaID, Descricao = pedidoProduto.Produto.Categoria.Descricao }
                    },
                    Preco = pedidoProduto.Produto.Preco
                };

                pedidoDao.PedidoProdutoDao.Add(pedidoProdutoDao);
            }

            foreach (var pedidoTipoPagamento in pedido.PedidoTipoPagamento)
            {
                var pedidoTipoPagamentoDao = new PedidoTipoPagamentoDao();

                pedidoTipoPagamentoDao.PedidoID = pedidoTipoPagamento.PedidoID;
                pedidoTipoPagamentoDao.TipoPagamentoDao = new TipoPagamentoDao()
                {
                    TipoPagamentoID = pedidoTipoPagamento.TipoPagamento.TipoPagamentoID,
                    Descricao = pedidoTipoPagamento.TipoPagamento.Descricao
                };
                pedidoTipoPagamentoDao.ParcelaDao = new ParcelaDao()
                {
                    ParcelaID = pedidoTipoPagamento.Parcela.ParcelaID,
                    Numero = pedidoTipoPagamento.Parcela.Numero
                };
                pedidoTipoPagamentoDao.ValorPago = pedidoTipoPagamento.ValorPago;
                pedidoTipoPagamentoDao.CV = pedidoTipoPagamento.CV;

                pedidoDao.PedidoTipoPagamentoDao.Add(pedidoTipoPagamentoDao);
            }

            return pedidoDao;
        }
    }
}

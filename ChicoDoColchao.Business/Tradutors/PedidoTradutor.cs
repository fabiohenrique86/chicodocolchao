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
            Pedido pedido = new Pedido();

            pedido.PedidoID = pedidoDao.PedidoID;

            if (pedidoDao.ClienteDao.FirstOrDefault() != null)
            {
                pedido.Cliente = new Cliente() { ClienteID = pedidoDao.ClienteDao.FirstOrDefault().ClienteID };
                pedido.ClienteID = pedidoDao.ClienteDao.FirstOrDefault().ClienteID;
            }

            pedido.DataEntrega = pedidoDao.DataEntrega;
            pedido.DataPedido = pedidoDao.DataPedido;

            if (pedidoDao.FuncionarioDao.FirstOrDefault() != null)
            {
                pedido.Funcionario = new Funcionario() { FuncionarioID = pedidoDao.FuncionarioDao.FirstOrDefault().FuncionarioID };
                pedido.FuncionarioID = pedidoDao.FuncionarioDao.FirstOrDefault().FuncionarioID;
            }

            if (pedidoDao.LojaDao.FirstOrDefault() != null)
            {
                pedido.Loja1 = new Loja() { LojaID = pedidoDao.LojaDao.FirstOrDefault().LojaID };
                pedido.LojaID = pedidoDao.LojaDao.FirstOrDefault().LojaID;
            }

            if (pedidoDao.LojaSaidaDao.FirstOrDefault() != null)
            {
                pedido.Loja = new Loja() { LojaID = pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID };
                pedido.LojaSaidaID = pedidoDao.LojaSaidaDao.FirstOrDefault().LojaID;
            }

            pedido.NomeCarreto = pedidoDao.NomeCarreto;
            pedido.ValorFrete = pedidoDao.ValorFrete;
            pedido.Observacao = pedidoDao.Observacao;
            pedido.Desconto = pedidoDao.Desconto;

            if (pedidoDao.PedidoStatusDao.FirstOrDefault() != null)
            {
                pedido.PedidoStatus = new PedidoStatus() { PedidoStatusID = pedidoDao.PedidoStatusDao.FirstOrDefault().PedidoStatusID };
                pedido.PedidoStatusID = pedidoDao.PedidoStatusDao.FirstOrDefault().PedidoStatusID;
            }

            foreach (var pedidoProdutoDao in pedidoDao.PedidoProdutoDao)
            {
                PedidoProduto pedidoProduto = new PedidoProduto();

                pedidoProduto.PedidoID = pedidoProdutoDao.PedidoID;
                pedidoProduto.ProdutoID = pedidoProdutoDao.ProdutoID;
                pedidoProduto.Quantidade = pedidoProdutoDao.Quantidade;
                pedidoProduto.Medida = pedidoProdutoDao.Medida;

                pedido.PedidoProduto.Add(pedidoProduto);
            }

            foreach (var pedidoTipoPagamentoDao in pedidoDao.PedidoTipoPagamentoDao)
            {
                PedidoTipoPagamento pedidoTipoPagamento = new PedidoTipoPagamento();

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
            PedidoDao pedidoDao = new PedidoDao();

            pedidoDao.PedidoID = pedido.PedidoID;
            pedidoDao.ClienteDao.Add(new ClienteDao()
            {
                ClienteID = pedido.Cliente.ClienteID,
                Nome = pedido.Cliente.Nome,
                Email = pedido.Cliente.Email,
                DataNascimento = pedido.Cliente.DataNascimento,
                Cpf = string.IsNullOrEmpty(pedido.Cliente.Cpf) ? string.Empty : Convert.ToInt64(pedido.Cliente.Cpf).ToString(@"###\.###\.###\-##"),
                Cnpj = string.IsNullOrEmpty(pedido.Cliente.Cnpj) ? string.Empty : Convert.ToInt64(pedido.Cliente.Cnpj).ToString(@"##\.###\.###\/####\-##"),                
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
                Bairro = pedido.Cliente.Bairro
            });
            pedidoDao.DataEntrega = pedido.DataEntrega;
            pedidoDao.DataPedido = pedido.DataPedido;
            pedidoDao.ValorFrete = pedido.ValorFrete;
            pedidoDao.Desconto = pedido.Desconto;
            pedidoDao.FuncionarioDao.Add(new FuncionarioDao()
            {
                FuncionarioID = pedido.Funcionario.FuncionarioID,
                Numero = pedido.Funcionario.Numero,
                Nome = pedido.Funcionario.Nome,
                Email = pedido.Funcionario.Email,
                Telefone = string.IsNullOrEmpty(pedido.Funcionario.Telefone) ? string.Empty : pedido.Funcionario.Telefone.Length > 10 ? Convert.ToInt64(pedido.Funcionario.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.Funcionario.Telefone).ToString("(##) ####-####"),
            });
            pedidoDao.LojaDao.Add(new LojaDao()
            {
                LojaID = pedido.Loja1.LojaID,
                Cnpj = string.IsNullOrEmpty(pedido.Loja1.Cnpj) ? string.Empty : Convert.ToInt64(pedido.Loja1.Cnpj).ToString(@"##\.###\.###\/####\-##"),
                NomeFantasia = pedido.Loja1.NomeFantasia,
                RazaoSocial = pedido.Loja1.RazaoSocial,
                Telefone = string.IsNullOrEmpty(pedido.Loja1.Telefone) ? string.Empty : pedido.Loja1.Telefone.Length > 10 ? Convert.ToInt64(pedido.Loja1.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.Loja1.Telefone).ToString("(##) ####-####"),
            });
            pedidoDao.LojaSaidaDao.Add(new LojaDao()
            {
                LojaID = pedido.Loja.LojaID,
                Cnpj = string.IsNullOrEmpty(pedido.Loja.Cnpj) ? string.Empty : Convert.ToInt64(pedido.Loja.Cnpj).ToString(@"##\.###\.###\/####\-##"),
                NomeFantasia = pedido.Loja.NomeFantasia,
                RazaoSocial = pedido.Loja.RazaoSocial,
                Telefone = string.IsNullOrEmpty(pedido.Loja.Telefone) ? string.Empty : pedido.Loja.Telefone.Length > 10 ? Convert.ToInt64(pedido.Loja.Telefone).ToString("(##) #####-####") : Convert.ToInt64(pedido.Loja.Telefone).ToString("(##) ####-####"),
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
                PedidoProdutoDao pedidoProdutoDao = new PedidoProdutoDao();

                pedidoProdutoDao.PedidoID = pedidoProduto.PedidoID;
                pedidoProdutoDao.ProdutoID = pedidoProduto.ProdutoID;
                pedidoProdutoDao.Quantidade = pedidoProduto.Quantidade;
                pedidoProdutoDao.Medida = pedidoProduto.Medida;

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
                    }
                };

                pedidoDao.PedidoProdutoDao.Add(pedidoProdutoDao);
            }

            foreach (var pedidoTipoPagamento in pedido.PedidoTipoPagamento)
            {
                PedidoTipoPagamentoDao pedidoTipoPagamentoDao = new PedidoTipoPagamentoDao();

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

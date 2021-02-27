using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChicoDoColchao.Business.Tradutors
{
    public static class OrcamentoTradutor
    {
        public static Orcamento ToBd(this OrcamentoDao orcamentoDao)
        {
            var orcamento = new Orcamento();

            orcamento.OrcamentoID = orcamentoDao.OrcamentoID;

            if (orcamentoDao.LojaDao.FirstOrDefault() != null)
            {
                orcamento.LojaID = orcamentoDao.LojaDao.FirstOrDefault().LojaID;
            }

            if (orcamentoDao.ConsultorDao.FirstOrDefault() != null)
            {
                orcamento.FuncionarioID = orcamentoDao.ConsultorDao.FirstOrDefault().FuncionarioID;
            }

            orcamento.DataOrcamento = orcamentoDao.DataOrcamento;
            orcamento.Observacao = orcamentoDao.Observacao;
            orcamento.Ativo = orcamentoDao.Ativo;
            orcamento.Desconto = orcamentoDao.Desconto;
            orcamento.NomeCliente = orcamentoDao.NomeCliente;
            orcamento.TelefoneCliente = orcamentoDao.TelefoneCliente;

            if (orcamentoDao.PedidoDao != null)
            {
                orcamento.PedidoID = orcamentoDao.PedidoDao.PedidoID;
            }

            foreach (var orcamentoProdutoDao in orcamentoDao.OrcamentoProdutoDao)
            {
                var orcamentoProduto = new OrcamentoProduto();

                orcamentoProduto.OrcamentoProdutoID = orcamentoProdutoDao.OrcamentoProdutoID;
                orcamentoProduto.OrcamentoID = orcamentoProdutoDao.OrcamentoID;
                orcamentoProduto.ProdutoID = orcamentoProdutoDao.ProdutoID;
                orcamentoProduto.Quantidade = orcamentoProdutoDao.Quantidade;
                orcamentoProduto.Medida = orcamentoProdutoDao.Medida;
                orcamentoProduto.Preco = orcamentoProdutoDao.Preco;

                orcamento.OrcamentoProduto.Add(orcamentoProduto);
            }

            foreach (var orcamentoHistoricoDao in orcamentoDao.OrcamentoHistoricoDao)
            {
                var orcamentoHistorico = new OrcamentoHistorico();

                orcamentoHistorico.OrcamentoHistoricoID = orcamentoHistoricoDao.OrcamentoHistoricoID;
                orcamentoHistorico.OrcamentoID = orcamentoHistoricoDao.OrcamentoID;
                orcamentoHistorico.Observacao = orcamentoHistoricoDao.Observacao;
                orcamentoHistorico.DataCadastro = orcamentoHistoricoDao.DataCadastro;

                orcamento.OrcamentoHistorico.Add(orcamentoHistorico);
            }

            return orcamento;
        }

        public static OrcamentoDao ToApp(this Orcamento orcamento)
        {
            var orcamentoDao = new OrcamentoDao();

            orcamentoDao.OrcamentoID = orcamento.OrcamentoID;
            orcamentoDao.DataOrcamento = orcamento.DataOrcamento;
            orcamentoDao.Observacao = orcamento.Observacao;
            orcamentoDao.Ativo = orcamento.Ativo;
            orcamentoDao.Desconto = orcamento.Desconto;

            if (orcamento.Pedido != null)
            {
                orcamentoDao.PedidoDao = new PedidoDao() { PedidoID = orcamento.Pedido.PedidoID };
            }

            orcamentoDao.NomeCliente = orcamento.NomeCliente;
            orcamentoDao.TelefoneCliente = orcamento.TelefoneCliente;

            //orcamentoDao.ClienteDao = new ClienteDao()
            //{
            //    ClienteID = orcamento.Cliente.ClienteID,
            //    Nome = orcamento.Cliente.Nome,
            //    Email = orcamento.Cliente.Email,
            //    DataNascimento = orcamento.Cliente.DataNascimento,
            //    Cpf = string.IsNullOrEmpty(orcamento.Cliente.Cpf) ? string.Empty : Convert.ToUInt64(orcamento.Cliente.Cpf).ToString(@"000\.000\.000\-00"),
            //    Cnpj = string.IsNullOrEmpty(orcamento.Cliente.Cnpj) ? string.Empty : Convert.ToUInt64(orcamento.Cliente.Cnpj).ToString(@"00\.000\.000\/0000\-00"),
            //    NomeFantasia = orcamento.Cliente.NomeFantasia,
            //    RazaoSocial = orcamento.Cliente.RazaoSocial,
            //    TelefoneResidencial = string.IsNullOrEmpty(orcamento.Cliente.TelefoneResidencial) ? string.Empty : orcamento.Cliente.TelefoneResidencial.Length > 10 ? Convert.ToInt64(orcamento.Cliente.TelefoneResidencial).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Cliente.TelefoneResidencial).ToString("(##) ####-####"),
            //    TelefoneResidencial2 = string.IsNullOrEmpty(orcamento.Cliente.TelefoneResidencial2) ? string.Empty : orcamento.Cliente.TelefoneResidencial2.Length > 10 ? Convert.ToInt64(orcamento.Cliente.TelefoneResidencial2).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Cliente.TelefoneResidencial2).ToString("(##) ####-####"),
            //    TelefoneCelular = string.IsNullOrEmpty(orcamento.Cliente.TelefoneCelular) ? string.Empty : orcamento.Cliente.TelefoneCelular.Length > 10 ? Convert.ToInt64(orcamento.Cliente.TelefoneCelular).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Cliente.TelefoneCelular).ToString("(##) ####-####"),
            //    TelefoneCelular2 = string.IsNullOrEmpty(orcamento.Cliente.TelefoneCelular2) ? string.Empty : orcamento.Cliente.TelefoneCelular2.Length > 10 ? Convert.ToInt64(orcamento.Cliente.TelefoneCelular2).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Cliente.TelefoneCelular2).ToString("(##) ####-####"),
            //    EstadoDao = { new EstadoDao() { EstadoID = orcamento.Cliente.Estado.EstadoID, Nome = orcamento.Cliente.Estado.Nome, Sigla = orcamento.Cliente.Estado.Sigla } },
            //    Cidade = orcamento.Cliente.Cidade,
            //    Logradouro = orcamento.Cliente.Logradouro,
            //    Numero = orcamento.Cliente.Numero,
            //    PontoReferencia = orcamento.Cliente.PontoReferencia,
            //    Complemento = orcamento.Cliente.Complemento,
            //    Bairro = orcamento.Cliente.Bairro,
            //    Cep = orcamento.Cliente.Cep
            //};

            orcamentoDao.ConsultorDao.Add(new ConsultorDao()
            {
                FuncionarioID = orcamento.Funcionario.FuncionarioID,
                Numero = orcamento.Funcionario.Numero,
                Nome = orcamento.Funcionario.Nome,
                Email = orcamento.Funcionario.Email,
                Telefone = string.IsNullOrEmpty(orcamento.Funcionario.Telefone) ? string.Empty : orcamento.Funcionario.Telefone.Length > 10 ? Convert.ToInt64(orcamento.Funcionario.Telefone).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Funcionario.Telefone).ToString("(##) ####-####"),
            });

            orcamentoDao.LojaDao.Add(new LojaDao()
            {
                LojaID = orcamento.Loja.LojaID,
                Cnpj = string.IsNullOrEmpty(orcamento.Loja.Cnpj) ? string.Empty : Convert.ToUInt64(orcamento.Loja.Cnpj).ToString(@"00\.000\.000\/0000\-00"),
                NomeFantasia = orcamento.Loja.NomeFantasia,
                RazaoSocial = orcamento.Loja.RazaoSocial,
                Telefone = string.IsNullOrEmpty(orcamento.Loja.Telefone) ? string.Empty : orcamento.Loja.Telefone.Length > 10 ? Convert.ToInt64(orcamento.Loja.Telefone).ToString("(##) #####-####") : Convert.ToInt64(orcamento.Loja.Telefone).ToString("(##) ####-####"),
            });

            foreach (var orcamentoProduto in orcamento.OrcamentoProduto)
            {
                var orcamentoProdutoDao = new OrcamentoProdutoDao();

                orcamentoProdutoDao.OrcamentoProdutoID = orcamentoProduto.OrcamentoProdutoID;
                orcamentoProdutoDao.OrcamentoID = orcamentoProduto.OrcamentoID;
                orcamentoProdutoDao.ProdutoID = orcamentoProduto.ProdutoID;
                orcamentoProdutoDao.Quantidade = orcamentoProduto.Quantidade;
                orcamentoProdutoDao.Medida = orcamentoProduto.Medida;
                orcamentoProdutoDao.Preco = orcamentoProduto.Preco;

                orcamentoProdutoDao.ProdutoDao = new ProdutoDao()
                {
                    ProdutoID = orcamentoProduto.ProdutoID,
                    Descricao = orcamentoProduto.Produto.Descricao,
                    Numero = orcamentoProduto.Produto.Numero,
                    MedidaDao = new MedidaDao()
                    {
                        MedidaID = orcamentoProduto.Produto.Medida.MedidaID,
                        Descricao = string.IsNullOrEmpty(orcamentoProduto.Medida) ? orcamentoProduto.Produto.Medida.Descricao : orcamentoProduto.Medida
                    },
                    CategoriaDao = new List<CategoriaDao>()
                    {
                        new CategoriaDao() { CategoriaID = orcamentoProduto.Produto.Categoria.CategoriaID, Descricao = orcamentoProduto.Produto.Categoria.Descricao }
                    },
                    Preco = orcamentoProduto.Produto.Preco
                };

                orcamentoDao.OrcamentoProdutoDao.Add(orcamentoProdutoDao);
            }

            foreach (var orcamentoHistorico in orcamento.OrcamentoHistorico)
            {
                var orcamentoHistoricoDao = new OrcamentoHistoricoDao();

                orcamentoHistoricoDao.OrcamentoHistoricoID = orcamentoHistorico.OrcamentoHistoricoID;
                orcamentoHistoricoDao.OrcamentoID = orcamentoHistorico.OrcamentoID;
                orcamentoHistoricoDao.Observacao = orcamentoHistorico.Observacao;
                orcamentoHistoricoDao.DataCadastro = orcamentoHistorico.DataCadastro;

                orcamentoDao.OrcamentoHistoricoDao.Add(orcamentoHistoricoDao);
            }

            return orcamentoDao;
        }
    }
}

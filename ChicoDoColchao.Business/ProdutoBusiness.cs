using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace ChicoDoColchao.Business
{
    public class ProdutoBusiness
    {
        ProdutoRepository produtoRepository;
        MedidaRepository medidaRepository;
        LojaRepository lojaRepository;
        MedidaBusiness medidaBusiness;
        CategoriaRepository categoriaRepository;
        CategoriaBusiness categoriaBusiness;
        LojaProdutoBusiness lojaProdutoBusiness;
        LogRepository logRepository;

        public ProdutoBusiness()
        {
            produtoRepository = new ProdutoRepository();
            medidaRepository = new MedidaRepository();
            medidaBusiness = new MedidaBusiness();
            lojaRepository = new LojaRepository();
            categoriaBusiness = new CategoriaBusiness();
            categoriaRepository = new CategoriaRepository();
            lojaProdutoBusiness = new LojaProdutoBusiness();
            logRepository = new LogRepository();
        }

        private void ValidarIncluir(ProdutoDao produtoDao)
        {
            if (produtoDao == null)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            if (produtoDao.Numero == null || produtoDao.Numero <= 0)
            {
                throw new BusinessException("Número é obrigatório");
            }

            if (string.IsNullOrEmpty(produtoDao.Descricao))
            {
                throw new BusinessException("Descrição é obrigatório");
            }

            if (produtoDao.CategoriaDao.FirstOrDefault() == null || produtoDao.CategoriaDao.FirstOrDefault().CategoriaID <= 0)
            {
                throw new BusinessException("Categoria é obrigatório");
            }

            if (produtoDao.MedidaDao == null || produtoDao.MedidaDao.MedidaID <= 0)
            {
                throw new BusinessException("Medida é obrigatório");
            }

            if (produtoDao.Preco <= 0)
            {
                throw new BusinessException("Preço é obrigatório");
            }

            var loja = produtoDao.LojaProdutoDao.FirstOrDefault();

            if (produtoRepository.Listar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault() }, loja == null ? 0 : loja.LojaID).FirstOrDefault() != null)
            {
                throw new BusinessException($"Produto {produtoDao.Numero.GetValueOrDefault()} já cadastrado");
            }
        }

        private void ValidarExcluir(ProdutoDao produtoDao)
        {
            if (produtoDao == null)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            if (produtoDao.ProdutoID <= 0)
            {
                throw new BusinessException("ProdutoID é obrigatório");
            }
        }

        private void ValidarAtualizar(ProdutoDao produtoDao)
        {
            if (produtoDao == null)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            if (produtoDao.Numero <= 0)
            {
                throw new BusinessException("Número é obrigatório");
            }

            if (produtoDao.LojaProdutoDao == null || produtoDao.LojaProdutoDao.Count() <= 0)
            {
                throw new BusinessException("Loja é obrigatório");
            }
            else
            {
                foreach (var lojaProdutoDao in produtoDao.LojaProdutoDao)
                {
                    if (lojaProdutoDao.Quantidade != 0)
                    {
                        if (produtoDao.LojaProdutoDao.Any(x => x.LojaID <= 0))
                        {
                            throw new BusinessException("Loja é obrigatório");
                        }

                        var produtoLoja = produtoRepository.Listar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault(), Ativo = true }, produtoDao.LojaProdutoDao.FirstOrDefault().LojaID, 0).FirstOrDefault();

                        if (produtoLoja == null)
                        {
                            throw new BusinessException("Produto não cadastrado na loja");
                        }
                    }
                }
            }
        }

        private void ValidarTransferir(int lojaOrigemId, int lojaDestinoId, List<ProdutoDao> produtosDao)
        {
            if (produtosDao == null)
            {
                throw new BusinessException("Produto é obrigatório");
            }

            if (lojaOrigemId <= 0)
            {
                throw new BusinessException("Loja de Origem é obrigatório");
            }

            if (lojaDestinoId <= 0)
            {
                throw new BusinessException("Loja de Destino é obrigatório");
            }

            if (lojaOrigemId == lojaDestinoId)
            {
                throw new BusinessException("Loja de Origem não pode ser igual a Loja de Destino");
            }

            foreach (var produtoDao in produtosDao)
            {
                if (produtoDao.ProdutoID <= 0)
                {
                    throw new BusinessException("ProdutoID é obrigatório");
                }

                if (produtoDao.Quantidade <= 0)
                {
                    throw new BusinessException("Quantidade é obrigatório");
                }

                var produtoLojaOrigem = produtoRepository.Listar(new Produto() { ProdutoID = produtoDao.ProdutoID, Ativo = true }, lojaOrigemId, 0).FirstOrDefault();

                if (produtoLojaOrigem == null)
                {
                    throw new BusinessException(string.Format("Produto {0} não cadastrado na Loja de Origem", produtoDao.Numero));
                }

                var produtoLojaDestino = produtoRepository.Listar(new Produto() { ProdutoID = produtoDao.ProdutoID, Ativo = true }, 0, lojaDestinoId).FirstOrDefault();

                if (produtoLojaDestino == null)
                {
                    throw new BusinessException(string.Format("Produto {0} não cadastrado na Loja de Destino", produtoDao.Numero));
                }
            }
        }

        private void ValidarImportar(System.IO.Stream arquivo)
        {
            if (arquivo == null || arquivo.Length <= 0)
            {
                throw new BusinessException("Arquivo obrigatório");
            }
        }

        public void Incluir(ProdutoDao produtoDao)
        {
            try
            {
                ValidarIncluir(produtoDao);

                produtoRepository.Incluir(produtoDao.ToBd());
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

        public void Atualizar(ProdutoDao produtoDao)
        {
            try
            {
                ValidarAtualizar(produtoDao);
                
                produtoRepository.Atualizar(produtoDao.ToBd());
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

        public void Excluir(ProdutoDao produtoDao)
        {
            try
            {
                ValidarExcluir(produtoDao);

                produtoRepository.Excluir(produtoDao.ToBd());
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

        public void Transferir(int lojaOrigemId, int lojaDestinoId, List<ProdutoDao> produtosDao)
        {
            try
            {
                ValidarTransferir(lojaOrigemId, lojaDestinoId, produtosDao);

                foreach (var produtoDao in produtosDao)
                {
                    produtoRepository.Transferir(lojaOrigemId, lojaDestinoId, produtoDao.ProdutoID, produtoDao.Quantidade);
                }
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

        public List<ProdutoDao> Listar(ProdutoDao produtoDao, int lojaOrigemId = 0, int lojaDestinoId = 0)
        {
            try
            {
                return produtoRepository.Listar(produtoDao.ToBd(), lojaOrigemId, lojaDestinoId).Select(x => x.ToApp()).ToList();
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

        public List<string> Importar(System.IO.Stream arquivo)
        {
            var retorno = new List<string>();

            try
            {
                ValidarImportar(arquivo);

                var produtosDao = LerXLSX(arquivo);

                if (produtosDao == null || produtosDao.Count() <= 0)
                {
                    retorno.Add("Planilha XLSX não possui produtos ou os produtos não estão na formatação correta");
                    retorno.Add("Coluna A = Número do Produto, B = Descrição do Produto, C = Categoria do Produto, D = Medida do Produto, E = Quantidade do Produto na Loja, F = Preço de Compra do Produto");
                    return retorno;
                }

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(10) }))
                {
                    // busca todos os produtos da planilha na base de dados
                    // caso exista, atualiza somente a quantidade
                    // caso não exista, cadastra na base de dados
                    foreach (var produtoDao in produtosDao)
                    {
                        //if (produtoDao.Numero.GetValueOrDefault() == 52843)
                        //{

                        //}

                        var produto = produtoRepository.Listar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault() }).FirstOrDefault();

                        // caso o produto não exista na base de dados
                        if (produto == null)
                        {
                            // busca se a medida da planilha já existe na base de dados
                            // caso não exista, cadastra-a na base de dados
                            // caso exista, substitui a medida da planilha pela da base de dados por conta do MedidaID
                            if (produtoDao.MedidaDao == null || (string.IsNullOrEmpty(produtoDao.MedidaDao.Descricao)))
                            {
                                retorno.Add(string.Format("Produto {0} não possui Medida associada", produtoDao.Numero));
                                continue;
                            }

                            var medidaDao = medidaRepository.Listar(produtoDao.MedidaDao.ToBd()).FirstOrDefault();
                            if (medidaDao == null)
                            {
                                int medidaId = medidaBusiness.Incluir(produtoDao.MedidaDao);
                                produtoDao.MedidaDao.MedidaID = medidaId;
                                produtoDao.MedidaDao.Ativo = true;
                            }
                            else
                            {
                                produtoDao.MedidaDao = medidaDao.ToApp();
                            }

                            // busca se a categoria da planilha já existe na base de dados
                            // caso não exista, cadastra-a na base de dados
                            // caso exista, substitui a linha da planilha pela da base de dados por conta do MedidaID
                            var categoria = produtoDao.CategoriaDao.FirstOrDefault();
                            if (categoria == null || (string.IsNullOrEmpty(produtoDao.CategoriaDao.FirstOrDefault().Descricao)))
                            {
                                retorno.Add(string.Format("Produto {0} não possui Categoria associada", produtoDao.Numero));
                                continue;
                            }

                            var categoriaDao = categoriaRepository.Listar(categoria.ToBd()).FirstOrDefault();
                            if (categoriaDao == null)
                            {
                                var categoriaId = categoriaBusiness.Incluir(produtoDao.CategoriaDao.FirstOrDefault());
                                produtoDao.CategoriaDao.FirstOrDefault().CategoriaID = categoriaId;
                                produtoDao.CategoriaDao.FirstOrDefault().Ativo = true;
                            }
                            else
                            {
                                produtoDao.CategoriaDao.Clear();
                                produtoDao.CategoriaDao.Add(categoriaDao.ToApp());
                            }

                            // cadastra o produto
                            produtoRepository.Incluir(produtoDao.ToBd());
                        }
                        else
                        {
                            // inclui/atualiza o produto na loja
                            var lojaProdutoDao = produtoDao.LojaProdutoDao.FirstOrDefault();
                            if (lojaProdutoDao != null)
                            {
                                var lojaProduto = lojaProdutoBusiness.Listar(new LojaProdutoDao() { LojaID = lojaProdutoDao.LojaID, ProdutoID = produto.ProdutoID }).FirstOrDefault();

                                if (lojaProduto == null)
                                {
                                    lojaProdutoBusiness.Incluir(new LojaProdutoDao() { LojaID = lojaProdutoDao.LojaID, ProdutoID = produto.ProdutoID, Quantidade = lojaProdutoDao.Quantidade, Ativo = true });
                                }
                                else
                                {
                                    lojaProdutoBusiness.Atualizar(new LojaProdutoDao() { LojaProdutoID = lojaProduto.LojaProdutoID, Quantidade = lojaProdutoDao.Quantidade, Ativo = true });
                                }

                                produtoRepository.Atualizar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault(), Preco = produtoDao.Preco });
                            }
                        }
                    }

                    // se não houveram erros, commit no banco de dados
                    if (retorno == null || retorno.Count() <= 0)
                    {
                        scope.Complete();
                    }
                }
            }
            catch (BusinessException ex)
            {
                retorno.Add(ex.Message);
            }
            catch (Exception ex)
            {
                
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }

            return retorno;
        }

        private List<ProdutoDao> LerXLSX(System.IO.Stream arquivo)
        {
            var produtosDao = new List<ProdutoDao>();
            var workbook = new XLWorkbook(arquivo);
            var worksheets = workbook.Worksheets;

            // cada aba da planilha
            foreach (var worksheet in worksheets)
            {
                ProdutoDao produtoDao = null;

                // busca a loja por NomeFantasia
                var loja = lojaRepository.Listar(new Loja() { NomeFantasia = worksheet.Name.Trim(), Ativo = true }).FirstOrDefault();

                // cada célula que tem valor
                foreach (var cellUsed in worksheet.Cells())
                {
                    switch (cellUsed.Address.ColumnLetter.ToUpper())
                    {
                        // início da linha, deve zerar o objeto para um novo produto
                        case "A":

                            produtoDao = new ProdutoDao();

                            // A = Número
                            long numero;
                            cellUsed.TryGetValue(out numero);
                            produtoDao.Numero = numero;

                            break;

                        case "B":

                            if (produtoDao.Numero.GetValueOrDefault() <= 0)
                            {
                                break;
                            }

                            // B = Descrição
                            string descricao;
                            cellUsed.TryGetValue(out descricao);
                            produtoDao.Descricao = descricao.Trim();

                            break;

                        case "C":

                            if (produtoDao.Numero.GetValueOrDefault() <= 0)
                            {
                                break;
                            }

                            // C = Categoria
                            string categoria;

                            cellUsed.TryGetValue(out categoria);
                            produtoDao.CategoriaDao.Clear();
                            produtoDao.CategoriaDao.Add(new CategoriaDao() { Descricao = categoria.Trim(), Ativo = true });

                            break;

                        case "D":

                            if (produtoDao.Numero.GetValueOrDefault() <= 0)
                            {
                                break;
                            }

                            // D = Medida
                            string medida;
                            cellUsed.TryGetValue(out medida);
                            produtoDao.MedidaDao = new MedidaDao() { Descricao = medida.Replace(" ", "").Trim(), Ativo = true };

                            break;

                        case "E":

                            if (produtoDao.Numero.GetValueOrDefault() <= 0)
                            {
                                break;
                            }

                            // E = Quantidade
                            short quantidade;
                            cellUsed.TryGetValue(out quantidade);
                            produtoDao.Quantidade = quantidade;

                            if (loja != null)
                            {
                                produtoDao.LojaProdutoDao.Clear();
                                produtoDao.LojaProdutoDao.Add(new LojaProdutoDao() { LojaID = loja.LojaID, Quantidade = quantidade, Ativo = true });
                            }

                            break;

                        // fim da linha, deve adicionar o produto a lista
                        case "F":

                            if (produtoDao.Numero.GetValueOrDefault() <= 0)
                            {
                                break;
                            }

                            // F = Preço de Compra
                            double preco = 0;
                            if (cellUsed.Value != null)
                            {
                                double.TryParse(cellUsed.Value.ToString(), System.Globalization.NumberStyles.Currency, new System.Globalization.CultureInfo("pt-BR"), out preco);
                                if (preco > 0)
                                {
                                    produtoDao.Preco = Math.Round(preco, 2);
                                }

                                if (produtoDao.Numero > 0)
                                {
                                    produtosDao.Add(produtoDao);
                                }
                            }

                            break;

                        default:

                            break;
                    }
                }
            }

            return produtosDao;
        }
    }
}

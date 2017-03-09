using System;
using System.Collections.Generic;
using System.Linq;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;
using ClosedXML.Excel;
using System.Transactions;

namespace ChicoDoColchao.Business
{
    public class ProdutoBusiness
    {
        ProdutoRepository produtoRepository;
        MedidaRepository medidaRepository;
        LojaRepository lojaRepository;
        MedidaBusiness medidaBusiness;
        LinhaRepository linhaRepository;
        LinhaBusiness linhaBusiness;
        LogRepository logRepository;

        public ProdutoBusiness()
        {
            produtoRepository = new ProdutoRepository();
            medidaRepository = new MedidaRepository();
            medidaBusiness = new MedidaBusiness();
            lojaRepository = new LojaRepository();
            linhaBusiness = new LinhaBusiness();
            linhaRepository = new LinhaRepository();
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

            if (produtoDao.LinhaDao.FirstOrDefault() == null || produtoDao.LinhaDao.FirstOrDefault().LinhaID <= 0)
            {
                throw new BusinessException("Linha é obrigatório");
            }

            if (produtoDao.MedidaDao == null || produtoDao.MedidaDao.MedidaID <= 0)
            {
                throw new BusinessException("Medida é obrigatório");
            }

            if (produtoDao.ParcelaProdutoDao == null || produtoDao.ParcelaProdutoDao.Count() <= 0 || produtoDao.ParcelaProdutoDao.Count(x => x.ParcelaID <= 0) > 0)
            {
                throw new BusinessException("Preço é obrigatório");
            }

            if (produtoRepository.Listar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault() }).FirstOrDefault() != null)
            {
                throw new BusinessException("Produto (Número) já cadastrado");
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

                var produtoLojaOrigem = produtoRepository.Listar(new Produto() { ProdutoID = produtoDao.ProdutoID }, lojaOrigemId, 0).FirstOrDefault();

                if (produtoLojaOrigem == null)
                {
                    throw new BusinessException(string.Format("Produto {0} não cadastrado na Loja de Origem", produtoDao.Numero));
                }

                var produtoLojaDestino = produtoRepository.Listar(new Produto() { ProdutoID = produtoDao.ProdutoID }, 0, lojaDestinoId).FirstOrDefault();

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
                // inclui o log do erro
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
                // inclui o log do erro
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
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }
        }

        public List<string> Importar(System.IO.Stream arquivo)
        {
            List<string> retorno = new List<string>();

            try
            {
                ValidarImportar(arquivo);

                var produtosDao = LerXLSX(arquivo);

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = TimeSpan.FromMinutes(5) }))
                {
                    // busca todos os produtos da planilha na base de dados
                    // caso exista, atualiza somente a quantidade
                    // caso não exista, cadastra na base de dados
                    foreach (var produtoDao in produtosDao)
                    {
                        var produto = produtoRepository.Listar(new Produto() { Numero = produtoDao.Numero.GetValueOrDefault(), Ativo = true }).FirstOrDefault();

                        // caso o produto não exista na base de dados
                        if (produto == null)
                        {
                            // busca se a medida da planilha já existe na base de dados
                            // caso não exista, cadastra-a na base de dados
                            // caso exista, substitui a medida da planilha pela da base de dados por conta do MedidaID
                            if (produtoDao.MedidaDao == null)
                            {
                                retorno.Add(string.Format("Produto {0} não possui Medida associada", produtoDao.Numero));
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

                            // busca se a linha da planilha já existe na base de dados
                            // caso não exista, cadastra-a na base de dados
                            // caso exista, substitui a linha da planilha pela da base de dados por conta do MedidaID
                            var linha = produtoDao.LinhaDao.FirstOrDefault();
                            if (linha == null)
                            {
                                retorno.Add(string.Format("Produto {0} não possui Linha associada", produtoDao.Numero));
                            }
                            var linhaDao = linhaRepository.Listar(linha.ToBd()).FirstOrDefault();
                            if (linhaDao == null)
                            {
                                var linhaId = linhaBusiness.Incluir(produtoDao.LinhaDao.FirstOrDefault());
                                produtoDao.LinhaDao.FirstOrDefault().LinhaID = linhaId;
                                produtoDao.LinhaDao.FirstOrDefault().Ativo = true;
                            }
                            else
                            {
                                produtoDao.LinhaDao.Clear();
                                produtoDao.LinhaDao.Add(linhaDao.ToApp());
                            }

                            // cadastra o produto
                            produtoRepository.Incluir(produtoDao.ToBd());
                        }
                        else
                        {
                            // atualiza a quantidade do produto
                            var lojaProdutoDao = produtoDao.LojaProdutoDao.FirstOrDefault();
                            if (lojaProdutoDao != null) { produtoRepository.Atualizar(lojaProdutoDao.LojaID, produtoDao.Numero.GetValueOrDefault(), lojaProdutoDao.Quantidade); }
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
                // inclui o log do erro
                logRepository.Incluir(new Log() { Descricao = ex.ToString(), DataHora = DateTime.Now });

                throw ex;
            }

            return retorno;
        }

        private List<ProdutoDao> LerXLSX(System.IO.Stream arquivo)
        {
            List<ProdutoDao> produtosDao = new List<ProdutoDao>();

            var workbook = new XLWorkbook(arquivo);
            var worksheets = workbook.Worksheets;

            // cada aba da planilha
            foreach (var worksheet in worksheets)
            {
                ProdutoDao produtoDao = null;

                // cada célula que tem valor
                foreach (var cellUsed in worksheet.CellsUsed())
                {
                    switch (cellUsed.Address.ColumnLetter.ToUpper())
                    {
                        case "A":
                            // A = Número
                            // Inicio da linha, deve zerar o objeto para um novo produto
                            produtoDao = new ProdutoDao();
                            long numero;
                            cellUsed.TryGetValue(out numero);
                            produtoDao.Numero = numero;
                            break;
                        case "B":
                            // B = Descrição
                            string descricao;
                            cellUsed.TryGetValue(out descricao);
                            produtoDao.Descricao = descricao.Trim();
                            break;
                        case "C":
                            // C = Linha
                            string linha;
                            cellUsed.TryGetValue(out linha);
                            produtoDao.LinhaDao.Clear();
                            produtoDao.LinhaDao.Add(new LinhaDao() { Descricao = linha.Trim(), Ativo = true });
                            break;
                        case "D":
                            // D = Medida
                            string medida;
                            cellUsed.TryGetValue(out medida);
                            produtoDao.MedidaDao = new MedidaDao() { Descricao = medida.Replace(" ", "").Trim(), Ativo = true };
                            break;
                        case "E":
                            // E = Quantidade
                            short quantidade;
                            cellUsed.TryGetValue(out quantidade);
                            produtoDao.Quantidade = quantidade;

                            // busca a loja por CNPJ
                            var loja = lojaRepository.Listar(new Loja() { Cnpj = worksheet.Name.Trim(), Ativo = true }).FirstOrDefault();
                            if (loja != null)
                            {
                                produtoDao.LojaProdutoDao.Clear();
                                produtoDao.LojaProdutoDao.Add(new LojaProdutoDao() { LojaID = loja.LojaID, Quantidade = quantidade, Ativo = true });
                            }
                            break;
                        case "F":
                            // F = Preço A Pedida
                            break;
                        case "G":
                            // G = Preço A Vista
                            double precoAVista = 0;
                            if (cellUsed.Value != null)
                            {
                                double.TryParse(cellUsed.Value.ToString(), System.Globalization.NumberStyles.Currency, new System.Globalization.CultureInfo("en-US"), out precoAVista);
                                if (precoAVista > 0)
                                {
                                    produtoDao.ParcelaProdutoDao.Add(new ParcelaProdutoDao() { ParcelaID = 1, Preco = precoAVista, AVista = true });
                                }
                            }
                            break;
                        case "H":
                            // H = Preço 10x
                            double preco10x = 0;
                            if (cellUsed.Value != null)
                            {
                                double.TryParse(cellUsed.Value.ToString(), System.Globalization.NumberStyles.Currency, new System.Globalization.CultureInfo("en-US"), out preco10x);
                                if (preco10x > 0)
                                {
                                    for (int i = 1; i <= 10; i++)
                                    {
                                        produtoDao.ParcelaProdutoDao.Add(new ParcelaProdutoDao() { ParcelaID = i, Preco = preco10x, AVista = false });
                                    }
                                }
                            }
                            // Final da linha, deve tentar adicionar item na lista
                            if (produtoDao.Numero > 0)
                            {
                                // se não existir na lista, adiciona
                                if (produtosDao.FirstOrDefault(x => x.Numero == produtoDao.Numero && x.Ativo == true) == null)
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

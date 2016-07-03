using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;
using ChicoDoColchao.Business.Tradutors;

namespace ChicoDoColchao.Business
{
    public class PedidoBusiness
    {
        PedidoRepository pedidoRepository;

        public PedidoBusiness()
        {
            pedidoRepository = new PedidoRepository();
        }

        private void ValidarIncluir(PedidoDao pedidoDao)
        {
            if (pedidoDao == null)
            {
                throw new BusinessException("Pedido é obrigatório");
            }

            if (pedidoDao.ClienteDao == null || pedidoDao.ClienteDao.ClienteID <= 0)
            {
                throw new BusinessException("Cliente é obrigatório");
            }

            if (pedidoDao.DataPedido == null)
            {
                throw new BusinessException("Data do Pedido é obrigatório");
            }

            if (pedidoDao.FuncionarioDao == null || pedidoDao.FuncionarioDao.FuncionarioID <= 0)
            {
                throw new BusinessException("Funcionário é obrigatório");
            }

            if (pedidoDao.LojaOrigemDao == null || pedidoDao.LojaOrigemDao.LojaID <= 0)
            {
                throw new BusinessException("Loja de origem é obrigatório");
            }

            if (pedidoDao.LojaSaidaDao == null || pedidoDao.LojaSaidaDao.LojaID <= 0)
            {
                throw new BusinessException("Loja de saída é obrigatório");
            }

            if (pedidoDao.Numero <= 0)
            {
                throw new BusinessException("Número do pedido é obrigatório");
            }

            if (pedidoDao.PedidoProdutoDao == null || pedidoDao.PedidoProdutoDao.Count <= 0)
            {
                throw new BusinessException("Produto do pedido é obrigatório");
            }

            if (pedidoDao.PedidoStatusDao == null || pedidoDao.PedidoStatusDao.PedidoStatusID <= 0)
            {
                throw new BusinessException("Status do pedido é obrigatório");
            }

            if (pedidoDao.PedidoTipoPagamentoDao == null || pedidoDao.PedidoTipoPagamentoDao.Count <= 0)
            {
                throw new BusinessException("Tipo de pagamento do pedido é obrigatório");
            }
        }

        private void ValidarAtualizar(PedidoDao pedidoDao)
        {
            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }
        }

        private void ValidarExcluir(PedidoDao pedidoDao)
        {
            if (pedidoDao.PedidoID <= 0)
            {
                throw new BusinessException("PedidoID é obrigatório");
            }
        }

        public List<PedidoDao> Listar(PedidoDao pedidoDao)
        {
            return pedidoRepository.Listar(pedidoDao.ToBD()).Select(x => x.ToAPP()).ToList();
        }

        public void Incluir(PedidoDao pedidoDao)
        {
            try
            {
                ValidarIncluir(pedidoDao);
                pedidoRepository.Incluir(pedidoDao.ToBD());
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        
        public void Atualizar(PedidoDao pedidoDao)
        {
            try
            {
                ValidarAtualizar(pedidoDao);
                pedidoRepository.Atualizar(pedidoDao.ToBD());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public void Excluir(PedidoDao pedidoDao)
        {
            try
            {
                ValidarExcluir(pedidoDao);
                pedidoRepository.Excluir(pedidoDao.ToBD());
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }        
    }
}

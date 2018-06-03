using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChicoDoColchao.Dao;
using ChicoDoColchao.Repository;
using ChicoDoColchao.Business.Exceptions;

namespace ChicoDoColchao.Business
{
    public class MenuBusiness
    {
        MenuRepository menuRepository;
        LogRepository logRepository;

        public MenuBusiness()
        {
            menuRepository = new MenuRepository();
        }
        
        public MenuDao Listar()
        {
            try
            {
                return menuRepository.Listar();
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
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChicoDoColchao.Repository
{
    public class ParcelaRepository
    {
        private ChicoDoColchaoEntities chicoDoColchaoEntities = null;

        public ParcelaRepository()
        {
            chicoDoColchaoEntities = new ChicoDoColchaoEntities();
        }
        
        public List<Parcela> Listar(Parcela parcela)
        {
            IQueryable<Parcela> query = chicoDoColchaoEntities.Parcela;
            
            query = query.Where(x => x.Ativo);

            return query.OrderBy(x => x.Numero).ToList();
        }
    }
}

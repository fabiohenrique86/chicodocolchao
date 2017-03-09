﻿using System.Collections.Generic;

namespace ChicoDoColchao.Dao
{
    public class LojaDao
    {
        public LojaDao()
        {
            FuncionarioDao = new HashSet<FuncionarioDao>();
            LojaProdutoDao = new HashSet<LojaProdutoDao>();
            NotaFiscalDao = new HashSet<NotaFiscalDao>();
            OrcamentoDao = new HashSet<OrcamentoDao>();
            PedidoDao = new HashSet<PedidoStatusDao>();
            TransferenciaDao = new HashSet<TransferenciaDao>();
        }

        public int LojaID { get; set; }
        public string Cnpj { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Telefone { get; set; }
        public bool Ativo { get; set; }
        public bool Deposito { get; set; }

        public virtual ICollection<FuncionarioDao> FuncionarioDao { get; set; }
        public virtual ICollection<LojaProdutoDao> LojaProdutoDao { get; set; }
        public virtual ICollection<NotaFiscalDao> NotaFiscalDao { get; set; }
        public virtual ICollection<OrcamentoDao> OrcamentoDao { get; set; }
        public virtual ICollection<PedidoStatusDao> PedidoDao { get; set; }
        public virtual ICollection<TransferenciaDao> TransferenciaDao { get; set; }
    }
}

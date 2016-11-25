using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;
using BuroFactorWS.model;

namespace BuroFactorWS.src.dao
{
    public interface IDaoBuro
    {
        financiera getFinancieraPorContratoWS(string user);
        bool validaRegistroCliente(string rfc);
        relacionclientefinanciera getRelacion(string rfc, string alias, financiera financiera);
        void registraTicket(ticket ticket);
        ticket getTicket(string ticket);
        void beginTrasaction();
        void registraCliente(ClienteCarga elemento, financiera financiera);
        void commitTrasaction();
        void rollback();
        void borraTicket(ticket ticket);
        bool validaInterno(String alias, financiera financiera);
    }
}

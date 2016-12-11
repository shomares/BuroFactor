using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;
using BuroFactorWS.model;

namespace BuroFactorWS.src.dao
{
    public interface IDaoBuro : IDisposable
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
        IDaoBuro createContext();
        bool validaInterno(String alias, financiera financiera);
        relacionclientefinanciera getEmisora(string idEmisor, financiera financiera);
        persona getCliente(string rFC);
        divisa getDivisa(string divisa);
        plancontratado getPlanContratado(string user);
        List<deuda> getDeudas(string rFC);
        List<financiera> getFinancieraConRiesgo(OperacionCarga elemento, financiera financiera);
        void registraOperacion(OperacionCarga elemento, financiera financiera);
        deuda getDeuda(string folio, string idEmisor, financiera financiera);
        void registraEstado(operacion operacion);
        relacionclientefinanciera getCliente(string rFC, financiera financiera);
        List<deuda> getDeudasRiesgo(OperacionCarga elemento, financiera financiera);
        bool validaRelacion(string rfc, financiera financiera);
        void registraConsultaBuro(consulta consulta);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BuroComun.src.model.cargas;
using BuroFactorWS.src.response;
using BuroFactorWS.src.bussines;
using BuroFactorWS.src.service;
using DevTrends.WCFDataAnnotations;

namespace BuroFactorWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ValidateDataAnnotationsBehavior]
    public class BuroFactorService : IBuroFactorService
    {

        IRutinaCliente RutinaCliente = null;
        public string GetData(int value)
        {
            var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            return string.Format("You entered: {0}", value);
        }

        public ClientesResponse RegistraClientes(string ticket)
        {
            try
            {
                RutinaCliente = ServiceBuro.Instance.RutinaCliente;
                return RutinaCliente.RegistraCliente(ticket, "RFC1fA3wK]B{k{");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ClientesResponse ValidaClientes(List<ClienteCarga> lista)
        {
            try
            {
                RutinaCliente = ServiceBuro.Instance.RutinaCliente;
                return RutinaCliente.ValidaClientes(lista, "RFC1fA3wK]B{k{");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

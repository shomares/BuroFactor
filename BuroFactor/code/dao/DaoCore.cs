using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;
using BuroFactor.BuroService;
using BuroComun.src.security;

namespace BuroFactor.code.dao
{
    public class DaoCore : IDaoCore
    {

        public IEncripta encrypta { get; set; }
        private BuroService.BuroFactorServiceClient client;

        public IDaoCore Create(string usernameWs)
        {
            try
            {
                var daoCore = new DaoCore()
                {
                    encrypta = this.encrypta,
                    client = new BuroFactorServiceClient()
                };
                daoCore.client.ClientCredentials.UserName.UserName = usernameWs;
                daoCore.client.ClientCredentials.UserName.Password = encrypta.Encrypt(usernameWs);
                daoCore.client.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.None;
                return daoCore;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            if (this.client != null)
                this.client = null;
        }

        public ClientesResponse saveClientes(string ticket)
        {
            return client.RegistraClientes(ticket);
        }

        public ClientesResponse validateClient(List<ClienteCarga> listaObjetos)
        {
            return client.ValidaClientes(listaObjetos.ToArray());
        }

        public OperacionResponse validateOperaciones(List<OperacionCarga> listaObjetos)
        {
            return client.ValidaOperaciones(listaObjetos.ToArray());
        }

        public OperacionResponse saveOperacion(string ticket)
        {
            return client.RegistraOperaciones(ticket);
        }

        public OperacionResponse CancelaDeudas(List<CambiaEstadoOperacionRequest> elementos)
        {
            foreach (var elemento in elementos)
                elemento.Estado = "C";
            return client.CambiaEstado(elementos.ToArray());
        }

        public OperacionResponse PagaDeudas(List<CambiaEstadoOperacionRequest> elementos)
        {
            foreach (var elemento in elementos)
                elemento.Estado = "P";
            return client.CambiaEstado(elementos.ToArray());
        }

        public BuroResponse ConsultaBuro(string RFC)
        {
            return client.ConsultaBuro(RFC);
        }
    }
}

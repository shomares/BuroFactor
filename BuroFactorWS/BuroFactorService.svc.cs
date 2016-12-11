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
using BuroFactorWS.src.bussines.impl;
using BuroFactorWS.src.extension;
using BuroFactorWS.src.model;
using System.Web;

namespace BuroFactorWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [ValidateDataAnnotationsBehavior]
    public class BuroFactorService : IBuroFactorService
    {

        #region Cliente
        IRutinaCliente RutinaCliente = null;
        IRutinaOperacion RutinaOperacion = null;


        public string GetData(int value)
        {
            var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
            return string.Format("You entered: {0}", value);
        }

        public ClientesResponse RegistraClientes(string ticket)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaCliente = ServiceBuro.Instance.RutinaCliente;
                return RutinaCliente.RegistraCliente(ticket, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }
        public ClientesResponse ValidaClientes(List<ClienteCarga> lista)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaCliente = ServiceBuro.Instance.RutinaCliente;
                return RutinaCliente.ValidaClientes(lista, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }

        public ClientesResponse EditaCliente(CambiaClienteRequest editar)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaCliente = ServiceBuro.Instance.RutinaCliente;
                return RutinaCliente.EditaCliente(editar, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }


        #endregion


        #region Operaciones

        public OperacionResponse RegistraOperaciones(string ticket)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaOperacion = ServiceBuro.Instance.RutinaOperacion;
                return RutinaOperacion.RegistraOperaciones(ticket, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }
        public OperacionResponse ValidaOperaciones(List<OperacionCarga> lista)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaOperacion = ServiceBuro.Instance.RutinaOperacion;
                return RutinaOperacion.ValidaOperaciones(lista, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }
        public OperacionResponse CambiaEstado(List<CambiaEstadoOperacionRequest> estados)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaOperacion = ServiceBuro.Instance.RutinaOperacion;
                return RutinaOperacion.CambiaEstado(estados, user);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }
        }
        public BuroResponse ConsultaBuro(String RFC)
        {
            try
            {
                var user = OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name;
                RutinaOperacion = ServiceBuro.Instance.RutinaOperacion;
                return RutinaOperacion.ConsultaBuro(RFC, user, HttpContext.Current.Request.UserHostAddress);
            }
            catch (Exception ex)
            {
                var error = ex.getInnerExceptions();
                if (error != null)
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString(), InnerException = error.ToArray() }, new FaultReason("Internal Exception "));
                else
                    throw new FaultException<UnexpectedServiceFault>(new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() }, new FaultReason("Internal Exception "));
            }

        }
        #endregion
    }
}

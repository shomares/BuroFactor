using GenericLinq.Correo;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactor.code.bussines.interfaces;
using BuroComun.src.security;

namespace BuroFactor.code.service
{
    public class ServiceBuro
    {

        private readonly IApplicationContext ctx = ContextRegistry.GetContext();
        private ContenedorPlantillas _contenedor;


        private ServiceBuro()
        {

        }

        private static ServiceBuro instance;

        public static ServiceBuro Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceBuro();
                return instance;
            }

        }

        public IEncripta FactoryEncripta {
            get
            {

                return ctx.GetObject("Encripta") as IEncripta;
            }
        }

        public IContrato Contrato
        {
            get
            {

                return ctx.GetObject("ContratoBusiness") as IContrato;
            }
        }

        public IBuroQuery DaoBuro
        {
            get
            {
                return ctx.GetObject("DaoBuro") as IBuroQuery;
            }
        }

        public ContenedorPlantillas getContenedorPlantillas()
        {
            try
            {
                if (_contenedor == null)
                {
                    _contenedor = ctx.GetObject("ContenedorPlantillas") as ContenedorPlantillas;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _contenedor;
        }
    }
}
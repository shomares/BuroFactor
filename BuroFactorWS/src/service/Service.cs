using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using BuroFactorWS.src.security;
using BuroFactorWS.src.bussines;
using BuroFactorWS.src.bussines.impl;
using GenericLinq.Correo;

namespace BuroFactorWS.src.service
{
    public class ServiceBuro
    {

        private readonly IApplicationContext ctx = ContextRegistry.GetContext();


        private ServiceBuro()
        {

        }

        private static ServiceBuro instance;
        private ContenedorPlantillas _contenedor;

        public IValidateCredentials ValidateProvider
        {
            get { return ctx.GetObject("ValidateProvider") as IValidateCredentials; }

        }

        public static ServiceBuro Instance
        {
            get
            {
                if (instance == null)
                    instance = new ServiceBuro();
                return instance;
            }

        }

        public IRutinaCliente RutinaCliente
        {
            get
            {
                return ctx.GetObject("RutinaCliente") as IRutinaCliente;

            }
        }

        public IRutinaOperacion RutinaOperacion
        {
            get
            {
                return ctx.GetObject("RutinaOperacion") as IRutinaOperacion;

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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using Spring.Context.Support;
using BuroFactorWS.src.security;
using BuroFactorWS.src.bussines;

namespace BuroFactorWS.src.service
{
    public class ServiceBuro
    {

        private readonly IApplicationContext ctx = ContextRegistry.GetContext();


        private ServiceBuro()
        {

        }

        private static ServiceBuro instance;


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
    }
}
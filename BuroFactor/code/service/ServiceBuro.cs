using GenericLinq.Correo;
using Spring.Context;
using Spring.Context.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactor.code.bussines.interfaces;
using BuroComun.src.security;
using GenericLinq.Excel.Data;
using BuroFactor.code.dao;

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

        public IRutinaCambios createRutinaCambio()
        {
            return (ctx.GetObject("RutinaCambio") as IRutinaCambios)?.Create();
        }

        public IEncripta FactoryEncripta
        {
            get
            {

                return ctx.GetObject("Encripta") as IEncripta;
            }
        }

        public IRutinaOpenacion createRutinaOperacion()
        {
            return (ctx.GetObject("RutinaOperacion") as IRutinaOpenacion)?.create();
        }

        public IContrato Contrato
        {
            get
            {
                return ctx.GetObject("ContratoBusiness") as IContrato;
            }
        }

        public IRutinaCliente createRutinaCliente()
        {

            return (ctx.GetObject("RutinaCliente") as IRutinaCliente)?.create();
        }

        public IBuroQuery DaoBuro
        {
            get
            {
                return ctx.GetObject("DaoBuro") as IBuroQuery;
            }
        }

        public IContainerLayout Container { get { return ctx.GetObject("ContainerLayout") as IContainerLayout; } }

        public ITokenOperaciones ValidaToken
        {
            get { return ctx.GetObject("ITokenOperaciones") as ITokenOperaciones; }

        }

        public TimeSpan TiempoSesion
        {
            get
            {
                return TimeSpan.FromMinutes(20);

            }
        }

        public IDaoCore DaoCore { get { return ctx.GetObject("DaoCore") as IDaoCore; } }

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
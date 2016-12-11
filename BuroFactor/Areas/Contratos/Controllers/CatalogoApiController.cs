using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.service;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BuroFactor.Areas.Contratos.Controllers
{
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class CatalogoApiController : ApiController
    {
        private IBuroQuery daoQuery; 
        [HttpPost]
        public async Task<HttpResponseMessage> getFinancieras()
        {
            List<financiera> financieras;
            try
            {
                daoQuery = ServiceBuro.Instance.DaoBuro;
                var task = Task.Run(() =>
                {
                    financieras = daoQuery.getFinancieras();

                    return Request.CreateResponse(HttpStatusCode.OK,
                        (from aux in financieras
                         select new
                         {
                             RazonSocial = aux.persona.RazonSocial,
                             RFC = aux.persona.RFC,
                             idPersona = aux.persona.idPersona,
                             idFinanciera = aux.idFinanciera
                         }).ToList()

                        );


                });

                return await task;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        [HttpPost]
        public async Task<HttpResponseMessage> getFinancierasPorRFC(String RFC)
        {

            List<financiera> financieras;
            try
            {
                daoQuery = ServiceBuro.Instance.DaoBuro;
                var task = Task.Run(() =>
                {
                    financieras = daoQuery.getFinancieras(RFC);

                    if (financieras.Count > 0)
                        return Request.CreateResponse(HttpStatusCode.OK,
                         (from aux in financieras
                          select new
                          {
                              RazonSocial = aux.persona.RazonSocial,
                              RFC = aux.persona.RFC,
                              idPersona = aux.persona.idPersona,
                              idFinanciera = aux.idFinanciera
                          }).ToList()

                         );
                    else
                        return Request.CreateResponse(HttpStatusCode.NotFound);


                });

                return await task;
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> getPlanes()
        {

            List<planconsulta> planes;
            try
            {
                daoQuery = ServiceBuro.Instance.DaoBuro;
                var task = Task.Run(() =>
                {
                    planes = daoQuery.getPlanes();


                    return Request.CreateResponse(HttpStatusCode.OK,
                        (from aux in planes
                         select new
                         {
                             idPlanConsulta = aux.idPlanConsulta,
                             Nombre = aux.Nombre
                         }
                         ).ToList());



                });

                return await task;
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        public async Task<HttpResponseMessage> getPlanesPorFinanciera(int financiera)
        {
            List<plancontratado> planes;
            try
            {
                daoQuery = ServiceBuro.Instance.DaoBuro;

                var task = Task.Run(() =>
                {
                    planes = daoQuery.getPlanesPorFinanciera(financiera);

                    return Request.CreateResponse(HttpStatusCode.OK,
                        (from aux in planes
                         select new
                         {
                             Nombre = aux.planconsulta.Nombre,
                             FechaContrato = aux.FechaContrato,
                             MaxConsultaMes = aux.planconsulta.MaxConsultaMes,
                             id = aux.idPlanContratado
                         }).ToList());



                });
                return await task;
            }
            catch (Exception)
            {

                throw;
            }
        }


    }
}

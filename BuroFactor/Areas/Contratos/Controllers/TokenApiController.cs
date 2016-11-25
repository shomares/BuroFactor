using BuroFactor.Areas.Contratos.Models;
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
    public class TokenApiController : ApiController
    {
        private IBuroQuery daoQuery = ServiceBuro.Instance.DaoBuro;

        private IContrato Contratos = ServiceBuro.Instance.Contrato;

        public async Task<HttpResponseMessage> validateToken(ModelToken model)
        {
            plancontratado plan;
            try
            {
                var task = Task.Run(() =>
                {
                    plan = daoQuery.getPlanPorSerial(model.token);
                    if (plan != null && (DateTime.Now - plan.FechaContrato).Minutes < 20 && plan.Activo == false)
                    {
                        //El RFC es Correcto
                        if (model.RFC == plan.financiera.persona.RFC && model.contrasena == model.validacionContrasena)
                        {
                            Contratos.altaContrato(plan);
                            Contratos.creaUsuarioFinanciera(plan.financiera, model.contrasena);

                            return Request.CreateResponse(HttpStatusCode.OK, plan.Serial);
                        }
                        else
                            return Request.CreateResponse(HttpStatusCode.BadRequest, plan.Serial);
                    }
                    else
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "");
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

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
    //[Authorize(Roles = "ADMINISTRADOR")]
    public class RegistroPlanApiController : ApiController
    {
        private IContrato Contratos = ServiceBuro.Instance.Contrato;
        [HttpPost]
        public async Task<HttpResponseMessage> registraContrato(ModelPlanRegisro plan)
        {

            try
            {
                var task = Task.Run(() =>
                {
                    plancontratado planC = Contratos.getPlanContratado(plan.idFinanciera, plan.plan);
                    Contratos.registraContrato(planC);
                    return Request.CreateResponse(HttpStatusCode.OK, planC.Serial);
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

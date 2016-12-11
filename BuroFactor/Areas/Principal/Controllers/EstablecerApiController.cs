using BuroFactor.Areas.Principal.Models;
using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.service;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuroFactor.Areas.Principal.Controllers
{
    [Authorize]
    public class EstablecerApiController : ApiController
    {
        public HttpResponseMessage Cuenta(PasswordRenovar pass)
        {
            ITokenOperaciones tokenOperaciones;
            IContrato contrato;

            financiera financieraa = null;

            tokenOperaciones = ServiceBuro.Instance.ValidaToken;


            if (tokenOperaciones.validaToken(User.Identity.Name, pass.password))
            {
                financieraa = ServiceBuro.Instance.DaoBuro.getFinancieraPorRFC(User.Identity.Name);
                if (financieraa != null)
                {
                    contrato = ServiceBuro.Instance.Contrato;
                    contrato.creaUsuarioFinanciera(financieraa, pass.passwordNueva);
                    return Request.CreateResponse(HttpStatusCode.OK, "Se ha asignado la contraseña correctamente");
                }
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "No existe la financiera registrada");
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError, "La contraseña asignada no coincide");
        }


    }
}

using BuroFactor.Areas.Operacion.Models;
using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.extension.bag;
using BuroFactor.code.extension.html;
using BuroFactor.code.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BuroFactor.Areas.Operacion.Controllers
{
    [Authorize(Roles = "FINANCIERA")]
    public class TokenValidateController : ApiController
    {
        public dynamic Execute(TokenDTO token)
        {
            ITokenOperaciones tokenOperaciones;
            Ticket ticket = TicketBag.Instance.getTicket();
            dynamic salida = null;
            if (ticket != null)
            {
                tokenOperaciones = ServiceBuro.Instance.ValidaToken;

                //Validamos el token--------------------------------------------------
                if (!ticket.IsExpires)
                {
                    if (tokenOperaciones.validaToken(User.Identity.Name, token.Token))
                    {
                        Work work = ticket.Work;
                        //Corremos el work
                        salida = work.run();
                        //--------------------------------------------------
                        TicketBag.Instance.removeTicket(token.IdTicket);
                    }
                    else
                        salida = null;
                }
                else
                {
                    salida = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    TicketBag.Instance.removeTicket(token.IdTicket);
                }

            }
            return salida;
        }
    }
}
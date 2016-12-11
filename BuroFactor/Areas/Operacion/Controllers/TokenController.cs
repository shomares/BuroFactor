using BuroFactor.code.extension.bag;
using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Operacion.Controllers
{
    [Security(Roles = "FINANCIERA", NotifyUrl = "~/Principal/Errores/Error404")]
    public class TokenController : Controller
    {
        // GET: Operacion/Token
        public ActionResult Index(String id)
        {

            Ticket ticket = TicketBag.Instance.getTicket();

            if (ticket != null && !ticket.isRequest)
            {
                if (ticket.isToken)
                {
                    ViewBag.TipoToken = "Clave dinámica";
                    ViewBag.ErrorToken = "La Clave dinámica es invalida";
                }
                else
                {
                    ViewBag.TipoToken = "Contraseña";
                    ViewBag.ErrorToken = "La contraseña es invalida";
                }
                if (!ticket.IsExpires)
                {
                    ticket.isRequest = true;
                    return View();
                }
                else
                    return Redirect("~/Principal/UnauthorizedPage");
            }
            else
                return Redirect("~/Principal/UnauthorizedPage");
        }
    }
}
using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Operacion.Controllers
{
    [Security(Roles = "FINANCIERA", NotifyUrl = "~/Principal/Errores/Error404")]
    public class ClienteController : Controller
    {
        // GET: Operacion/Cliente
        public ActionResult Index()
        {

            return View();
        }

    }
}
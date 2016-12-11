using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Principal.Controllers
{
    [Security(NotifyUrl = "~/Principal/Errores/Error404")]
    public class CuentaController : Controller
    {
        // GET: Principal/Cuenta/Establecer
        public ActionResult Establecer()
        {
            return View();
        }
    }
}
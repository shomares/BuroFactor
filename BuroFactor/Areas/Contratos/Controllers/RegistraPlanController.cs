using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Contratos.Controllers
{
    //[Security(Roles = "ADMINISTRADOR", NotifyUrl = "~/Principal/Errores/Error404")]
    public class RegistraPlanController : Controller
    {
        // GET: Contratos/RegistraPlan
        public ActionResult Index()
        {
            return View();
        }
    }
}
using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Operacion.Controllers
{
    [Security(Roles = "FINANCIERA", NotifyUrl = "~/Principal/Errores/Error404")]
    public class OperacionController : Controller
    {
        // GET: Operacion/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cancelacion() {
            return View();
        }
        public ActionResult Liquidar() {
            return View();
        }
    }
}
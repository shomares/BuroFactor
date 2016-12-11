using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Reporte.Controllers
{
    [Security(Roles = "FINANCIERA", NotifyUrl = "~/Principal/Errores/Error404")]
    public class VisorController : Controller
    {
        public ActionResult Index() {
            return View();
        }
        // GET: Reporte/Visor
        public ActionResult Vigente()
        {
            return View();
        }

        public ActionResult Vencida() {
            return View();
        }


        public ActionResult Cancelada()
        {
            return View();
        }

        public ActionResult Pagada()
        {
            return View();
        }

        public ActionResult Cliente() {
            return View();
        }





    }
}
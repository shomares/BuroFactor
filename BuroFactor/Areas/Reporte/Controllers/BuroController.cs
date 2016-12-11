using BuroFactor.code.dao;
using BuroFactor.code.extension.html;
using BuroFactor.code.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Reporte.Controllers
{
    [Security(Roles = "FINANCIERA", NotifyUrl = "~/Principal/Errores/Error404")]
    public class BuroController : Controller
    {
        // GET: Reporte/Buro
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult Visualizar(BuroFactor.Areas.Reporte.Models.ConsultaBuro consulta)
        {
            IDaoCore daoCoreProvider = ServiceBuro.Instance.DaoCore;
            using (IDaoCore daoCore = daoCoreProvider.Create((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value))
            {
                var response = daoCore.ConsultaBuro(consulta.RFC);
                if (response != null)
                {
                    return View(response);
                }
            }
            return null;
        }
    }
}
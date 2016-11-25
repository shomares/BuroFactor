using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.service;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Contratos.Controllers
{
    public class TokenController : Controller
    {

        private IBuroQuery daoQuery = ServiceBuro.Instance.DaoBuro;

        // GET: Contratos/Token?Token=
        public ActionResult Index(String Token)
        {
            plancontratado plan;

            plan = daoQuery.getPlanPorSerial(Token);

            if (plan != null && (DateTime.Now - plan.FechaContrato).Minutes < 20 && !plan.Activo)
                return View();
            else
                return Redirect("~/");

        }


    }
}
﻿using BuroFactor.code.extension.html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Areas.Principal.Controllers
{

    [Security(NotifyUrl = "~/Principal/Errores/Error404")]

    public class ErroresController : Controller
    {
        // GET: Principal/Error404
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }


    }
}
using BuroComun.src.security;
using BuroFactor.code.extension.html;
using BuroFactor.code.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuroFactor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            IEncripta encrypta = null;


            try
            {
                BuroService.BuroFactorServiceClient client = new BuroService.BuroFactorServiceClient();

                encrypta = ServiceBuro.Instance.FactoryEncripta;

                client.ClientCredentials.UserName.UserName = "RFC1fA3wK]B{k{";
                client.ClientCredentials.UserName.Password = "ad1045af-0931-4916-8d55-ec9761ba5de3";


                client.GetData(1);
            }
            catch (Exception ex)
            {

               
            }

            return View();
        }

        [ContentType("text/javascript")]
        public ActionResult RootPath()
        {

            return View();
        }
    }
}

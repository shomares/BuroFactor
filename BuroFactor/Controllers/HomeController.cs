using BuroComun.src.security;
using BuroFactor.code.extension.html;
using BuroFactor.code.service;
using BuroFactor.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThreadExecutor.thread;
using WebMatrix.WebData;

namespace BuroFactor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!this.User.Identity.IsAuthenticated)
            {
                ViewBag.Title = "Home Page";
                return View();
            }
            else
                return Redirect("~/Principal/Inicio");
        }


        [HttpPost]
        [AllowAnonymous]
        [MyValidateAntiForgeryToken]
        public ActionResult Logger(UsuarioDTO user, string ReturnUrl)
        {
            Result<HttpStatusCodeResult> result = null;
            try
            {
                if (ModelState.IsValid && WebSecurity.Login(user.Username, user.Password))
                {
                    Response.SetCookie(new HttpCookie("LOGINEFACTOR")
                    {
                        Expires = DateTime.Now.AddDays(1),
                        Value = "true"
                    });
                    if (!Request.IsAjaxRequest())
                    {
                        if (string.IsNullOrEmpty(ReturnUrl))
                            return Redirect("~/Principal/Inicio");
                        else
                            return RedirectToLocal(ReturnUrl);
                    }
                    else
                    {
                        if (!Url.IsLocalUrl(ReturnUrl))
                            ReturnUrl = "";
                        else
                        {
                            var url = new Uri(HttpContext.Request.Url, ReturnUrl);
                            ReturnUrl = url.AbsoluteUri;
                        }
                        result = new Result<HttpStatusCodeResult>() { Notificacion = ReturnUrl, State = States.StateEnum.OK };
                        return Json(result, JsonRequestBehavior.DenyGet);
                    }
                }
                else
                {
                    if (!Request.IsAjaxRequest())
                    {
                        if (TempData.ContainsKey("Error"))
                            TempData.Remove("Error");
                        TempData.Add("Error", "El usuario o la contrasena es invalida");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        result = new Result<HttpStatusCodeResult>() { Notificacion = "El usuario o la contrasena es invalida", State = States.StateEnum.ERROR };
                        return Json(result, JsonRequestBehavior.DenyGet);
                    }

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Authorize]
        public ActionResult CloseSession()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //CrystalBag.Instance.closeSession(HttpContext.Current.Session.SessionID);
            HttpContext.Request.Cookies.Clear();
            HttpContext.Session.Clear();
            HttpContext.Session.Abandon();
            return Redirect("~/Home");
        }
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect("~/" + returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        [ContentType("text/javascript")]
        public ActionResult RootPath()
        {

            return View();
        }
    }
}

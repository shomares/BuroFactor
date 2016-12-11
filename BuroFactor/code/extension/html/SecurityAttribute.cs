using log4net;
using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;


namespace BuroFactor.code.extension.html
{

    public class SecurityAttribute : AuthorizeAttribute
    {
        // Set default Unauthorized Page Url here
        private string _notifyUrl = "/Error/Unauthorized";



        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SecurityAttribute()
        {

        }

        public bool RequireQueryString
        {
            get; set;
        }


        public string NotifyUrl
        {
            get { return _notifyUrl; }
            set { _notifyUrl = value; }
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //Para OAM


            base.HandleUnauthorizedRequest(filterContext);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            //Sirve par js
            filterContext.HttpContext.Response.SetCookie(new HttpCookie("authData")
            {
                Expires = DateTime.Now.AddDays(-1),
                Value = "true"
            });

            if (AuthorizeCore(filterContext.HttpContext))
            {
                HttpCachePolicyBase cachePolicy =
                    filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                //cachePolicy.AddValidationCallback(CacheValidateHandler, null);
            }
            else if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                if (NotifyUrl != null)
                {
                    log.Info(String.Format("Peticion a la URL: {0}, no es autorizada para el usuario: {1} ",
                        HttpContext.Current.Request.Url.ToString(), HttpContext.Current?.User?.Identity?.Name));
                    filterContext.Result = new RedirectResult(NotifyUrl);
                }
                else
                {
                    log.Info(String.Format("Peticion a la URL: {0}, no es autorizada para el usuario: {1} ",
                        HttpContext.Current.Request.Url.ToString(), HttpContext.Current?.User?.Identity?.Name));
                    HandleUnauthorizedRequest(filterContext);
                }
            }
            else
            {
                log.Info(String.Format("Peticion a la URL: {0}, no es autorizada para el usuario: {1} ",
                    HttpContext.Current.Request.Url.ToString(), HttpContext.Current?.User?.Identity?.Name));
                HandleUnauthorizedRequest(filterContext);
            }
        }
    }

}
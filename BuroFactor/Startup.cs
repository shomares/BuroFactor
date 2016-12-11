using BuroFactor.code.service;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MultipartDataMediaFormatter;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(BuroFactor.Startup))]
namespace BuroFactor
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var provider = new CookieAuthenticationProvider();
            var originalHandler = provider.OnApplyRedirect;
            MvcHandler.DisableMvcResponseHeader = true;
            config.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter());
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ApplicationCookie",
                CookieSecure = CookieSecureOption.SameAsRequest,
                ExpireTimeSpan = ServiceBuro.Instance.TiempoSesion,
                LoginPath = new PathString("/Home"),
                Provider = provider,
                SlidingExpiration = true,
                CookieHttpOnly = true
            });
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }

    }
}
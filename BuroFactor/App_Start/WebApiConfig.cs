using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace BuroFactor
{
    public static class WebApiConfig
    {
        public static string UrlPrefixRelative { get { return "~/api"; } }

        public static void Register(HttpConfiguration config)
        {

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SupportedEncodings.Add(Encoding.UTF8);
            config.Formatters.JsonFormatter.SupportedEncodings.RemoveAt(0);
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "Reporte",
             routeTemplate: "api/Reporte/{controller}/{action}");


            config.Routes.MapHttpRoute(
               name: "Contratos",
               routeTemplate: "api/Contratos/{controller}/{action}");
            config.Routes.MapHttpRoute(
             name: "Operacion",
             routeTemplate: "api/Operacion/{controller}/{action}");

            config.Routes.MapHttpRoute(
           name: "Principal",
           routeTemplate: "api/Principal/{controller}/{action}");


        }
    }
}

using BuroFactor.Areas.Operacion.Models;
using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.dao;
using BuroFactor.code.extension.html;
using BuroFactor.code.service;
using BuroFactor.reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using ThreadExecutor.thread;

namespace BuroFactor.Areas.Reporte.Controllers
{
    [Authorize(Roles = "FINANCIERA")]
    public class EngineController : ApiController
    {

        


        [HttpPost]
        public HttpResponseMessage BuscarFacturasCanceladas(BuscarModelFactura model)
        {
            Result<string> salida = new Result<string>();
            IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            //reporteSession = new ReportWeb();
            parametros.Add("financiera", "Banco Interacciones S.A.");
            parametros.Add("nombre", "Operaciones Canceladas");

            var elementos = dao.getDeudasCanceladas(model.empresa, model.FechaIni, model.FechaFin, model.Divisa, (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);
            if (elementos.Count > 0)
            {
                var datos = (from elemento in elementos
                             select new
                             {
                                 folio = elemento.FolioDocumento,
                                 monto = elemento.MontoNominal,
                                 financiado = elemento.MontoFinanciado,
                                 emisor = elemento.relacionclientefinanciera.persona.RazonSocial,
                                 proveedor = elemento.relacionclientefinanciera1.persona.RazonSocial,
                                 fechaOperacion = elemento.FechaOperacion,
                                 fechaVencimiento = elemento.FechaPago,
                                 financiera = elemento.financiera.persona.RazonSocial
                             }).ToList();

                if (HttpContext.Current.Session["REPORTE"] != null)
                    HttpContext.Current.Session.Remove("REPORTE");

                HttpContext.Current.Session.Add("REPORTE", new ReportSessionObject()
                {
                    Datos = datos.ToArray<object>(),
                    Parametros = parametros.Select(x => x.Key + "|" + x.Value).ToArray(),
                    ReportNameType = typeof(rptOperacion).FullName
                });
                salida.State = States.StateEnum.OK;
            }
            else
            {
                salida.Notificacion = "No hay resultados";
                salida.State = States.StateEnum.ERROR;
            }

            return Request.CreateResponse(HttpStatusCode.OK, salida);

        }





        [HttpPost]
        public HttpResponseMessage BuscarFacturasPagadas(BuscarModelFactura model)
        {
            Result<string> salida = new Result<string>();
            IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            //reporteSession = new ReportWeb();
            parametros.Add("financiera", "Banco Interacciones S.A.");
            parametros.Add("nombre", "Operaciones Liquidadas");

            var elementos = dao.getDeudasPagadas(model.empresa, model.FechaIni, model.FechaFin, model.Divisa, (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);
            if (elementos.Count > 0)
            {
                var datos = (from elemento in elementos
                             select new
                             {
                                 folio = elemento.FolioDocumento,
                                 monto = elemento.MontoNominal,
                                 financiado = elemento.MontoFinanciado,
                                 emisor = elemento.relacionclientefinanciera.persona.RazonSocial,
                                 proveedor = elemento.relacionclientefinanciera1.persona.RazonSocial,
                                 fechaOperacion = elemento.FechaOperacion,
                                 fechaVencimiento = elemento.FechaPago,
                                 financiera = elemento.financiera.persona.RazonSocial
                             }).ToList();

                if (HttpContext.Current.Session["REPORTE"] != null)
                    HttpContext.Current.Session.Remove("REPORTE");

                HttpContext.Current.Session.Add("REPORTE", new ReportSessionObject()
                {
                    Datos = datos.ToArray<object>(),
                    Parametros = parametros.Select(x => x.Key + "|" + x.Value).ToArray(),
                    ReportNameType = typeof(rptOperacion).FullName
                });
                salida.State = States.StateEnum.OK;
            }
            else
            {
                salida.Notificacion = "No hay resultados";
                salida.State = States.StateEnum.ERROR;
            }

            return Request.CreateResponse(HttpStatusCode.OK, salida);

        }


        [HttpPost]
        public HttpResponseMessage BuscarFacturasVencidas(BuscarModelFactura model)
        {
            Result<string> salida = new Result<string>();
            IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            //reporteSession = new ReportWeb();
            parametros.Add("financiera", "Banco Interacciones S.A.");
            parametros.Add("nombre", "Operaciones Vencidas");

            var elementos = dao.getDeudasVencidas(model.empresa, model.FechaIni, model.FechaFin, model.Divisa, (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);
            if (elementos.Count > 0)
            {
                var datos = (from elemento in elementos
                             select new
                             {
                                 folio = elemento.FolioDocumento,
                                 monto = elemento.MontoNominal,
                                 financiado = elemento.MontoFinanciado,
                                 emisor = elemento.relacionclientefinanciera.persona.RazonSocial,
                                 proveedor = elemento.relacionclientefinanciera1.persona.RazonSocial,
                                 fechaOperacion = elemento.FechaOperacion,
                                 fechaVencimiento = elemento.FechaPago,
                                 financiera = elemento.financiera.persona.RazonSocial
                             }).ToList();

                if (HttpContext.Current.Session["REPORTE"] != null)
                    HttpContext.Current.Session.Remove("REPORTE");

                HttpContext.Current.Session.Add("REPORTE", new ReportSessionObject()
                {
                    Datos = datos.ToArray<object>(),
                    Parametros = parametros.Select(x => x.Key + "|" + x.Value).ToArray(),
                    ReportNameType = typeof(rptOperacion).FullName
                });
                salida.State = States.StateEnum.OK;
            }
            else
            {
                salida.Notificacion = "No hay resultados";
                salida.State = States.StateEnum.ERROR;
            }

            return Request.CreateResponse(HttpStatusCode.OK, salida);

        }

        [HttpPost]
        public HttpResponseMessage BuscarFacturasActivas(BuscarModelFactura model)
        {
            Result<string> salida = new Result<string>();
            IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
            Dictionary<string, object> parametros = new Dictionary<string, object>();
            //reporteSession = new ReportWeb();
            parametros.Add("financiera", "Banco Interacciones S.A.");
            parametros.Add("nombre", "Operaciones Activas");

            var elementos = dao.getDeudasActivas(model.empresa, model.FechaIni, model.FechaFin, model.Divisa, (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);
            if (elementos.Count > 0)
            {
                var datos = (from elemento in elementos
                             select new
                             {
                                 folio = elemento.FolioDocumento,
                                 monto = elemento.MontoNominal,
                                 financiado = elemento.MontoFinanciado,
                                 emisor = elemento.relacionclientefinanciera.persona.RazonSocial,
                                 proveedor = elemento.relacionclientefinanciera1.persona.RazonSocial,
                                 fechaOperacion = elemento.FechaOperacion,
                                 fechaVencimiento = elemento.FechaPago,
                                 financiera = elemento.financiera.persona.RazonSocial
                             }).ToList();

                if (HttpContext.Current.Session["REPORTE"] != null)
                    HttpContext.Current.Session.Remove("REPORTE");

                HttpContext.Current.Session.Add("REPORTE", new ReportSessionObject()
                {
                    Datos = datos.ToArray<object>(),
                    Parametros = parametros.Select(x => x.Key + "|" + x.Value).ToArray(),
                    ReportNameType = typeof(rptOperacion).FullName
                });
                salida.State = States.StateEnum.OK;
            }
            else
            {
                salida.Notificacion = "No hay resultados";
                salida.State = States.StateEnum.ERROR;
            }

            return Request.CreateResponse(HttpStatusCode.OK, salida);
        }


    }
}

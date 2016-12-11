using BuroFactor.Areas.Operacion.Models;
using BuroFactor.BuroService;
using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.extension.bag;
using BuroFactor.code.service;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BuroFactor.Areas.Operacion.Controllers
{
    [Authorize(Roles = "FINANCIERA")]
    public class OperacionApiController : ApiController
    {


        [HttpPost]
        public HttpResponseMessage Catalogos()
        {
            try
            {
                IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
                List<divisa> divisas = dao.getDivisas();

                List<relacionclientefinanciera> relacion;
                var moneda = (from aux in divisas
                              select new
                              {
                                  id = aux.idDivisa,
                                  nombre = aux.nombre
                              }).ToList();

                relacion = dao.getRelacionesPorFinanciera((User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);

                var result = (from aux in relacion
                              select new { id = aux.idRelacionClienteFinanciera, nombre = aux.persona.RazonSocial, alias = aux.Alias }).ToList();


                return Request.CreateResponse(HttpStatusCode.OK, new { empresas = result, divisas = moneda });
            }
            catch (Exception ex)
            {

                throw;
            }


        }



        [HttpPost]
        public HttpResponseMessage BuscarFacturas(BuscarModelFactura model)
        {

            try
            {
                IBuroQuery dao = ServiceBuro.Instance.DaoBuro;
                List<deuda> deudasActivas = dao.getDeudasActivas(model.empresa, model.FechaIni, model.FechaFin, model.Divisa, (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p => p.Type == ClaimTypes.SerialNumber).Value);
                var result = (from aux in deudasActivas
                              select new
                              {
                                  id= aux.idDeuda,
                                  folio = aux.FolioDocumento,
                                  montoNominal = aux.MontoNominal,
                                  montoFinanciado = aux.MontoFinanciado,
                                  divisa = aux.divisa.nombre,
                                  emisor = aux.relacionclientefinanciera.Alias,
                                  proveedor = aux.relacionclientefinanciera1.Alias,
                                  fechaPublicacion= aux.FechaOperacion,
                                  fechaEmision= aux.FechaEmision,
                                  fechaVencimiento= aux.FechaPago
                              }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public HttpResponseMessage PagaDocumentos(List<FacturaModificar> lista)
        {

            HttpResponseMessage salida = null;
            IRutinaCambios rutina = ServiceBuro.Instance.createRutinaCambio();
            if (lista.Count > 0)
            {
                rutina.Elementos = lista;
                rutina.State = RUTINACAMBIOSSTATE.PAGA;
                rutina.usernameWs = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p=>p.Type== ClaimTypes.NameIdentifier).Value;
                Ticket ticket = new Ticket();
                ticket.Work = new Work() { WorkI = rutina };
                ticket.isRunner = false;
                TicketBag.Instance.addTicket(ticket);
                salida = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                salida = Request.CreateResponse(HttpStatusCode.BadRequest);
            return salida;
        }


        [HttpPost]
        public HttpResponseMessage CancelaDocumentos(List<FacturaModificar> lista)
        {

            HttpResponseMessage salida = null;
            IRutinaCambios rutina = ServiceBuro.Instance.createRutinaCambio();
            if (lista.Count > 0)
            {
                rutina.Elementos = lista;
                rutina.State = RUTINACAMBIOSSTATE.CANCELA;
                rutina.usernameWs = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p=>p.Type== ClaimTypes.NameIdentifier).Value;

                Ticket ticket = new Ticket();
                ticket.Work = new Work() { WorkI = rutina };
                ticket.isRunner = false;
                TicketBag.Instance.addTicket(ticket);
                salida = Request.CreateResponse(HttpStatusCode.OK);
            }
            else
                salida = Request.CreateResponse(HttpStatusCode.BadRequest);
            return salida;
        }

        public async Task<HttpResponseMessage> CargaLayout()
        {
            string usernameWs = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p=>p.Type== ClaimTypes.NameIdentifier).Value;
            var user = (ClaimsIdentity)User.Identity;

            return await CargaLayoutTask(usernameWs);
        }


        private async Task<HttpResponseMessage> CargaLayoutTask(string usernameWs)
        {
            HttpResponseMessage salida = new HttpResponseMessage();
            IRutinaOpenacion rutina = ServiceBuro.Instance.createRutinaOperacion();
            MultipartFormDataStreamProvider result = null;
            try
            {
                string path, targetFolder;
                if (!Request.Content.IsMimeMultipartContent())
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
                path = HttpContext.Current.Server.MapPath("~/tmp");
                targetFolder = path + @"\" + Guid.NewGuid().ToString() + @"\";
                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                var provider = new MultipartFormDataStreamProvider(targetFolder);
                result = await Request.Content.ReadAsMultipartAsync(provider);

                if (!Request.Content.IsMimeMultipartContent() && provider.FileData.Count > 1)
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                MultipartFileData fileData = provider.FileData.FirstOrDefault();
                rutina.RutaExcel = fileData.LocalFileName;
                rutina.NombreExcel = fileData.Headers.ContentDisposition.FileName;
                rutina.UsernameWs = usernameWs;
                rutina.layout = "CargaOperacion";
                rutina.Path = targetFolder;
                BuroService.OperacionResponse response = await rutina.validate();
                if (response.Error)
                {
                    var s = (from a in response.Errores
                             select new
                             {
                                 folio = a.Operacion?.Folio,
                                 error = a.Error,
                             }
                            ).ToList();

                    salida = Request.CreateResponse(HttpStatusCode.OK, new { error = true, errores = s });
                }
                else
                {
                    Ticket ticket = new Ticket();
                    ticket.Work = new Work() { WorkI = rutina };
                    ticket.isRunner = false;
                    TicketBag.Instance.addTicket(ticket);
                    salida = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        error = false,
                        token = response.Token,
                        riesgos =
                                (from aux in response.Errores
                                 where aux.isRiesgo
                                 select new { folio = aux.Operacion?.Folio, riesgo = aux.Error }
                                 ).ToList()
                    });
                }
                return salida;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

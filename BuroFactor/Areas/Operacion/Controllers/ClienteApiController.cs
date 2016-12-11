using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.bussines.operacion;
using BuroFactor.code.extension.bag;
using BuroFactor.code.service;
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
    public class ClienteApiController : ApiController
    {
        public async Task<HttpResponseMessage> CargaLayout()
        {
            string usernameWs = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(p=>p.Type== ClaimTypes.NameIdentifier).Value;
            var user = (ClaimsIdentity)User.Identity;

            return await CargaLayoutTask(usernameWs);
        }

        public async Task<HttpResponseMessage> Save(String ticket)
        {
            IRutinaCliente rutina = ServiceBuro.Instance.createRutinaCliente();
            var salida = await rutina.saveCliente(ticket);
            return Request.CreateResponse(HttpStatusCode.OK, salida);
        }

        private async Task<HttpResponseMessage> CargaLayoutTask(string usernameWs)
        {
            HttpResponseMessage salida = new HttpResponseMessage();
            IRutinaCliente rutina = ServiceBuro.Instance.createRutinaCliente();
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
                rutina.layout = "CargaCliente";
                rutina.Path = targetFolder;
                BuroService.ClientesResponse response = await rutina.validate();
                if (response.Error)
                {
                    var s = (from a in response.Errores
                             select new
                             {
                                 razonSocial = a.Cliente?.nombre,
                                 rfc = a.Cliente?.rfc,
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
                    salida = Request.CreateResponse(HttpStatusCode.OK, new { error = false, token = response.Token });
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactorWS.model;
using BuroFactorWS.src.model;
using System.Text;
using GenericLinq.Correo;
using BuroFactorWS.src.service;

namespace BuroFactorWS.src.bussines.impl
{
    public class NotificaCrystal : INotificaRiesgo
    {

        public ServidorCorreo ServidorCorreo { get; set; }
        public String CancelacionLink { get; set; }

        private void notificaRiesgo(IGrouping<financiera, deuda> elemento)
        {
            financiera financiera = elemento.Key;
            StringBuilder sb = new StringBuilder();
            foreach (var operacionRiesgosa in elemento.ToList())
            {
                sb.AppendLine("<tr><td>" + operacionRiesgosa.FolioDocumento + "</td>");
                sb.AppendLine("<td>" + operacionRiesgosa.MontoNominal + "</td>");
                sb.AppendLine("<td>" + operacionRiesgosa.MontoFinanciado + "</td>");
                sb.AppendLine("<td>" + operacionRiesgosa.divisa.nombre + "</td>");
                sb.AppendLine("<td>" + CancelacionLink + "?idOperacion=" + operacionRiesgosa.idDeuda + "</td></tr>");

            }
            Dictionary<String, string> datos = new Dictionary<String, string>() {
                        { "RazonSocial", financiera.persona.RazonSocial},
                        { "RFC", financiera.persona.RFC},
                        { "data",sb.ToString()}
                };
            Correo correo = new Correo()
            {
                Asunto = "Registro Buro",
                Authentificacion = ServidorCorreo.Autentificacion,
                correo = new System.Net.Mail.MailAddress(ServidorCorreo.Cuenta_Alias, ServidorCorreo.Nombre_Alias),
                Mensaje = ServiceBuro.Instance.getContenedorPlantillas().getMensaje("Riesgo", datos),
                Host = ServidorCorreo.Host,
                Pass = ServidorCorreo.Contrasena,
                Puerto = ServidorCorreo.Puerto,
                CorreoDestinos = new List<DestinatarioCorreo>() { (new DestinatarioCorreo() { cuenta = financiera.Correo }) }
            };
            correo.enviame();
        }

        public void notificaRiesgos(List<deuda> deudaRiesgo)
        {
            var grouping = deudaRiesgo.GroupBy(p => p.financiera);
            foreach (var elemento in grouping)
                notificaRiesgo(elemento);
        }


    }
}
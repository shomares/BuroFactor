using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace GenericLinq.Correo
{
    public class Correo
    {
        private MailAddress _correo;
        private string _pass;
        private List<DestinatarioCorreo> _correoDestinos;
        private string _host;
        private int _puerto;
        private string _authentificacion;
        private string _mensaje;
        private string _asunto;
        private string _resultado;

        #region Publics

        public MailAddress correo
        {
            get { return _correo; }
            set { _correo = value; }
        }
        public string Pass
        {
            get { return _pass; }
            set { _pass = value; }
        }
        public List<DestinatarioCorreo> CorreoDestinos
        {
            get { return _correoDestinos; }
            set { _correoDestinos = value; }
        }
        public int Puerto
        {
            get { return _puerto; }
            set { _puerto = value; }
        }
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }
        public string Authentificacion
        {
            get { return _authentificacion; }
            set { _authentificacion = value; }
        }
        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }
        public string Asunto
        {
            get { return _asunto; }
            set { _asunto = value; }
        }
        public string Resultado
        {
            get { return _resultado; }
            set { _resultado = value; }
        }
        #endregion

        public List<FileStream> streams;

        public void setCorreoDestion(string correoDestino)
        {
            DestinatarioCorreo des = new DestinatarioCorreo();
            des.cuenta = correoDestino;
            CorreoDestinos.Add(des);
        }

        public void enviarMensaje(FileStream file)
        {
            MailMessage mail = new MailMessage();
            Attachment adjunto;
            SmtpClient mailClient = new SmtpClient();
            NetworkCredential basicAuthenticationInfo = new NetworkCredential();
            try
            {
                mail.From = (correo);
                foreach (var correoD in CorreoDestinos)
                    mail.To.Add(correoD.cuenta);
                mail.Subject = Asunto;
                mail.Body = Mensaje;
                mail.IsBodyHtml = true;

                adjunto = obtenDocumentos(file);
                if (adjunto != null)
                {
                    mail.Attachments.Add(adjunto);
                    basicAuthenticationInfo = new NetworkCredential(correo.Address, Pass);

                    mailClient.Host = Host;
                    mailClient.Port = Puerto;
                    if (Authentificacion != "Null")
                    {
                        mailClient.UseDefaultCredentials = true;
                        mailClient.Credentials = basicAuthenticationInfo;
                    }
                    switch (this.Authentificacion)
                    {
                        case "Normal":
                            mailClient.EnableSsl = false;
                            break;
                        case "SSL":
                            mailClient.EnableSsl = true;
                            break;
                        case "Null":
                            mailClient.EnableSsl = false;
                            break;
                        default:
                            throw new Exception("Error en tipo de autenticacion");

                    }
                    mailClient.Send(mail);
                }
                else
                    throw new Exception("No selecciono un archivo");
            }
            catch (Exception)
            {
                throw;
            }

        }
        public void enviarMensaje()
        {
            MailMessage mail = new MailMessage();
            SmtpClient mailClient = new SmtpClient();
            NetworkCredential basicAuthenticationInfo = new NetworkCredential();

            mail.From = (correo);
            foreach (var correoD in CorreoDestinos)
                mail.To.Add(correoD.cuenta);

            if (streams != null)
            {
                foreach (var file in streams)
                {
                    mail.Attachments.Add(obtenDocumentos(file) ?? default(Attachment));
                }
            }

            mail.Subject = Asunto;
            mail.Body = Mensaje;
            mail.IsBodyHtml = true;

            basicAuthenticationInfo = new NetworkCredential(correo.Address, Pass);

            mailClient.Host = Host;
            mailClient.Port = Puerto;
            if (Authentificacion != "Null")
            {
                mailClient.UseDefaultCredentials = true;
                mailClient.Credentials = basicAuthenticationInfo;
            }
            switch (this.Authentificacion)
            {
                case "Normal":
                    mailClient.EnableSsl = false;
                    break;
                case "SSL":
                    mailClient.EnableSsl = true;
                    break;
                case "Null":
                    mailClient.EnableSsl = false;
                    break;
                default:
                    throw new Exception("Error en tipo de autenticacion");

            }
            mailClient.Send(mail);
     
    }
    public void enviarMensaje(string email, string nombre)
    {
        MailMessage mail = new MailMessage();
        SmtpClient mailClient = new SmtpClient();
        NetworkCredential basicAuthenticationInfo = new NetworkCredential();
        try
        {
            mail.From = new MailAddress(email, nombre);
            foreach (var correoD in CorreoDestinos)
                mail.To.Add(correoD.cuenta);


            if (streams != null)
                foreach (var file in streams)
                    if (obtenDocumentos(file) != null)
                        mail.Attachments.Add(obtenDocumentos(file));

            mail.Subject = Asunto;
            mail.Body = Mensaje;
            mail.IsBodyHtml = true;
            basicAuthenticationInfo = new NetworkCredential(correo.Address, Pass);
            mailClient.Host = Host;
            mailClient.Port = Puerto;
            if (Authentificacion != "Null")
            {
                mailClient.UseDefaultCredentials = true;
                mailClient.Credentials = basicAuthenticationInfo;
            }
            switch (this.Authentificacion)
            {
                case "Normal":
                    mailClient.EnableSsl = false;
                    break;
                case "SSL":
                    mailClient.EnableSsl = true;
                    break;
                case "Null":
                    mailClient.EnableSsl = false;
                    break;
                default:
                    throw new Exception("Error en tipo de autenticacion");

            }
            mailClient.Send(mail);
        }
        catch (Exception)
        {
            throw;
        }

    }
    public Attachment obtenDocumentos(FileStream file)
    {
        string[] nombres;
        nombres = file.Name.Split('\\');
        return new Attachment(file, nombres[nombres.Length - 1]);
    }

    public void enviame()
    {
        try
        {
            Thread t = new Thread(this.enviarMensaje);
            t.Start();
            if (t.Join(5000000)) { }
            else throw new Exception("Supero el tiempo de espera para el envio del archivo (s)");
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void enviaCorreoDestinatarios(List<FileStream> files, string mensaje, string asunto, List<DestinatarioCorreo> correosDestino, string conexion)
    {
        Correo enviar = new Correo();
        if (enviar != null)
        {
            enviar.CorreoDestinos = correosDestino;
            enviar.Asunto = asunto;
            enviar.Mensaje = mensaje;
            enviar.streams = files;
            enviar.enviame();
        }
        else throw new Exception("No hay cuenta configurada en el servidor");
    }
}
}

using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.extension;
using BuroFactor.code.service;
using BuroFactor.Models.dao;
using GenericLinq.Correo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Data.Entity;
using BuroComun.src.security;

namespace BuroFactor.code.bussines
{
    public class ContratoBusiness : IContrato
    {
        public IGeneraContrasena Generador { get; set; }
        public IBuroFactory FactoryEntityFactory { get; set; }
        public ServidorCorreo ServidorCorreo { get; set; }

        public String Certificate { get; set; }
        public String RutaWSDL { get; set; }
        public String RutaTOKEN { get; set; }

        public void registraContrato(plancontratado plan)
        {

            try
            {

                using (burofactorEntities FactorEntity = FactoryEntityFactory.BeginTrasanction())
                {
                    try
                    {

                        plancontratado nuevoPlan = new plancontratado()
                        {
                            Serial = Generador.generaPassword(),
                            Activo = false,
                            Financiera_idFinanciera = plan.financiera.idFinanciera,
                            PlanConsulta_idPlanConsulta = plan.planconsulta.idPlanConsulta

                        };
                        nuevoPlan.FechaContrato = DateTime.Now;
                        nuevoPlan.Serial = Generador.generaPassword();
                        nuevoPlan.Activo = false;
                      

                        Dictionary < String, string> datos = new Dictionary<String, string>() {
                        { "RazonSocial", plan.financiera.persona.RazonSocial},
                        { "RFC", plan.financiera.persona.RFC},
                        { "link", RutaTOKEN +  "?Token=" + nuevoPlan.Serial}};




                        Correo correo = new Correo()
                        {
                            Asunto = "Registro Buro",
                            Authentificacion = ServidorCorreo.Autentificacion,
                            correo = new System.Net.Mail.MailAddress(ServidorCorreo.Cuenta_Alias, ServidorCorreo.Nombre_Alias),
                            Mensaje = ServiceBuro.Instance.getContenedorPlantillas().getMensaje("registraContrato", datos),
                            Host = ServidorCorreo.Host,
                            Pass = ServidorCorreo.Contrasena,
                            Puerto = ServidorCorreo.Puerto,
                            CorreoDestinos = new List<DestinatarioCorreo>() { (new DestinatarioCorreo() { cuenta = plan.financiera.Correo }) }
                        };


                        nuevoPlan.UsuarioWS = "";
                        nuevoPlan.SalWS = "";
                        nuevoPlan.ContrasenaWS = "";

                        FactorEntity.plancontratado.Add(nuevoPlan);
                        FactorEntity.SaveChanges();
                        correo.enviame();
                        FactorEntity.Database.CurrentTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        FactorEntity.Database.CurrentTransaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void altaContrato(plancontratado plan)
        {

            String salt = Generador.generaAleatorio();
            String pass = Generador.generaPassword();
            String aleatorio = Generador.generaAleatorio();

            try
            {

                using (burofactorEntities FactorEntity = FactoryEntityFactory.BeginTrasanction())
                {
                    try
                    {
                        plancontratado planOriginal = FactorEntity.plancontratado.Find(plan.idPlanContratado);
                        planOriginal.UsuarioWS = plan.financiera.persona.RFC + aleatorio;
                        planOriginal.ContrasenaWS = Generador.generaPassword(salt, pass);
                        planOriginal.SalWS = salt;
                        planOriginal.Activo = true;
                        planOriginal.FechaContrato = DateTime.Now;
                        var lista = FactorEntity.plancontratado.Where(aux => aux.Financiera_idFinanciera == planOriginal.Financiera_idFinanciera && aux.Activo == true).ToList();
                        //Actualizamos 
                        foreach (var elemento in lista)
                        {
                            elemento.Activo = false;
                            FactorEntity.Entry(elemento).State = System.Data.Entity.EntityState.Modified;
                        }

                        FactorEntity.Entry(planOriginal).State = System.Data.Entity.EntityState.Modified;


                        Dictionary<String, string> datos = new Dictionary<String, string>() {
                         { "RazonSocial", plan.financiera.persona.RazonSocial},
                        { "RFC", plan.financiera.persona.RFC},
                        { "Usuario", planOriginal.UsuarioWS},
                        { "Pass", pass},
                        { "Contrato", planOriginal.planconsulta.Nombre},
                        { "ConsultasMes", planOriginal.planconsulta.MaxConsultaMes.ToString()},
                        { "Ruta",RutaWSDL},

                        };

                        List<FileStream> adjuntos = new List<FileStream>();

                        adjuntos.Add(new FileStream(Certificate, FileMode.Open));

                        Correo correo = new Correo()
                        {
                            Asunto = "Alta Plan",
                            Authentificacion = ServidorCorreo.Autentificacion,
                            correo = new System.Net.Mail.MailAddress(ServidorCorreo.Cuenta_Alias, ServidorCorreo.Nombre_Alias),
                            Mensaje = ServiceBuro.Instance.getContenedorPlantillas().getMensaje("altaplan", datos),
                            Host = ServidorCorreo.Host,
                            Pass = ServidorCorreo.Contrasena,
                            Puerto = ServidorCorreo.Puerto,
                            CorreoDestinos = new List<DestinatarioCorreo>() { (new DestinatarioCorreo() { cuenta = planOriginal.financiera.Correo }) },
                            streams = adjuntos

                        };

                        FactorEntity.SaveChanges();
                        correo.enviame();

                        foreach (var elemento in adjuntos)
                        {
                            elemento.Close();
                            elemento.Dispose();
                        }
                        FactorEntity.Database.CurrentTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        FactorEntity.Database.CurrentTransaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public plancontratado getPlanContratado(int idFinanciera, int idPlan)
        {
            plancontratado salida = null;
            try
            {

                using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
                {
                    salida = new plancontratado();
                    var financieras = FactorEntity.financiera.Include(f => f.persona);
                    salida.financiera = financieras.Where(aux => aux.idFinanciera == idFinanciera).FirstOrDefault();
                    salida.planconsulta = FactorEntity.planconsulta.Find(idPlan);
                }
                return salida;
            }
            catch (Exception)
            {
                throw;

            }
        }

        public plancontratado getPlanContratado(string hash)
        {
            plancontratado salida = null;
            try
            {

                using (burofactorEntities FactorEntity = FactoryEntityFactory.BeginTrasanction())
                {
                    salida = (from aux in FactorEntity.plancontratado
                              where aux.Serial == hash
                              select aux).FirstOrDefault();

                    return salida;

                }
            }
            catch (Exception)
            {
                throw;

            }
        }

        public void creaUsuarioFinanciera(financiera financiera, string contrasena)
        {

            using (burofactorEntities FactorEntity = FactoryEntityFactory.BeginTrasanction())
            {
                try
                {
                    usuario user = new usuario()
                    {
                        Financiera_idFinanciera = financiera.idFinanciera,
                        username = financiera.persona.RFC,
                        activo = true
                    };

                    String salt = Generador.generaAleatorio();

                    String passHash = Generador.generaPassword(salt, contrasena);

                    user.aleatorio = salt;
                    user.password = passHash;
                    user.salt = salt;

                    usuario userAntiguo = (from aux in FactorEntity.usuario
                                           where aux.username == user.username && aux.activo == true
                                           select aux).FirstOrDefault();

                    if (userAntiguo != null)
                    {
                        userAntiguo.activo = false;
                        FactorEntity.Entry(userAntiguo).State = System.Data.Entity.EntityState.Modified;
                    }
                    FactorEntity.usuario.Add(user);
                    FactorEntity.SaveChanges();
                    FactorEntity.Database.CurrentTransaction.Commit();

                }
                catch (Exception ES)
                {
                    FactorEntity.Database.CurrentTransaction.Rollback();
                    throw;
                }
            }
        }


    }
}
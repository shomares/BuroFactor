using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroComun.src.model.cargas;
using BuroFactorWS.model;
using System.Data.Entity;

namespace BuroFactorWS.src.dao.impl
{
    public class DaoBuroEF : IDaoBuro
    {

        public IBuroFactory Factory { get; set; }
        private burofactorEntities Database { get; set; }

        public void beginTrasaction()
        {
            Database = Factory.BeginTrasanction();
        }
        public void commitTrasaction()
        {
            Database.SaveChanges();
            Database.Database.CurrentTransaction.Commit();
            Database.Dispose();
            Database = null;
        }
        public void rollback()
        {
            Database.Database.CurrentTransaction.Rollback();
            Database.Dispose();
            Database = null;
        }


        public void borraTicket(ticket ticket)
        {
            if (Database != null)
                Database.ticket.Remove(ticket);
        }

        public ticket getTicket(string ticket)
        {
            ticket salida = null;
            if (Database != null)
            {
                salida = (from a in Database.ticket.Include(f => f.plancontratado)
                         .Include(x => x.plancontratado.financiera)
                         .Include(s => s.plancontratado.financiera.persona)
                          where a.Serial == ticket
                          select a).FirstOrDefault();
            }
            return salida;
        }

        public void registraTicket(ticket ticket)
        {
            Database.ticket.Add(ticket);
        }


        public financiera getFinancieraPorContratoWS(string user)
        {
            financiera salida = null;
            if (Database != null)
            {
                salida = (from a in Database.plancontratado.Include(f => f.financiera).Include(p => p.financiera.persona)
                          where a.UsuarioWS == user && a.Activo && a.planconsulta.FechaVencimiento > DateTime.Now
                          select a.financiera
                         ).FirstOrDefault();
            }
            return salida;
        }

        public relacionclientefinanciera getRelacion(string rfc, string idInterno, financiera financiera)
        {
            relacionclientefinanciera relacion = null;
            if (Database != null)
            {

                relacion = (from a in Database.relacionclientefinanciera.Include(f => f.persona)
                            where a.financiera.idFinanciera == financiera.idFinanciera &&
                            a.persona.RFC == rfc && idInterno == a.Alias
                            select a).FirstOrDefault();
            }
            return relacion;
        }

        public bool validaInterno(String alias, financiera financiera)
        {

            bool salida = false;
            if (Database != null)
            {
                salida = (from a in Database.relacionclientefinanciera
                          where a.Alias == alias && financiera.idFinanciera == financiera.idFinanciera
                          select a).Any();
            }
            return salida;
        }

        public void registraCliente(ClienteCarga elemento, financiera financiera)
        {
            relacionclientefinanciera relacion = null;
            persona persona = null;
            if (!validaRegistroCliente(elemento.rfc))
            {

                persona = new persona()
                {
                    FechaConstitucion = elemento.fechaConstitucion,
                    RazonSocial = elemento.nombre,
                    RFC = elemento.rfc
                };
                relacion = getRelacion(elemento.rfc, elemento.idInterno, financiera);

                if (!validaInterno(elemento.idInterno, financiera))
                {
                    if (relacion == null)
                    {
                        Database.relacionclientefinanciera.Add(new relacionclientefinanciera()
                        {
                            Alias = elemento.idInterno,
                            CorreoContacto = elemento.correo,
                            Financiera_idFinanciera = financiera.idFinanciera,
                            persona = persona
                        });
                    }
                }
                else
                {
                    throw new Exception("El identificador ya fue registrado");
                }

            }
            else
            {
                persona = getPersona(elemento.rfc);
                relacion = getRelacion(elemento.rfc, elemento.idInterno, financiera);
                if (relacion == null)
                {
                    Database.relacionclientefinanciera.Add(new relacionclientefinanciera()
                    {
                        Alias = elemento.idInterno,
                        CorreoContacto = elemento.correo,
                        Financiera_idFinanciera = financiera.idFinanciera,
                        Persona_idPersona = persona.idPersona
                    });
                }



            }
        }

        private persona getPersona(string rfc)
        {
            persona salida = null;
            if (Database != null)
            {
                salida = (from a in Database.persona
                          where a.RFC == rfc
                          select a).FirstOrDefault();
            }
            return salida;
        }

        public bool validaRegistroCliente(string rfc)
        {
            bool salida = false;
            if (Database != null)
            {
                salida = (from a in Database.persona
                          where a.RFC == rfc
                          select a).Any();
            }
            return salida;

        }
    }
}
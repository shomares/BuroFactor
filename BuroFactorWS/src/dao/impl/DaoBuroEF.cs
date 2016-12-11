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

        public IDaoBuro createContext()
        {
            return (IDaoBuro)this.MemberwiseClone();
        }

        public relacionclientefinanciera getEmisora(string idEmisor, financiera financiera)
        {
            relacionclientefinanciera salida = null;
            if (Database != null)
                salida = Database.relacionclientefinanciera.Where(aux => aux.Alias == idEmisor && aux.Financiera_idFinanciera == financiera.idFinanciera).FirstOrDefault();
            return salida;

        }

        public divisa getDivisa(string divisa)
        {
            divisa salida = null;
            if (Database != null)
                salida = Database.divisa.Where(aux => aux.nombre == divisa).FirstOrDefault();

            return salida;
        }

        public List<financiera> getFinancieraConRiesgo(OperacionCarga elemento, financiera financiera)
        {
            List<financiera> salida = null;
            if (Database != null)
            {
                salida = (from aux in Database.deuda
                              //El segundo elemento a observar si un documento es parecido a alguna deuda
                              //Es decir que sea del mismo emisor
                              //Y que no haya sido pagado
                          where
                          (
                             aux.FolioDocumento == elemento.Folio &&
                            (aux.MontoNominal - elemento.MontoNominal) <= (decimal).5 &&
                            aux.relacionclientefinanciera.Persona_idPersona == getEmisora(elemento.idProveedor, financiera).Persona_idPersona &&
                            aux.FechaEmision == aux.FechaEmision &&
                            !aux.operacion.Where(p => p.Tipo == "P" && p.idDeuda == aux.idDeuda).Any()
                          )
                          //El segundo elemento a observar si un documento es parecido a alguna deuda
                          //Es decir que sea del mismo emisor
                          //Y que no haya sido pagado
                          ||
                          (
                            aux.FolioDocumento == elemento.Folio &&
                            (aux.MontoNominal - elemento.MontoNominal) <= (decimal).5 &&
                            aux.relacionclientefinanciera1.Persona_idPersona == getEmisora(elemento.idEmisor, financiera).Persona_idPersona &&
                            aux.FechaEmision == aux.FechaEmision &&
                            !aux.operacion.Where(p => p.Tipo == "P" && p.idDeuda == aux.idDeuda).Any()
                          )
                          select aux.relacionclientefinanciera.financiera
                        ).ToList();

            }

            return salida;

        }

        public void registraOperacion(OperacionCarga elemento, financiera financiera)
        {
            if (Database != null)
            {
                deuda deuda = new deuda()
                {

                    FechaEmision = elemento.FechaEmision,
                    FechaOperacion = DateTime.Now,
                    FolioDocumento = elemento.Folio,
                    FechaPago = elemento.fechaPago,
                    MontoNominal = elemento.MontoNominal,
                    MontoFinanciado = elemento.MontoFinanciado
                };

                deuda.Divisa_idDivisa = (from aux in Database.divisa
                                         where aux.nombre == elemento.Divisa
                                         select aux.idDivisa).FirstOrDefault();

                deuda.idDeudor = (from aux in Database.relacionclientefinanciera
                                  where aux.Financiera_idFinanciera == financiera.idFinanciera
                                  && aux.Alias == elemento.idProveedor
                                  select aux.idRelacionClienteFinanciera).FirstOrDefault();
                deuda.idEmisor = (from aux in Database.relacionclientefinanciera
                                  where aux.Financiera_idFinanciera == financiera.idFinanciera
                                  && aux.Alias == elemento.idEmisor
                                  select aux.idRelacionClienteFinanciera).FirstOrDefault();

                deuda.Financiera_idFinanciera = financiera.idFinanciera;



                if (deuda.Divisa_idDivisa > 0 && deuda.idDeudor > 0 && deuda.idEmisor > 0)
                    Database.deuda.Add(deuda);
                else
                    throw new Exception("Falla de validacion");
            }
        }

        public void Dispose()
        {
            if (this.Database != null)
                this.Database.Dispose();
        }

        public deuda getDeuda(string folio, string idEmisor, financiera financiera)
        {
            deuda salida = null;
            if (Database != null)
                salida = (from aux in Database.deuda.Include(s => s.operacion)
                          where aux.relacionclientefinanciera.Alias == idEmisor
                          && aux.Financiera_idFinanciera == financiera.idFinanciera
                          && aux.FolioDocumento == folio
                          select aux).FirstOrDefault();

            return salida;
        }

        public void registraEstado(operacion operacion)
        {
            if (Database != null)
                Database.operacion.Add(operacion);
        }

        public relacionclientefinanciera getCliente(string rfc, financiera financiera)
        {
            relacionclientefinanciera salida = null;

            if (Database != null)
            {
                salida = (from aux in Database.relacionclientefinanciera
                          where aux.persona.RFC.Equals(rfc) && aux.financiera.idFinanciera == financiera.idFinanciera
                          select aux).FirstOrDefault();
            }
            return salida;

        }

        public List<deuda> getDeudasRiesgo(OperacionCarga elemento, financiera financiera)
        {
            List<deuda> deudas = null;
            relacionclientefinanciera emisora = getEmisora(elemento.idEmisor, financiera);
            if (Database != null && emisora!=null)
            {
                deudas = (from p in Database.deuda
                          where
                            (p.FolioDocumento == elemento.Folio
                            && p.Financiera_idFinanciera != financiera.idFinanciera
                            && p.relacionclientefinanciera.Persona_idPersona == emisora.Persona_idPersona
                            )
                          select p
                            ).ToList();
                if (deudas.Count == 0)
                    deudas = Database.deuda.Where(p =>
                Math.Abs(p.MontoNominal - elemento.MontoNominal) < (decimal).5
                && p.FechaEmision == elemento.FechaEmision
                && p.Financiera_idFinanciera != financiera.idFinanciera
                && p.relacionclientefinanciera1.Persona_idPersona == emisora.Persona_idPersona).ToList();
            }

            return deudas;

        }

        public bool validaRelacion(string rfc, financiera financiera)
        {
            bool salida = false;
            if (Database != null)
            {
                salida = (from aux in Database.relacionclientefinanciera
                          where aux.persona.RFC == rfc
                          && aux.financiera.idFinanciera == financiera.idFinanciera
                          select aux).Any();

            }
            return salida;
        }

        public persona getCliente(string rFC)
        {
            persona persona = null;
            if (Database != null)
            {
                persona = (from aux in Database.persona
                           where aux.RFC == rFC
                           select aux).FirstOrDefault();
            }
            return persona;
        }

        public plancontratado getPlanContratado(string user)
        {
            plancontratado plan = null;
            if (Database != null)
            {
                plan = (from aux in Database.plancontratado
                        where aux.Activo && aux.UsuarioWS == user
                        select aux).FirstOrDefault();
            }
            return plan;
        }

        public List<deuda> getDeudas(string rFC)
        {
            throw new NotImplementedException();
        }

        public void registraConsultaBuro(consulta consulta)
        {
            if (Database != null)
                Database.consulta.Add(consulta);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroComun.src.model.cargas;
using BuroFactorWS.src.response;
using BuroFactorWS.src.dao;
using Data.XML.Validacion;
using BuroComun.src.security;
using BuroFactorWS.model;
using BuroFactorWS.src.model;

namespace BuroFactorWS.src.bussines.impl
{
    public class RutinaOperacion : IRutinaOperacion
    {

        public IDaoBuro DaoBuroProvider { get; set; }
        public IValidaEntrada<OperacionCarga> Validador { get; set; }
        public ISerialize Serialize { get; set; }
        public INotificaRiesgo Notifica { get; set; }


        public BuroResponse ConsultaBuro(String RFC, string user, string IP)
        {
            BuroResponse salida = new BuroResponse();
            using (IDaoBuro daoBuro = DaoBuroProvider.createContext())
            {
                try
                {
                    daoBuro.beginTrasaction();
                    persona persona = daoBuro.getCliente(RFC);
                    plancontratado plan = daoBuro.getPlanContratado(user);
                    List<consulta> consultaMes = null;
                    if (plan != null)
                    {
                        consultaMes = plan.consulta.Where(p => p.FechaConsulta.Month == DateTime.Now.Month).ToList();
                        if (plan.planconsulta.Ilimitado || plan.planconsulta.MaxConsultaMes - consultaMes.Count > 0)
                            if (persona != null)
                            {
                                salida.FechaConstitucion = persona.FechaConstitucion;
                                salida.RazonSocial = persona.RazonSocial;
                                salida.RFC = persona.RFC;

                                foreach (var relacion in persona.relacionclientefinanciera)
                                {
                                    RelacionFinancieraResponse response = new RelacionFinancieraResponse()
                                    {
                                        Financiera = new FinancieraBuroResponse()
                                        {
                                            FechaConstitucion = relacion.financiera.persona.FechaConstitucion,
                                            RazonSocial = relacion.financiera.persona.RazonSocial,
                                            RFC = relacion.financiera.persona.RFC
                                        }

                                    };
                                    foreach (var deuda in relacion.deuda)
                                    {
                                        OperacionesResponse operacion = new OperacionesResponse()
                                        {
                                            Divisa = deuda.divisa.nombre,
                                            FechaEmision = deuda.FechaEmision,
                                            FechaVencimiento = deuda.FechaPago,
                                            Folio = deuda.FolioDocumento,
                                            MontoFinanciado = deuda.MontoFinanciado,
                                            MontoNominal = deuda.MontoNominal,
                                        };

                                        foreach (var historial in deuda.operacion)
                                        {
                                            HistorialOperacionResponse historialR = new HistorialOperacionResponse()
                                            {
                                                Fecha = historial.Fecha,
                                                Tipo = historial.Tipo
                                            };
                                            operacion.Historial.Add(historialR);
                                        }
                                        response.Operaciones.Add(operacion);
                                    }
                                    salida.Relacion.Add(response);
                                }

                                consulta consulta = new consulta()
                                {
                                    FechaConsulta = DateTime.Now,
                                    Interno = false,
                                    IP = IP,
                                    PlanContratado_idPlanContratado = plan.idPlanContratado
                                };
                                daoBuro.registraConsultaBuro(consulta);
                                daoBuro.commitTrasaction();
                            }
                    }
                }
                catch (Exception)
                {
                    daoBuro.rollback();
                    throw;
                }
            }
            return salida;
        }


        public OperacionResponse ValidaOperaciones(List<OperacionCarga> lista, string user)
        {
            IValidaEntrada<OperacionCarga> validadorS = Validador.clone();
            OperacionResponse salida = new OperacionResponse();

            validadorS.errores.Clear();
            using (IDaoBuro daoBuro = DaoBuroProvider.createContext())
            {
                validadorS.validaEntrada(lista);

                try
                {
                    daoBuro.beginTrasaction();

                    if (validadorS.errores.Count == 0)
                    {
                        financiera financiera = daoBuro.getFinancieraPorContratoWS(user);
                        if (financiera != null)
                        {
                            foreach (var elemento in lista)
                            {
                                List<deuda> deudaRiesgo = new List<deuda>();
                                relacionclientefinanciera relacionEmisor, relacionProveedor;
                                divisa divisa;
                                deuda deudaR = null;
                                relacionEmisor = daoBuro.getEmisora(elemento.idEmisor, financiera);
                                relacionProveedor = daoBuro.getEmisora(elemento.idProveedor, financiera);
                                divisa = daoBuro.getDivisa(elemento.Divisa);
                                deudaR = daoBuro.getDeuda(elemento.Folio, elemento.idEmisor, financiera);

                                if (deudaR != null)
                                {
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Operacion = elemento,
                                        Error = "La operacion ya fue registrada con anterioridad"
                                    });
                                }

                                if (relacionEmisor == null)
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Operacion = elemento,
                                        Error = "El emisor no tiene ninguna relacion con la financiera"
                                    });
                                if (relacionProveedor == null)
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Operacion = elemento,
                                        Error = "El proveedor no tiene ninguna relacion con la financiera"
                                    });

                                if (divisa == null)
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Operacion = elemento,
                                        Error = "No existe la divisa: " + elemento.Divisa + " registrada en el sistema"
                                    });
                                deudaRiesgo = daoBuro.getDeudasRiesgo(elemento, financiera);
                                if (deudaRiesgo != null && deudaRiesgo.Count > 0)
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Operacion = elemento,
                                        Error = "Se detecto un riesgo",
                                        isRiesgo = true
                                    });

                            }
                            if (salida.Errores.Count == 0 || salida.Errores.Where(aux => aux.isRiesgo).ToList().Count == salida.Errores.Count)
                            {
                                ticket Ticket = new ticket()
                                {
                                    Fecha = DateTime.Now,
                                    PlanContratado_idPlanContratado = (from aux in financiera.plancontratado
                                                                       where aux.FechaContrato < DateTime.Now &&
                                                                       aux.Activo && aux.planconsulta.FechaVencimiento > DateTime.Now
                                                                       select aux).FirstOrDefault().idPlanContratado,
                                    Data = Serialize.Serialize(lista),
                                    Serial = Guid.NewGuid().ToString()

                                };
                                salida.Error = false;
                                daoBuro.registraTicket(Ticket);
                                salida.Token = Ticket.Serial;
                            }
                            else
                                salida.Error = true;
                        }
                    }
                    else
                    {

                        foreach (var elemento in validadorS.errores)
                        {
                            salida.Errores.Add(new ErroresOperacionResponse()
                            {
                                Operacion = elemento.objeto,
                                Error = elemento.notficacion
                            });

                        }
                    }
                    daoBuro.commitTrasaction();
                }
                catch (Exception)
                {
                    daoBuro.rollback();
                    throw;
                }



            }
            return salida;
        }

        public OperacionResponse RegistraOperaciones(string ticket, string user)
        {
            OperacionResponse salida = new OperacionResponse();
            List<deuda> deudaRiesgo = new List<deuda>();

            using (IDaoBuro DaoBuro = DaoBuroProvider.createContext())
            {
                DaoBuro.beginTrasaction();
                ticket Ticket = DaoBuro.getTicket(ticket);
                List<OperacionCarga> lista;
                if (Ticket != null && Ticket.plancontratado.UsuarioWS.Equals(user))
                {
                    if ((DateTime.Now - Ticket.Fecha).Minutes < 10)
                    {
                        lista = (List<OperacionCarga>)Serialize.Derialize(Ticket.Data);
                        if (lista != null)
                        {

                            try
                            {
                                foreach (var elemento in lista)
                                {
                                    List<deuda> deuRies = DaoBuro.getDeudasRiesgo(elemento, Ticket.plancontratado.financiera);
                                    if (deuRies != null)
                                        deudaRiesgo.AddRange(deuRies);
                                    DaoBuro.registraOperacion(elemento, Ticket.plancontratado.financiera);
                                }
                                //Se detectaron riesgos--------------------
                                if (deudaRiesgo.Count > 0)
                                    Notifica.notificaRiesgos(deudaRiesgo);
                                DaoBuro.borraTicket(Ticket);
                                DaoBuro.commitTrasaction();
                                salida.Error = false;
                            }
                            catch (Exception)
                            {
                                DaoBuro.rollback();
                                throw;
                            }


                        }
                        else
                        {
                            throw new Exception("Error de serializacion");
                        }
                    }
                    else
                    {
                        try
                        {
                            DaoBuro.borraTicket(Ticket);
                            DaoBuro.commitTrasaction();
                            throw new Exception("Ticket Expirado");
                        }
                        catch (Exception)
                        {
                            DaoBuro.rollback();
                            throw;
                        }
                    }
                }
                else
                {
                    throw new Exception("Prohibido");
                }
            }
            return salida;
        }

        public OperacionResponse CambiaEstado(List<CambiaEstadoOperacionRequest> estados, string user)
        {
            OperacionResponse salida = new OperacionResponse();
            using (IDaoBuro DaoBuro = DaoBuroProvider.createContext())
            {
                DaoBuro.beginTrasaction();
                try
                {
                    financiera financiera = DaoBuro.getFinancieraPorContratoWS(user);
                    if (financiera != null)
                    {
                        foreach (var estado in estados)
                        {
                            deuda deuda = DaoBuro.getDeuda(estado.Folio, estado.IdEmisor, financiera);
                            if (deuda != null)
                            {
                                //Si no esta cancelada--------------------------
                                if (!deuda.operacion.Where(p => p.Tipo == "C").Any() && (estado.Estado == "C" || estado.Estado == "P"))
                                {
                                    operacion operacion = new operacion()
                                    {
                                        idDeuda = deuda.idDeuda,
                                        Fecha = DateTime.Now,
                                        Tipo = estado.Estado
                                    };
                                    DaoBuro.registraEstado(operacion);
                                    salida.Error = false;
                                }
                                else
                                {
                                    salida.Errores.Add(new ErroresOperacionResponse()
                                    {
                                        Error = "El estado no es valido"
                                    });
                                    DaoBuro.rollback();
                                    break;
                                }
                            }
                            else
                            {

                                salida.Errores.Add(new ErroresOperacionResponse()
                                {
                                    Error = "La deuda no encuentra registrada"
                                });
                                DaoBuro.rollback();
                                break;
                            }
                        }

                        if (salida.Errores.Count == 0)
                            DaoBuro.commitTrasaction();
                    }
                }
                catch (Exception)
                {
                    DaoBuro.rollback();
                    throw;
                }
                return salida;
            }




        }
    }
}
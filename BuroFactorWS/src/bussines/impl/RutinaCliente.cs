using BuroComun.src.model.cargas;
using BuroComun.src.security;
using BuroFactorWS.model;
using BuroFactorWS.src.dao;
using BuroFactorWS.src.response;
using Data.XML.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactorWS.src.bussines
{
    public class RutinaCliente : IRutinaCliente
    {

        public IDaoBuro DaoBuroProvider { get; set; }
        public IValidaEntrada<ClienteCarga> Validador { get; set; }
        public ISerialize Serialize { get; set; }

        public ClientesResponse ValidaClientes(List<ClienteCarga> lista, String user)
        {
            IValidaEntrada<ClienteCarga> validadorS = Validador.clone();
            ClientesResponse salida = new ClientesResponse();
            using (IDaoBuro DaoBuro = DaoBuroProvider.createContext())
            {

                validadorS.errores = new List<Errores<ClienteCarga>>();
                DaoBuro.beginTrasaction();

                try
                {
                    financiera financiera = DaoBuro.getFinancieraPorContratoWS(user);

                    if (financiera != null)
                    {
                        validadorS.validaEntrada(lista);
                        if (validadorS.errores.Count == 0)
                        {
                            foreach (var elemento in lista)
                            {
                                relacionclientefinanciera relacion = DaoBuro.getRelacion(elemento.rfc, elemento.idInterno, financiera);
                                if (relacion != null)
                                {
                                    salida.Errores.Add(new ErroresClientesResponse()
                                    {
                                        Cliente = elemento,
                                        Error = "El cliente ya fue registrado con la financiera"
                                    });
                                }
                                if (DaoBuro.validaInterno(elemento.idInterno, financiera))
                                {
                                    salida.Errores.Add(new ErroresClientesResponse()
                                    {
                                        Cliente = elemento,
                                        Error = "El cliente ya registro el identificador"
                                    });
                                }
                                if (DaoBuro.validaRelacion(elemento.rfc, financiera)) {
                                    salida.Errores.Add(new ErroresClientesResponse()
                                    {
                                        Cliente = elemento,
                                        Error = "El cliente ya tiene una relacion con la financiera"
                                    });
                                }
                            }
                        }
                        else
                        {
                            foreach (var elemento in validadorS.errores)
                            {
                                salida.Errores.Add(new ErroresClientesResponse()
                                {
                                    Cliente = elemento.objeto,
                                    Error = elemento.notficacion
                                });

                            }
                            DaoBuro.commitTrasaction();

                        }

                    }
                    if (salida.Errores.Count == 0)
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
                        DaoBuro.registraTicket(Ticket);
                        salida.Token = Ticket.Serial;
                    }
                    else
                        salida.Error = true;

                    DaoBuro.commitTrasaction();
                }
                catch (Exception)
                {
                    DaoBuro.rollback();
                    throw;
                }
                finally
                {
                    validadorS.Dispose();
                }
            }
            return salida;
        }

        public ClientesResponse RegistraCliente(String ticket, String user)
        {

            ClientesResponse salida = new ClientesResponse();
            using (IDaoBuro DaoBuro = DaoBuroProvider.createContext())
            {
                DaoBuro.beginTrasaction();
                ticket Ticket = DaoBuro.getTicket(ticket);
                List<ClienteCarga> lista;
                if (Ticket != null && Ticket.plancontratado.UsuarioWS.Equals(user))
                {
                    if ((DateTime.Now - Ticket.Fecha).Minutes < 10)
                    {
                        lista = (List<ClienteCarga>)Serialize.Derialize(Ticket.Data);

                        if (lista != null)
                        {

                            try
                            {
                                foreach (var elemento in lista)
                                    DaoBuro.registraCliente(elemento, Ticket.plancontratado.financiera);

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

        public ClientesResponse EditaCliente(CambiaClienteRequest editar, string user)
        {
            ClientesResponse salida = new ClientesResponse();
            using (IDaoBuro DaoBuro = DaoBuroProvider.createContext())
            {
                DaoBuro.beginTrasaction();
                try
                {
                    financiera financiera = DaoBuro.getFinancieraPorContratoWS(user);
                    if (financiera != null)
                    {
                        relacionclientefinanciera cliente = DaoBuro.getCliente(editar.RFC, financiera);
                        if (cliente != null)
                        {
                            cliente.CorreoContacto = editar.Email;
                            cliente.Alias = editar.Alias;
                            DaoBuro.commitTrasaction();
                            salida.Error = false;
                        }
                    }
                }
                catch (Exception)
                {
                    DaoBuro.rollback();
                    throw;
                }
            }
            return salida;
        }
    }
}
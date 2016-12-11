using BuroFactor.code.bussines.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactor.BuroService;
using System.Threading.Tasks;
using ThreadExecutor.thread;
using BuroFactor.code.dao;
using BuroComun.src.model.cargas;
using Data.XML.Validacion;
using Report.Excel;
using BuroFactor.code.service;
using System.IO;
using BuroFactor.Areas.Operacion.Models;

namespace BuroFactor.code.bussines.operacion
{
    public class RutinaOperacion : IRutinaOpenacion
    {
        public string layout { get; set; }
        public string NombreExcel { get; set; }
        public string RutaExcel { get; set; }
        public string UsernameWs { get; set; }
        public string Path { get; set; }
        public IDaoCore CargaClienteProvider { get; set; }
        private string token { get; set; }
        private bool busy { set; get; }
        private string name { get; set; }

        public bool isBusy
        {
            get
            {
                return busy;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Result
        {
            get
            {
                return "";
            }

            set
            {

            }
        }

        public event EventIRunnable.OnAvance onAvance;

        public Task<OperacionResponse> saveCliente(String ticket)
        {
            var task = Task.Run(() =>
            {
                OperacionResponse salida = null;
                using (var cargaClientes = CargaClienteProvider.Create(UsernameWs))
                {
                    salida = cargaClientes.saveOperacion(ticket);
                }
                return salida;
            });

            return task;
        }

        public Task<OperacionResponse> validate()
        {
            var task = Task.Run(() =>
            {
                OperacionResponse salida = null;
                ListaHandler<OperacionCarga> handler;
                List<Errores<OperacionCarga>> errores = new List<Errores<OperacionCarga>>();
                try
                {
                    using (FuncionesExcel lectura = new FuncionesExcel())
                    {
                        lectura.Container = ServiceBuro.Instance.Container;

                        handler = lectura.cargaListadeExcelconHandler(RutaExcel, layout, ref errores, new OperacionCarga());

                        if (errores.Count == 0)
                        {
                            using (var cargaOperaciones = CargaClienteProvider.Create(UsernameWs))
                                salida = cargaOperaciones.validateOperaciones(handler.listaObjetos);
                        }
                        else
                        {
                            var response = new List<ErroresOperacionResponse>();

                            foreach (var elemento in errores)
                            {
                                response.Add(new ErroresOperacionResponse()
                                {
                                    Operacion = elemento.objeto,
                                    Error = elemento.notficacion
                                });
                            }
                            salida = new OperacionResponse()
                            {
                                Errores = response.ToArray(),
                                Error = true
                            };
                        }
                    }

                    if (!salida.Error)
                        this.token = salida.Token;
                    return salida;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (File.Exists(RutaExcel))
                        File.Delete(RutaExcel);
                    if (Directory.Exists(Path))
                        Directory.Delete(Path, true);
                }
            });

            return task;

        }

        public IRutinaOpenacion create()
        {
            return new RutinaOperacion()
            {
                CargaClienteProvider = this.CargaClienteProvider,
            };
        }

        public Result<object> run()
        {
            Result<object> salida = new Result<object>();
            var task = this.saveCliente(this.token);
            task.Wait();
            var result = task.Result;
            if (!result.Error)
            {
                salida.State = States.StateEnum.OK;
                salida.Notificacion = "Se han registrado correctamente los operaciones";
            }
            else
                salida.State = States.StateEnum.ERROR;
            salida.Objeto = result;
            return salida;
        }

        public void kill()
        {

        }

        public void Dispose()
        {

        }

        
    }
}
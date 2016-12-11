using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroFactor.BuroService;
using Data.XML.Validacion;
using BuroComun.src.model.cargas;
using Report.Excel;
using BuroFactor.code.dao;
using BuroFactor.code.bussines.interfaces;
using System.IO;
using BuroFactor.code.service;
using ThreadExecutor.thread;

namespace BuroFactor.code.bussines.operacion
{
    public class RutinaCliente : IRutinaCliente
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

        public Task<ClientesResponse> saveCliente(String ticket)
        {
            var task = Task.Run(() =>
            {
                ClientesResponse salida = null;
                using (var cargaClientes = CargaClienteProvider.Create(UsernameWs))
                {
                    salida = cargaClientes.saveClientes(ticket);
                }
                return salida;
            });

            return task;
        }

        public Task<ClientesResponse> validate()
        {
            var task = Task.Run(() =>
            {
                ClientesResponse salida = null;
                ListaHandler<ClienteCarga> handler;
                List<Errores<ClienteCarga>> errores = new List<Errores<ClienteCarga>>();
                try
                {
                    using (FuncionesExcel lectura = new FuncionesExcel())
                    {
                        lectura.Container = ServiceBuro.Instance.Container;

                        handler = lectura.cargaListadeExcelconHandler(RutaExcel, layout, ref errores, new ClienteCarga());

                        if (errores.Count == 0)
                        {
                            using (var cargaClientes = CargaClienteProvider.Create(UsernameWs))
                                salida = cargaClientes.validateClient(handler.listaObjetos);
                        }
                        else
                        {
                            var response = new List<ErroresClientesResponse>();

                            foreach (var elemento in errores)
                            {
                                response.Add(new ErroresClientesResponse()
                                {
                                    Cliente = elemento.objeto,
                                    Error = elemento.notficacion
                                });
                            }
                            salida = new ClientesResponse()
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

        public IRutinaCliente create()
        {
            return new RutinaCliente()
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
                salida.Notificacion = "Se han registrado correctamente los clientes";
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

using BuroFactor.Areas.Operacion.Models;
using BuroFactor.BuroService;
using BuroFactor.code.bussines.interfaces;
using BuroFactor.code.dao;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ThreadExecutor.thread;

namespace BuroFactor.code.bussines.operacion
{
    public class RutinaCambios : IRutinaCambios
    {
        private bool busy;
        private string name;
        public RUTINACAMBIOSSTATE State { get; set; }
        public IDaoCore CargaClienteProvider { get; set; }
        public List<FacturaModificar> Elementos { get; set; }
        public IBuroQuery Dao { get; set; }



        public string usernameWs { get; set; }
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

        public Task<OperacionResponse> CancelaDeudas()
        {
            IDaoCore dao = CargaClienteProvider.Create(usernameWs);
            var task = Task.Run(() =>
            {
                List<CambiaEstadoOperacionRequest> estados = new List<CambiaEstadoOperacionRequest>();
                foreach (var elemento in Elementos)
                {
                    CambiaEstadoOperacionRequest auxEstado = null;
                    deuda deudaAux = this.Dao.getDeuda(elemento.idFactura);
                    if (deudaAux != null)
                    {
                        auxEstado = new CambiaEstadoOperacionRequest()
                        {
                            Folio = deudaAux.FolioDocumento,
                            IdEmisor = deudaAux.relacionclientefinanciera.Alias
                        };
                        estados.Add(auxEstado);
                    }
                }
                if (estados.Count > 0)
                    return dao.CancelaDeudas(estados);
                else
                    return null;
            });
            return task;
        }

        public Task<OperacionResponse> PagaDeudas()
        {
            IDaoCore dao = CargaClienteProvider.Create(usernameWs);
            var task = Task.Run(() =>
            {
                List<CambiaEstadoOperacionRequest> estados = new List<CambiaEstadoOperacionRequest>();
                foreach (var elemento in Elementos)
                {
                    CambiaEstadoOperacionRequest auxEstado = null;
                    deuda deudaAux = this.Dao.getDeuda(elemento.idFactura);
                    if (deudaAux != null)
                    {
                        auxEstado = new CambiaEstadoOperacionRequest()
                        {
                            Folio = deudaAux.FolioDocumento,
                            IdEmisor = deudaAux.relacionclientefinanciera.Alias
                        };
                        estados.Add(auxEstado);
                    }
                }
                if (estados.Count > 0)
                    return dao.PagaDeudas(estados);
                else
                    return null;
            });
            return task;
        }

        public Result<object> run()
        {
            Result<object> salida = new Result<object>();
            try
            {
                OperacionResponse result = null;
                Task<OperacionResponse> task;
                if (State == RUTINACAMBIOSSTATE.CANCELA)
                    task = CancelaDeudas();
                else
                    task = PagaDeudas();

                task.Wait();
                result = task.Result;

                if (result.Error)
                {
                    salida.State = States.StateEnum.ERROR;
                    salida.Objeto = result.Errores;
                    salida.Notificacion = "Surgieron errores al intentar cambiar el estado de la deuda";
                }
                else
                {
                    salida.State = States.StateEnum.OK;
                    salida.Notificacion = "Se actualizaron correctamente las deudas";
                }
            }
            catch (Exception)
            {

                throw;
            }
            return salida;
        }

        public void kill()
        {
        }

        public void Dispose()
        {
        }

        public IRutinaCambios Create()
        {
            return new RutinaCambios() {
                CargaClienteProvider = this.CargaClienteProvider,
                Dao = this.Dao,
                Elementos = new List<FacturaModificar>()
            };
        }
    }
}
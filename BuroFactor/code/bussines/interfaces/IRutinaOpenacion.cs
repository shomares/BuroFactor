using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroFactor.Areas.Operacion.Models;
using BuroFactor.BuroService;
using ThreadExecutor.thread;

namespace BuroFactor.code.bussines.interfaces
{
    public interface IRutinaOpenacion: IRunnable
    {
        string layout { get; set; }
        string NombreExcel { get; set; }
        string Path { get; set; }
        string RutaExcel { get; set; }
        string UsernameWs { get; set; }

        Task<OperacionResponse> validate();
        IRutinaOpenacion create();
    }
}

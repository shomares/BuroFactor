using BuroFactor.Areas.Operacion.Models;
using BuroFactor.BuroService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadExecutor.thread;

namespace BuroFactor.code.bussines.interfaces
{

    public enum RUTINACAMBIOSSTATE { CANCELA, PAGA };

    public interface IRutinaCambios : IRunnable
    {
        Task<OperacionResponse> CancelaDeudas();
        Task<OperacionResponse> PagaDeudas();
        RUTINACAMBIOSSTATE State { get; set; }

        List<FacturaModificar> Elementos { get; set; }
        string usernameWs { get; set; }

        IRutinaCambios Create();


    }
}

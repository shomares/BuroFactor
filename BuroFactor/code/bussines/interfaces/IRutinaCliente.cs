using BuroFactor.BuroService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreadExecutor.thread;

namespace BuroFactor.code.bussines.interfaces
{
    public interface IRutinaCliente : IRunnable
    {
        string RutaExcel { get; set; }
        string NombreExcel { get; set; }
        string UsernameWs { get; set; }
        string layout { get; set; }
        string Path { get; set; }

        Task<ClientesResponse> saveCliente(String ticket);
        Task<ClientesResponse> validate();

        IRutinaCliente create();
    }
}

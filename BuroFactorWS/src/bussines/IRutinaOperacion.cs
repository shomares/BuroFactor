using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroComun.src.model.cargas;
using BuroFactorWS.src.response;

namespace BuroFactorWS.src.bussines.impl
{
    public interface IRutinaOperacion
    {
        OperacionResponse RegistraOperaciones(string ticket, string v);
        OperacionResponse ValidaOperaciones(List<OperacionCarga> lista, string v);
        OperacionResponse CambiaEstado(List<CambiaEstadoOperacionRequest> estados, string v);

        BuroResponse ConsultaBuro(String RFC, string user, string IP);
    }
}

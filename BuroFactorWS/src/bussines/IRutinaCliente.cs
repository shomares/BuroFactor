using BuroComun.src.model.cargas;
using BuroFactorWS.src.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroFactorWS.src.bussines
{
    public interface IRutinaCliente
    {
        ClientesResponse ValidaClientes(List<ClienteCarga> lista, String user);
        ClientesResponse RegistraCliente(String ticket, String user);
        ClientesResponse EditaCliente(CambiaClienteRequest editar, string user);
    }
}

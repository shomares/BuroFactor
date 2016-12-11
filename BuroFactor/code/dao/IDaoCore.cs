using System;
using System.Collections.Generic;
using BuroComun.src.model.cargas;
using BuroFactor.BuroService;

namespace BuroFactor.code.dao
{
    public interface IDaoCore: IDisposable
    {
        IDaoCore Create(string usernameWs);
        ClientesResponse validateClient(List<ClienteCarga> listaObjetos);
        ClientesResponse saveClientes(string ticket);
        OperacionResponse validateOperaciones(List<OperacionCarga> listaObjetos);
        OperacionResponse saveOperacion(string ticket);
        OperacionResponse CancelaDeudas(List<CambiaEstadoOperacionRequest> elementos);
        OperacionResponse PagaDeudas(List<CambiaEstadoOperacionRequest> elementos);

        BuroResponse ConsultaBuro(String RFC);
    }
}
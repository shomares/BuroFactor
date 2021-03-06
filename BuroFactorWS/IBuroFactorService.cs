﻿using BuroComun.src.model.cargas;
using BuroFactorWS.src.response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace BuroFactorWS
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IBuroFactorService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        ClientesResponse ValidaClientes(List<ClienteCarga> lista);

        [OperationContract]
        ClientesResponse RegistraClientes(String ticket);

        [OperationContract]
        BuroResponse ConsultaBuro(String RFC);

        [OperationContract]
        ClientesResponse EditaCliente(CambiaClienteRequest editar);

        [OperationContract]
        OperacionResponse ValidaOperaciones(List<OperacionCarga> lista);

        [OperationContract]
        OperacionResponse RegistraOperaciones(String ticket);

        [OperationContract]
        OperacionResponse CambiaEstado(List<CambiaEstadoOperacionRequest> estados);




        // TODO: Add your service operations here
    }




}

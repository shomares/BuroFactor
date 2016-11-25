using BuroComun.src.model.cargas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BuroFactorWS.src.response
{
    [DataContract]
    public class ClientesResponse
    {
        public ClientesResponse() {
            Errores = new List<ErroresClientesResponse>();
        }

        [DataMember]
        public List<ErroresClientesResponse> Errores { get; set; }
        [DataMember]
        public String Token { get; set; }
        [DataMember]
        public Boolean Error { get; set; }

    }

    [DataContract]
    public class ErroresClientesResponse
    {
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public ClienteCarga Cliente { get; set; }
    }



}
using BuroComun.src.model.cargas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BuroFactorWS.src.response
{
    [DataContract]
    public class ClientesResponse
    {
        public ClientesResponse()
        {
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

    [DataContract]
    public class ErroresOperacionResponse
    {
        public ErroresOperacionResponse()
        {
            isRiesgo = false;
        }

        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public OperacionCarga Operacion { get; set; }

        [DataMember]
        public bool isRiesgo { get; set; }
    }



    [DataContract]
    public class OperacionResponse
    {
        public OperacionResponse()
        {
            Errores = new List<ErroresOperacionResponse>();
        }

        [DataMember]
        public List<ErroresOperacionResponse> Errores { get; set; }
        [DataMember]
        public String Token { get; set; }
        [DataMember]
        public Boolean Error { get; set; }
    }

    [DataContract]
    public class CambiaEstadoOperacionRequest
    {

        [Required]
        [DataMember]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public String Folio { get; set; }

        [Required]
        [DataMember]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        public String Estado { get; set; }

        [Required]
        [RegularExpression(@"^[0-9A-Za-z\u00f1\u00d1\,\/\.\-\ ]{0,100}$")]
        [DataMember]
        public String IdEmisor { get; set; }
    }


    [DataContract]
    public class CambiaClienteRequest
    {
        [Required]
        [DataMember]
        [RegularExpressionAttribute(@"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$")]
        public String RFC { get; set; }

        [DataMember]
        [RegularExpressionAttribute("^[_a-z0-9-]+(.[_a-z0-9-]+)*@[a-z0-9-]+(.[a-z0-9-]+)*(.[a-z]{2,4})$")]
        public String Email { get; set; }

        [DataMember]
        public String Alias { get; set; }


    }

    [DataContract]
    public class BuroResponse
    {
        [DataMember]
        public String RazonSocial { get; set; }
        [DataMember]
        public String RFC { get; set; }
        [DataMember]
        public DateTime FechaConstitucion { get; set; }
        [DataMember]
        public List<RelacionFinancieraResponse> Relacion { get; set; }

        public BuroResponse() {
            Relacion = new List<RelacionFinancieraResponse>();
        }



    }

    [DataContract]
    public class RelacionFinancieraResponse
    {
        [DataMember]
        public FinancieraBuroResponse Financiera { get; set; }
        [DataMember]
        public List<OperacionesResponse> Operaciones { get; set; }

        public RelacionFinancieraResponse()
        {
            Operaciones = new List<OperacionesResponse>();
        }

    }
    [DataContract]
    public class FinancieraBuroResponse
    {
        [DataMember]

        public String RazonSocial { get; set; }
        [DataMember]

        public String RFC { get; set; }
        [DataMember]

        public DateTime FechaConstitucion { get; set; }
    }
    [DataContract]
    public class OperacionesResponse
    {

        public OperacionesResponse() {
            Historial = new List<HistorialOperacionResponse>();

        }
        [DataMember]

        public Decimal MontoNominal { get; set; }
        [DataMember]

        public Decimal MontoFinanciado { get; set; }
        [DataMember]

        public String Folio { get; set; }
        [DataMember]

        public DateTime FechaEmision { get; set; }
        [DataMember]

        public DateTime FechaVencimiento { get; set; }
        [DataMember]

        public String Divisa { get; set; }
        [DataMember]

        public List<HistorialOperacionResponse> Historial { get; set; }

    }
    [DataContract]

    public class HistorialOperacionResponse
    {
        [DataMember]

        public DateTime Fecha { get; set; }
        [DataMember]

        public String Tipo { get; set; }
    }



}
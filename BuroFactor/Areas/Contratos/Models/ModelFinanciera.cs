using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Contratos.Models
{
    public class ModelFinanciera
    {
        public int idPersona { get; set; }
        public string RazonSocial { get; set; }
        public string RFC { get; set; }
        public System.DateTime FechaConstitucion { get; set; }
        public bool Regulada { get; set; }

        public planconsulta Plan { get; set; }
    }
}
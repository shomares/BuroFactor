using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Operacion.Models
{
    public class BuscarModelFactura
    {
        public int? empresa { get; set; }
        public DateTime? FechaIni { get; set; }
        public DateTime? FechaFin { get; set; }
        [StringLength(40)]
        public string Divisa { get; set; }
    }
}
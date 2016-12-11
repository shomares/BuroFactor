using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Reporte.Models
{
    public class ConsultaBuro
    {
        [RegularExpressionAttribute(@"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$")]
        [Required]
        public String RFC { get; set; }
    }
}
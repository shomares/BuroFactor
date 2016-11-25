using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Contratos.Models
{
    public class ModelPlanRegisro
    {
        public Int32 idFinanciera { get; set; }
        public Int32 plan { get; set; }
    }

    public class ModelHashValidacion {
        public String Hash { get; set; }
        public String RFC { get; set; }

    }
}
using BuroComun.src.model.cargas;
using BuroFactorWS.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactorWS.src.model
{
    public class FinancieraRiesgo
    {
        public FinancieraRiesgo()
        {

        }
        public financiera Financiera { get; set; }
        public List<OperacionCarga> Operaciones { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactor.Models.dao;

namespace BuroFactor.Areas.Contratos.Models
{
    public class ModelToken
    {
        public string contrasena { get;  set; }
        public string RFC { get;  set; }
        public string token { get;  set; }
        public string validacionContrasena { get;  set; }
    }
}
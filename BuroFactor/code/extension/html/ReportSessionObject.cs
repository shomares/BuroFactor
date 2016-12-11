using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactor.code.extension.html
{
    [Serializable]
    public class ReportSessionObject
    {
        public string ReportNameType { get; set; }
        public string[] Parametros { get; set; }
        public object[] Datos { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericLinq.Correo
{
    public  class ServidorCorreo
    {
        public string Host { get; set; }

        public int Puerto { get; set; }

        public string Autentificacion { get; set; }

        public string Cuenta { get; set; }

        public string Contrasena { get; set; }

        public string Cuenta_Alias { get; set; }

        public string Nombre_Alias { get; set; }
    }
}

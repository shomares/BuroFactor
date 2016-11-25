using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML.Validacion
{
    public class Errores<T>
    {
        public T objeto { get; set; }
        public string notficacion { get; set; }
        public string id { get; set; }

        public Errores() { }

        public Errores(T objeto, string notificacion)
        {
            this.objeto = objeto;
            this.notficacion = notificacion;

        }
    }
}

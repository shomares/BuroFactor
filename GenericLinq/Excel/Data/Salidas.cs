using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML
{
    public class Salida
    {
        public string valor { get; set; }
        public string opcion { get; set; }

        public String toString()
        {
            return valor;
        }
    }
    public class Salidas
    {
        public Boolean opcional { get; set; }
        public List<Salida> salidas { get; set; }
        public string mapa { get; set; }

        public Salidas()
        {
            salidas = new List<Salida>();
        }
    }
}

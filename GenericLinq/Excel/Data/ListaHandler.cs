using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML.Validacion
{
    public class ListaHandler<T>
    {
        public List<T> listaObjetos { get; set; }

        public XMLHandler<T> handler { set; get; }
    }
}

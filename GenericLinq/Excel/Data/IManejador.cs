using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML.Validacion
{
    public abstract class IManejador<T>
    {
        public List<Errores<T>> errores { set; get; }
        public abstract List<T> manejameEsta(List<T> lista);
    }
}

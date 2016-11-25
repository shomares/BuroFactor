using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.XML;

namespace Data.XML.Validacion
{
    public class XMLHandler<T> : XMLDatosExternos
    {
        public IValidaEntrada<T> validador { set; get; }

        public IManejador<T> manejador { get; set; }

        public void validarDatosLista(List<T> registros)
        {
            validador.validaEntrada(registros);
        }
    }
}

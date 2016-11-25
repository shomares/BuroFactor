using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML.Validacion
{
    public abstract class IValidaEntrada<T>: IDisposable
    {
        public IValidaEntrada()
        {
            errores = new List<Errores<T>>();
        }
        public List<Errores<T>> errores { set; get; }
        public abstract void validaEntrada(List<T> registros);

        public abstract IValidaEntrada<T> clone();

        public abstract void Dispose();
       
    }
}

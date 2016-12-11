using BuroComun.src.model.cargas;
using Data.XML.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.validator
{
    public class ValidadorOperacion : IValidaEntrada<OperacionCarga>
    {
        public override IValidaEntrada<OperacionCarga> clone()
        {
            return (IValidaEntrada<OperacionCarga>)MemberwiseClone();
        }

        public override void Dispose()
        {
            this.errores.Clear();
            this.errores = null;
        }

        public override void validaEntrada(List<OperacionCarga> registros)
        {
            int i = 0;
            foreach (var auxDoc in registros)
            {

                if ((from auxDoc2 in registros
                     where auxDoc.Folio == auxDoc2.Folio && auxDoc.Divisa == auxDoc2.Divisa && auxDoc.idEmisor == auxDoc2.idEmisor
                     select auxDoc2).Count() > 1)
                {
                    this.errores.Add(new Errores<OperacionCarga>(auxDoc, "El cliente se encuentra duplicado en los archivos de carga."));
                }
                i += 1;
            }

        }
    }
}

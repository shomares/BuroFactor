using BuroComun.src.model.cargas;
using Data.XML.Validacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuroComun.src.validator
{
    public class ValidadorCargaClientes : IValidaEntrada<ClienteCarga>
    {
        public override IValidaEntrada<ClienteCarga> clone()
        {
            return (IValidaEntrada<ClienteCarga>)MemberwiseClone();
        }

        public override void Dispose()
        {
            this.errores.Clear();
            this.errores = null;
        }

        public override void validaEntrada(List<ClienteCarga> registros)
        {
            int i = 0;
            foreach (var auxDoc in registros)
            {
                if (auxDoc.fechaConstitucion >= DateTime.Now)
                {
                    this.errores.Add(new Errores<ClienteCarga>(auxDoc, "La fecha de constitucion es mayor a la fecha de hoy."));
                }
                if ((from auxDoc2 in registros where auxDoc.rfc == auxDoc2.rfc || auxDoc.idInterno == auxDoc2.idInterno select auxDoc2).Count() > 1)
                {
                    this.errores.Add(new Errores<ClienteCarga>(auxDoc, "El cliente se encuentra duplicado en los archivos de carga."));
                }
                i += 1;
            }
        }
    }
}

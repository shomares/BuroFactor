using GenericLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Secure
{
    public class CadenaConexion
    {
        private String _cadenaConexion;
        private String valor;
        public CadenaConexion() { }

        public CadenaConexion(IManejaCadenaConexion convertidor) { this.convertidor = convertidor; }

        public IManejaCadenaConexion convertidor { set; get; }
        public String value
        {
            set
            {
                valor = value;
                if (convertidor != null)
                    _cadenaConexion = convertidor.getCadenaConexion(value);
                else
                    _cadenaConexion = value;
            }
            get
            {
                return valor;
            }
        }


        public String cadenaConexion
        {
            get
            {
                return _cadenaConexion;
            }
        }

    }
    public class ContenedorCadenasConexion
    {
        public IDictionary<String, CadenaConexion> conexiones { get; set; }

    }
   
}


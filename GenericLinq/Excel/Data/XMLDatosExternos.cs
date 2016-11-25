using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.XML.Validacion;

namespace Data.XML
{
    public class XMLDatosExternos
    {
        private int _lineaDeColumnas;
        private bool _leeUltimaLinea;

        public XMLDatosExternos()
        {
            _lineaDeColumnas = 1;
            _leeUltimaLinea = true;
        }

        public int lineaColumnas
        {
            get { return _lineaDeColumnas; }
            set { _lineaDeColumnas = value; }
        }

        public bool leeUltimaLinea
        {
            get { return _leeUltimaLinea; }
            set { _leeUltimaLinea = value; }
        }
    }
}

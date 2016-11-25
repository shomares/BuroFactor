using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.XML
{
    public class Campo : IComparable<Campo>
    {
        private string _nombre;
        private string _formato;
        private string _tipo;
        private string _obligatorio;
        private string _defecto;
        private bool _evalua;
        private int _orden;
        private List<string> _valor = new List<string>();
        public Salidas salidas { get; set; }

        public bool evalua
        {
            set { _evalua = value; }
            get { return _evalua; }
        }

        public string defecto
        {
            set { _defecto = value; }
            get { return _defecto; }
        }

        public List<string> valor
        {
            set { _valor = value; }
            get { return _valor; }
        }

        public int orden
        {
            set { _orden = value; }
            get { return _orden; }
        }

        public string obligatorio
        {
            set { _obligatorio = value; }
            get { return _obligatorio; }
        }

        public Campo(string nombre)
        {
            this._nombre = nombre;
        }
        public Campo(string nombre, string formato)
        {
            this._nombre = nombre;
            this._formato = formato;
        }

        public Campo()
        {
            // TODO: Complete member initialization
        }
        public string tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }
        public string nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public string formato
        {
            get { return _formato; }
            set { _formato = value; }
        }


        public static Campo buscaCampo(List<Campo> campo, int pos)
        {
            Campo aux;
            int limiIzq, limiDer;
            int pivote = 0;
            limiIzq = 0;
            limiDer = campo.Count - 1;
            campo.Sort();

            while (limiIzq <= limiDer)
            {
                aux = new Campo();
                pivote = (limiIzq + limiDer) / 2;
                aux = campo[pivote];
                if (pos == aux.orden)
                    return aux;
                if (pos < aux.orden)
                    limiDer = pivote - 1;
                else
                    limiIzq = pivote + 1;
            }
            return null;
        }

        #region IComparable<Campo> Members
        int IComparable<Campo>.CompareTo(Campo other)
        {
            if (this.orden > other.orden)
                return 1;
            else if (this.orden < other.orden)
                return -1;
            else
                return 0;
        }
        #endregion
    }
}


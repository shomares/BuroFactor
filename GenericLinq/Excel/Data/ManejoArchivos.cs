using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra
{
    public class ManejoArchivos
    {
        public static string obtieneExtensionArchivo(string archivo)
        {
            string[] arreglo;
            try
            {
                if (!(String.IsNullOrEmpty(archivo)))
                {
                    arreglo = archivo.Split(".".ToCharArray());
                    return arreglo[arreglo.Length - 1];
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string getNameFile(string file)
        {

            var aux = file.Split('/');
            string name = file;
            if (aux.Length > 0)
                name = aux[aux.Length - 1];
            return name;

        }
    }
}

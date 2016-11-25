using Data.XML.Validacion;
using Extra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Data.XML
{

    public class LeeCampos
    {
        private static IValidaEntrada<T> creaInstanciaValidador<T>(string clase, T dato)
        {
            object aux;
            Type iValidaEntrada = Type.GetType(clase);
            if (iValidaEntrada != null)
            {
                aux = Activator.CreateInstance(iValidaEntrada);
                if (aux is IValidaEntrada<T>)
                    return (IValidaEntrada<T>)aux;
            }
            else
                throw new Exception("No existe la clase: " + clase);
            throw new Exception("La clase  " + clase + "no hereda de IValidaEntrada");
        }

        private static IManejador<T> creaInstanciaManejador<T>(string clase, T datos)
        {
            object aux;
            Type iValidaEntrada = Type.GetType(clase);
            if (iValidaEntrada != null)
            {
                aux = Activator.CreateInstance(iValidaEntrada);
                if (aux is IManejador<T>)
                    return (IManejador<T>)aux;
            }
            else
                throw new Exception("No existe la clase: " + clase);
            throw new Exception("La clase  " + clase + "no hereda de IManejador");
        }

        public static XMLHandler<T> leerConfiguracion<T>(string ruta, T datos)
        {
            XMLHandler<T> campo = new XMLHandler<T>();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;

            try
            {
                xmld = new XmlDocument();
                try
                {
                    xmld.Load(ruta);
                }
                catch (ArgumentException ae)
                {
                    xmld.LoadXml(ruta);
                }
                XmlNodeList = xmld.SelectNodes("/definicion/configuracion");
                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new XMLHandler<T>();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "defcolumnas")
                            campo.lineaColumnas = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "ultimaLinea")
                            campo.leeUltimaLinea = bool.Parse(nodo.InnerText);
                        else if (nodo.Name == "validador-class")
                        {
                            IValidaEntrada<T> validador;
                            if (!String.IsNullOrEmpty(nodo.InnerText))
                            {
                                validador = creaInstanciaValidador(nodo.InnerText, datos);
                                campo.validador = validador;
                            }
                        }
                        else if (nodo.Name == "manejador-class")
                        {

                            IManejador<T> maneja;
                            String[] aux = new String[2];
                            aux = nodo.InnerText.Split(',');
                            if (!String.IsNullOrEmpty(nodo.InnerText))
                            {
                                maneja = creaInstanciaManejador(nodo.InnerText, datos);
                                campo.manejador = maneja;
                            }
                        }
                    }

                }
                return campo;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public static XMLHandler<T> leerConfiguracion<T>(Stream ruta, T datos)
        {
            XMLHandler<T> campo = new XMLHandler<T>();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;

            try
            {
                xmld = new XmlDocument();
                xmld.Load(ruta);

                XmlNodeList = xmld.SelectNodes("/definicion/configuracion");
                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new XMLHandler<T>();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "defcolumnas")
                            campo.lineaColumnas = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "ultimaLinea")
                            campo.leeUltimaLinea = bool.Parse(nodo.InnerText);
                        else if (nodo.Name == "validador-class")
                        {
                            IValidaEntrada<T> validador;
                            validador = creaInstanciaValidador(nodo.InnerText, datos);
                            campo.validador = validador;
                        }
                        else if (nodo.Name == "manejador-class")
                        {

                            IManejador<T> maneja;
                            String[] aux = new String[2];
                            aux = nodo.InnerText.Split(',');
                            maneja = creaInstanciaManejador(nodo.InnerText, datos);
                            campo.manejador = maneja;
                        }
                    }

                }
                return campo;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
        public static XMLDatosExternos leerConfiguracion(string ruta)
        {
            List<XMLDatosExternos> lista = new List<XMLDatosExternos>();
            XMLDatosExternos campo = new XMLDatosExternos();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;
            try
            {
                xmld = new XmlDocument();
                try
                {
                    xmld.Load(ruta);
                }
                catch (ArgumentException ae)
                {
                    xmld.LoadXml(ruta);
                }
                XmlNodeList = xmld.SelectNodes("/definicion/configuracion");

                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new XMLDatosExternos();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "defcolumnas")
                            campo.lineaColumnas = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "ultimaLinea")
                            campo.leeUltimaLinea = bool.Parse(nodo.InnerText);
                    }
                }
                return campo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static XMLDatosExternos leerConfiguracion(Stream str)
        {
            List<XMLDatosExternos> lista = new List<XMLDatosExternos>();
            XMLDatosExternos campo = new XMLDatosExternos();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;
            try
            {
                xmld = new XmlDocument();
                xmld.Load(str);
                XmlNodeList = xmld.SelectNodes("/definicion/configuracion");

                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new XMLDatosExternos();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "defcolumnas")
                            campo.lineaColumnas = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "ultimaLinea")
                            campo.leeUltimaLinea = bool.Parse(nodo.InnerText);
                    }
                }
                return campo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Campo> leerEstructuraCamposXML(string ruta)
        {
            List<Campo> lista = new List<Campo>();
            Campo campo = new Campo();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;
            try
            {
                xmld = new XmlDocument();
                try
                {
                    xmld.Load(ruta);
                }
                catch (ArgumentException ae)
                {
                    xmld.LoadXml(ruta);
                }
                XmlNodeList = xmld.SelectNodes("/definicion/columnas/item");
                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new Campo();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "columna")
                            campo.nombre = nodo.InnerText;
                        else if (nodo.Name == "formato")
                        {

                            if (nodo.Attributes.Count > 0)
                            {
                                foreach (XmlNode atrib in nodo.Attributes)
                                    if (atrib.Name == "tipo")
                                        campo.tipo = atrib.InnerText;
                            }
                            campo.formato = nodo.InnerText;
                        }
                        else if (nodo.Name == "obligatorio")
                            campo.obligatorio = nodo.InnerText;
                        else if (nodo.Name == "orden")
                            campo.orden = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "salidas")
                        {
                            campo.salidas = new Salidas();
                            foreach (XmlNode atrib in nodo.Attributes)
                                if (atrib.Name == "opcional")
                                    campo.salidas.opcional = Boolean.Parse(atrib.InnerText);
                                else if (atrib.Name == "mapa")
                                    campo.salidas.mapa = atrib.InnerText;
                            foreach (XmlNode auxNodoSalida in nodo.ChildNodes)
                            {
                                if (auxNodoSalida.Name == "salida")
                                {
                                    Salida salida = new Salida();
                                    salida.valor = auxNodoSalida.InnerText;
                                    foreach (XmlNode atrib in auxNodoSalida.Attributes)
                                        if (atrib.Name == "opcion")
                                            salida.opcion = atrib.InnerText;
                                    campo.salidas.salidas.Add(salida);
                                }
                            }
                        }
                        else if (nodo.Name == "valor")
                            campo.valor.Add(nodo.InnerText);
                        else if (nodo.Name == "default")
                            campo.defecto = nodo.InnerText;
                        else if (nodo.Name == "evalua")
                            campo.evalua = Boolean.Parse(nodo.InnerText);
                    }
                    lista.Add(campo);
                }
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<Campo> leerEstructuraCamposXML(Stream ruta)
        {
            List<Campo> lista = new List<Campo>();
            Campo campo = new Campo();
            XmlDocument xmld;
            XmlNodeList XmlNodeList;
            try
            {
                xmld = new XmlDocument();
                xmld.Load(ruta);
                XmlNodeList = xmld.SelectNodes("/definicion/columnas/item");
                foreach (XmlNode node in XmlNodeList)
                {
                    campo = new Campo();
                    foreach (XmlNode nodo in node.ChildNodes)
                    {
                        if (nodo.Name == "columna")
                            campo.nombre = nodo.InnerText;
                        else if (nodo.Name == "formato")
                        {

                            if (nodo.Attributes.Count > 0)
                            {
                                foreach (XmlNode atrib in nodo.Attributes)
                                    if (atrib.Name == "tipo")
                                        campo.tipo = atrib.InnerText;
                            }
                            campo.formato = nodo.InnerText;
                        }
                        else if (nodo.Name == "obligatorio")
                            campo.obligatorio = nodo.InnerText;
                        else if (nodo.Name == "orden")
                            campo.orden = Int32.Parse(nodo.InnerText);
                        else if (nodo.Name == "salidas")
                        {
                            campo.salidas = new Salidas();
                            foreach (XmlNode atrib in nodo.Attributes)
                                if (atrib.Name == "opcional")
                                    campo.salidas.opcional = Boolean.Parse(atrib.InnerText);
                                else if (atrib.Name == "mapa")
                                    campo.salidas.mapa = atrib.InnerText;
                            foreach (XmlNode auxNodoSalida in nodo.ChildNodes)
                            {
                                if (auxNodoSalida.Name == "salida")
                                {
                                    Salida salida = new Salida();
                                    salida.valor = auxNodoSalida.InnerText;
                                    foreach (XmlNode atrib in auxNodoSalida.Attributes)
                                        if (atrib.Name == "opcion")
                                            salida.opcion = atrib.InnerText;
                                    campo.salidas.salidas.Add(salida);
                                }
                            }
                        }
                        else if (nodo.Name == "valor")
                            campo.valor.Add(nodo.InnerText);
                        else if (nodo.Name == "default")
                            campo.defecto = nodo.InnerText;
                        else if (nodo.Name == "evalua")
                            campo.evalua = Boolean.Parse(nodo.InnerText);
                    }
                    lista.Add(campo);
                }
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static void validaDatosDataSet<T>(DataTable tabla, List<Campo> campos, ref List<Errores<T>> errores)
        {
            string valor;
            int i = 0;
            Regex expresionrRegular;
            Object[] aux;
            Match match;
            Errores<T> error;
            int contador = 2;
            try
            {

                if (campos.Count == tabla.Columns.Count)
                {
                    foreach (DataRow row in tabla.Rows)
                    {
                        i = 0;
                        aux = row.ItemArray;
                        foreach (Campo campo in campos)
                        {

                            if (campo.obligatorio.ToUpper() == "SI" || !aux[i].ToString().Equals(String.Empty))
                            {
                                if (!(string.IsNullOrEmpty(campo.formato)))
                                {
                                    expresionrRegular = new Regex(campo.formato);

                                    if (aux[i] != null)
                                    {
                                        if (aux[i] is DateTime)
                                            valor = ((DateTime)aux[i]).ToString("dd/MM/yyyy");
                                        else
                                            valor = aux[i].ToString();
                                        match = expresionrRegular.Match(valor);
                                        if (!(match.Success))
                                        {
                                            error = new Errores<T>();
                                            error.notficacion = (i + 1).ToString() + " El campo " + tabla.Columns[i].ColumnName + ", no tiene un formato adecuado en la fila:" + contador;
                                            error.objeto = default(T);
                                            errores.Add(error);
                                        }
                                    }
                                }
                            }
                            i++;
                        }
                        contador++;
                    }
                }
                else {
                    error = new Errores<T>();
                    error.notficacion = "Le faltan o le sobran columnas al layout";
                    error.objeto = default(T);
                    errores.Add(error);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool validaPorExpresionRegular(string valor, string expresion)
        {
            try
            {
                return new Regex(expresion).Match(valor).Success;
            }
            catch (Exception)
            {

                throw;
            }
        }



        public static void validaEstructuraDataSet(DataSet data, List<Campo> campos, ref List<DescripcionError> errores)
        {
            string salida = "OK";
            DataTable tabla;
            int i = 0;
            DescripcionError error;
            try
            {
                tabla = data.Tables[0];

                if (tabla == null)
                    throw new Exception("El archivo esta vacio");

                if (campos.Count > tabla.Columns.Count)
                {
                    throw new Exception("Faltan columnas en el layout");
                }

                if (tabla.Columns.Count > campos.Count)
                    throw new Exception("Hay mas campos en el layout");

                foreach (Campo campo in campos)
                {
                    if (tabla.Columns[i].ColumnName != campo.nombre)
                    {
                        error = new DescripcionError();
                        error.linea = (i + 1).ToString();
                        error.descripcion = "El campo " + tabla.Columns[i].ColumnName + " no esta especificado en el archivo de definicion";
                        salida = "El layout no  tiene el formato correcto";
                        errores.Add(error);
                    }
                    i++;
                }
            }
            catch (Exception e)
            {
                salida = e.Message;
                throw e;
            }
        }

        public List<T> getLista<T>(T objetoref, List<Campo> xml, DataSet dataset)
        {
            List<T> lista = new List<T>();
            Type tipo = typeof(T);
            object aux = Activator.CreateInstance(tipo);
            DataTable tabla;
            Campo campo;
            int i = 0;
            int j = 0;
            try
            {
                tabla = dataset.Tables[0];
                foreach (DataRow row in tabla.Rows)
                {
                    i = 0;
                    aux = Activator.CreateInstance(tipo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;

        }
        public static List<dynamic> convert(DataTable table)
        {
            List<dynamic> lista = new List<dynamic>();
            foreach (DataRow row in table.Rows)
            {
                dynamic objRow = new ExpandoObject();
                foreach (DataColumn colum in table.Columns)
                    (objRow as IDictionary<string, object>).Add(colum.ColumnName, row[colum.ColumnName]);
                lista.Add(objRow);
            }
            return lista;
        }

        public static T convertObject<T>(IDictionary<String, Object> obj)
        {
            T result = Activator.CreateInstance<T>();
            Type type = result.GetType();

            foreach (var key in obj.Keys)
            {
                Object objs = obj[key];
                var propiedad = type.GetProperty(key);

                if (propiedad != null)
                    if (propiedad.PropertyType == typeof(DateTime))
                    {
                        DateTime time = new DateTime();
                        if (DateTime.TryParse(objs.ToString(), out time))
                            propiedad.SetValue(result, time, null);
                    }
                    else
                        propiedad.SetValue(result, objs, null);
            }
            return result;

        }

        public static T convertDynamicObject<T>(dynamic obj)
        {
            IDictionary<String, object> diccionario = obj as IDictionary<String, Object>;
            return convertObject<T>(diccionario);
        }
    
     


    }
}

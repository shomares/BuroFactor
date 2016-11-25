using Data.XML;
using Data.XML.Validacion;
using Extra;
using GenericLinq.Excel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Report.Excel
{
    public class FuncionesExcel : IDisposable
    {
        public IContainerLayout Container { get; set; }
        public DataTable getDatosCSV(string ruta)
        {
            DataSet data = new DataSet();
            OleDbCommand comando = new OleDbCommand();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbConnection conexion = new OleDbConnection();
            DataTable tabla = null;
            try
            {
                conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties=text;HDR=Yes;FMT=Delimited");
                conexion.Open();
                comando.CommandText = "SELECT * FROM " + ManejoArchivos.getNameFile(ruta);
                comando.Connection = conexion;
                adapter.SelectCommand = comando;
                adapter.Fill(data, "cl");
                tabla = data.Tables[0];
                borrarValoresNULL(ref tabla);
                conexion.Close();
                return tabla;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable getDatosExcel(string ruta, string extension, string hoja)
        {
            DataSet data = new DataSet();
            OleDbCommand comando = new OleDbCommand();
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            OleDbConnection conexion = new OleDbConnection();
            DataTable tabla;
            try
            {
                conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";");
                try
                {
                    conexion.Open();
                }
                catch (InvalidOperationException ioe)
                {
                    if (extension == "xlsx")
                    {
                        Process.Start("http://www.microsoft.com/en-us/download/details.aspx?id=13255");
                        throw ioe;
                    }
                    else
                    {
                        conexion = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + ruta + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";");
                        conexion.Open();
                    }
                }
                DataTable dbSchema = conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dbSchema == null || dbSchema.Rows.Count < 1)
                {
                    throw new Exception("Error: Could not determine the name of the first worksheet.");
                }
                string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();

                comando.CommandText = "SELECT * FROM [" + firstSheetName + "]";
                comando.Connection = conexion;
                adapter.SelectCommand = comando;
                adapter.Fill(data, "cl");
                tabla = data.Tables[0];
                borrarValoresNULL(ref tabla);
                conexion.Close();
                return tabla;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void borrarValoresNULL(ref DataTable tabla)
        {
            int s = 0;
            int aux = 0;
            Object[] objeto;
            DataRow row;
            while (aux < tabla.Rows.Count)
            {
                s = 0;
                row = tabla.Rows[aux];
                objeto = row.ItemArray;
                if (objeto != null)
                {
                    foreach (Object obj in objeto)
                        if (obj == null || String.IsNullOrEmpty(obj.ToString())) s++;
                    if (s == objeto.Length) tabla.Rows.RemoveAt(aux);
                    else aux++;
                }
            }
        }


        public List<T> getDatosExcel<T>(DataTable table, List<Campo> campos, T aux)
        {
            List<T> lista = new List<T>();
            PropertyInfo[] propiedades;
            Type tipo;
            T objeto;
            string clase;
            bool entro = false;

            try
            {
                tipo = aux.GetType();
                clase = tipo.FullName;
                propiedades = tipo.GetProperties();

                foreach (DataRow row in table.Rows)
                {
                    objeto = (T)Activator.CreateInstance(tipo);

                    foreach (Campo campo in campos)
                    {
                        entro = false;
                        foreach (string valor in campo.valor)
                            objeto = getObjeto(valor, campo, row, objeto, ref entro);
                    }
                    lista.Add(objeto);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private T getObjeto<T>(string nombre, Campo campo, DataRow data, T aux, ref bool entro)
        {
            PropertyInfo[] propiedades;
            Type tipo;
            string[] campos;
            string nombrecampo;
            Type tipoPropiedad;
            int contador = 0;
            try
            {
                tipo = aux.GetType();
                propiedades = tipo.GetProperties();
                campos = nombre.Split('.');
                if (campos.Length > 0) nombrecampo = campos[0];
                else nombrecampo = nombre;
                foreach (PropertyInfo propiedad in propiedades)
                {
                    if (propiedad.Name == nombrecampo)
                    {
                        tipoPropiedad = propiedad.PropertyType;
                        if (tipoPropiedad == typeof(string) || tipoPropiedad.IsPrimitive || tipoPropiedad == typeof(DateTime) || tipoPropiedad == typeof(decimal))
                        {
                            contador = 0;
                            foreach (DataColumn columna in data.Table.Columns)
                            {
                                if (columna.ColumnName.Replace("#", ".") == campo.nombre)
                                {
                                    propiedad.SetValue(aux, Convert.ChangeType(data[columna.ColumnName], tipoPropiedad), null);
                                    break;
                                }
                                else
                                    contador++;
                            }
                            if (contador == data.Table.Columns.Count)
                                if (!String.IsNullOrEmpty(campo.defecto))
                                    propiedad.SetValue(aux, Convert.ChangeType(campo.defecto, tipoPropiedad), null);
                            break;
                        }
                        else if (tipoPropiedad.IsClass && !tipoPropiedad.IsGenericType)
                        {
                            string buscar = "";
                            for (int i = 1; i < campos.Length; i++)
                            {
                                buscar += campos[i];
                                if (i < campos.Length - 1) buscar += ".";
                            }

                            object objeto2;
                            Type auxc2 = propiedad.GetType();
                            objeto2 = propiedad.GetValue(aux, null);

                            if (objeto2 == null)
                                objeto2 = Activator.CreateInstance(tipoPropiedad);
                            objeto2 = getObjeto(buscar, campo, data, objeto2, ref entro);
                            propiedad.SetValue(aux, objeto2, null);
                            break;
                        }
                    }
                }
                return aux;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ListaHandler<T> cargaListadeExcelconHandler<T>(String excel, String layout, ref List<Errores<T>> errores, T objeto)
        {
            List<T> objetos;
            List<Campo> campos = new List<Campo>();
            String msg = "";
            DataTable tabla = null;
            XMLHandler<T> handler = null;
            ListaHandler<T> lista;
            try
            {
                lista = new ListaHandler<T>();
                tabla = getDatosExcel(excel, ManejoArchivos.obtieneExtensionArchivo(excel), "Sheet1");
                campos = Container.GetLayout(layout);
                LeeCampos.validaDatosDataSet<T>(tabla, campos, ref errores);
                if (errores.Count == 0)
                {
                    objetos = getDatosExcel(tabla, campos, objeto);
                    handler = Container.GetHandler<T>(layout, objeto);
                    if (handler.validador != null)
                    {
                        handler.validador.errores = errores;
                        handler.validarDatosLista(objetos);
                    }

                    if (handler.manejador != null)
                        objetos = handler.manejador.manejameEsta(objetos);
                    lista.listaObjetos = objetos;
                    lista.handler = handler;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lista;
        }

        public void Dispose()
        {

        }
    }

}

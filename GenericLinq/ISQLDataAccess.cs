using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{

    /// <summary>
    /// Interfaz que implementa una conexión a la base de datos utilizando ADO.DB
    /// </summary>
    public interface ISQLDataAcces
    {
        /// <summary>
        /// La cadena de conexión
        /// </summary>
        string CadenaConexion { get; set; }
        /// <summary>
        /// Ejecuta una operación query que no regresa consulta
        /// </summary>
        /// <param name="_tipoComando">El tipo de la operación query</param>
        /// <param name="_cadenaComando">El contenido de la operación query</param>
        void ExecuteNonQuery(System.Data.CommandType _tipoComando, string _cadenaComando);
        /// <summary>
        /// Obtiene el objeto DataSet de retorno de una operación query
        /// </summary>
        /// <param name="_tipoComando">El tipo de la operación query</param>
        /// <param name="_cadenaComando">El contenido de la operación query</param>
        /// <returns>Regresa la tabla consultada en un objeto DataSet</returns>
        System.Data.DataSet GetDataSet(System.Data.CommandType _tipoComando, string _cadenaComando);
        /// <summary>
        /// Obtiene el objeto DataView de retorno de una operación query
        /// </summary>
        /// <param name="_tipoComando">El tipo de la operación query</param>
        /// <param name="_cadenaComando">El contenido de la operación query</param>
        /// <returns>Regresa la tabla consultada en un objeto DataView</returns>
        System.Data.DataView GetDataView(System.Data.CommandType _tipoComando, string _cadenaComando);
        /// <summary>
        /// Obtiene el objeto de retorno de una operación query
        /// </summary>
        /// <param name="_tipoComando">El tipo de la operación query</param>
        /// <param name="_cadenaComando">El contenido de la operación query</param>
        /// <returns>Regresa la tabla consultada en un objeto</returns>
        object GetOutput(System.Data.CommandType _tipoComando, string _cadenaComando);
        /// <summary>
        /// Obtiene el una fila de retorno de una operación query
        /// </summary>
        /// <param name="_tipoComando">El tipo de la operación query</param>
        /// <param name="_cadenaComando">El contenido de la operación query</param>
        /// <returns>Regresa la fila consultada en un objeto</returns>
        object GetRow(System.Data.CommandType _tipoComando, string _cadenaComando);
    }
}

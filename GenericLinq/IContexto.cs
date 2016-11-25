using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{
    /// <summary>
    /// Interfaz para Bases de datos, particularmente funcionalidades de Transacciones
    /// </summary>
    public interface IContexto
    {
        /// <summary>
        /// Comienza la transaccion
        /// </summary>
        void beginTransaction();
        /// <summary>
        /// Ejecuta la transaccion
        /// </summary>
        void commitTransaction();
        /// <summary>
        /// Regresa la base de datos a su estado previo a la transaccion
        /// </summary>
        void rollbackTransaction();
        /// <summary>
        /// Obtiene la conexion de base de datos (Implementado para BD)
        /// </summary>
        /// <returns>Objeto BDConnection, de la cual hereda SQLConnection</returns>
        DbConnection getConexion();
        /// <summary>
        /// Reinicia la conexión
        /// </summary>
        void Recarga();
        /// <summary>
        /// Obtiene una nueva instancia de la conexión en curso
        /// </summary>
        /// <param name="cn">El medio de conexion</param>
        /// <returns>Una nueva instancia de la conexión</returns>
        IContexto reload(string cn);
    }
}

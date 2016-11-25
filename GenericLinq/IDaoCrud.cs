using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{
    /// <summary>
    /// Intefaz de control para Funcionalidad Altas, Bajas, Cambios y Consultas.
    /// Se recomienda implementar en una clase abstracta, de a cuerdo al driver de Acceso a Datos.
    /// Sea T la Tabla o su equivalente
    /// </summary>
    public interface IDaoCrud<T>
    {
        /// <summary>
        /// Obtiene una lista de todos los elementos de la tabla (o equivalente)
        /// </summary>
        /// <returns>La lista de elementos obtenida</returns>
        List<T> consulta();
        /// <summary>
        /// Inserta el objeto en la tabla (o equivalente) mapeada (o especificado)
        /// <param name="obj">El objeto a insertar</param>
        /// </summary>
        void inserta(T obj);
        /// <summary>
        /// Inserta una lista de objetos en la tabla (o equivalente) mapeada (o especificado)
        /// </summary>
        /// <param name="lista">La lista de objetos a insertar</param>
        void inserta(List<T> lista);
        /// <summary>
        /// Modifica el objeto especificado 
        /// </summary>
        /// <param name="obj">El objeto a modificar</param>
        void modifica(T obj);
        /// <summary>
        /// Borra el objeto especificado 
        /// </summary>
        /// <param name="obj">El objeto a borrar</param>
        void borra(T obj);
        /// <summary>
        /// Borra toda la tabla (o equivalente)
        /// </summary>
        void borraTodo();
        /// <summary>
        /// Efectua la transacción (o equivalente)
        /// </summary>
        void commit();
        /// <summary>
        /// Resetea el medio de conexión (por ejemplo Cadena de Conexion)
        /// </summary>
        /// <param name="cn">El medio de conexion</param>
        void restartConnection(string cn);
        /// <summary>
        /// Regresa la instancia de la conexión
        /// </summary>
        /// <returns>La conexión</returns>
        IContexto getContexto();

        void setIContexto(IContexto contexto);
    }
}

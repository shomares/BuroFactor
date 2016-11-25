using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{    
    /// <summary>
    /// Interfaz de DAO para una Interfaz Gráfica de Usuario
    /// Para utilizarse en conjunto con la implementación de la interfaz IDaoCrud<T>
    /// Pensada particularmente para Tablas de Tipo Catalogo y funcionalidad 
    /// Altas Bajas Cambios y Consultas
    /// ->Pero Extendible
    /// </summary>
    /// <typeparam name="T">La tabla o equivalente</typeparam>
    public interface ICatalogos<T>
    {
        /// <summary>
        /// Borra el objeto especificado en la tabla o equivalente
        /// </summary>
        /// <param name="aux">El objeto a borrar</param>
        void borrar(T aux);
        /// <summary>
        /// Obtiene una lista con todos los datos de la tabla o equivalente
        /// </summary>
        /// <returns>Una lista con los datos</returns>
        System.Collections.Generic.List<T> getAll();
        /// <summary>
        /// Inserta un objeto en la tabla o equivalente
        /// </summary>
        /// <param name="aux">El objeto a insertar</param>
        void inserta(T aux);
        /// <summary>
        /// Actualiza la tabla o equivalente con su estado actual
        /// </summary>
        void update(T aux);
        /// <summary>
        /// Refresca la conexion a la base de datos o equivalente
        /// </summary>
        void refresh();
    }
}

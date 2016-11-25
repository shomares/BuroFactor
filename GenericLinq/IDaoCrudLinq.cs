using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericLinq
{
    /// <summary>
    /// Clase Abstracta que implementa IDaoCrud utilizando Linq.
    /// Sea T la tabla y P la Base de Datos que implementa IContexto
    /// </summary>
    public abstract class IDaoCrudLinq<T, P> : IDaoCrud<T> where P : DataContext, IContexto
    {
        private P _BD;
        ///<summary>
        ///La base de Datos
        ///</summary>
        public P BD
        {
            get { return _BD; }
            set { _BD = value; }
        }
        public  List<T> consulta()
        {
            var x = (from T algo in BD.GetTable(typeof(T)) select algo).ToList<T>();
            return x;
        }
        public  void inserta(T obj)
        {
            try
            {
                Type ti = typeof(T);
                ITable tabla = BD.GetTable(ti);
                tabla.InsertOnSubmit(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public  void inserta(List<T> lista)
        {
            try
            {
                Type ti = typeof(T);
                ITable tabla = BD.GetTable(ti);
                tabla.InsertAllOnSubmit(lista);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// No está correctamente implementada para linq :c
        /// </summary>
        /// <param name="obj">El objeto a modificar</param>
        public abstract void modifica(T obj);
        public void borra(T obj)
        {
            try
            {
                Type ti = typeof(T);
                ITable tabla = BD.GetTable(ti);
                tabla.DeleteOnSubmit(obj);
                commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public abstract void borraTodo();
        public void commit()
        {
            try
            {
                BD.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IContexto getContexto()
        {
            return BD;
        }


        public void restartConnection(string cn)
        {
            BD = (P)BD.reload(cn);
        }

        public void setIContexto(IContexto contexto)
        {
            this.BD = (P)contexto;
        }
    }
}

using BuroFactorWS.src.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactorWS.src.extension
{
    public static class ExceptionMessage
    {
        private static UnexpectedServiceFault getInner(Exception ex, ref IList<UnexpectedServiceFault> lista)
        {
            if (ex.InnerException != null)
                lista.Add(getInner(ex.InnerException, ref lista));
            return new UnexpectedServiceFault() { ErrorMessage = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace, Target = ex.TargetSite.ToString() };
        }

        public static IList<UnexpectedServiceFault> getInnerExceptions(this Exception ex)
        {
            IList<UnexpectedServiceFault> lista = null;
            if (ex.InnerException != null)
            {
                lista = new List<UnexpectedServiceFault>();
                lista.Add(getInner(ex, ref lista));
            }
            return lista;
        }
    }
}
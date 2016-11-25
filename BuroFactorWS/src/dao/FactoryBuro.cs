using BuroFactorWS.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactorWS.src.dao
{
    public class DaoFactoryEF : IBuroFactory
    {
        public burofactorEntities BeginTrasanction()
        {
            var buro = this.Create();
            buro.Database.BeginTransaction();
            return buro;
        }

        public burofactorEntities Create()
        {
            return new burofactorEntities();
        }
    }
}
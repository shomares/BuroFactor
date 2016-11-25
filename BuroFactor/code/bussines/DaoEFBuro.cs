using BuroFactor.code.bussines.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BuroFactor.Models.dao;

namespace BuroFactor.code.bussines
{
    public class DaoEFBuro : IBuroQuery
    {
        public IBuroFactory FactoryEntityFactory { get; set; }

        public List<financiera> getFinancieras()
        {
            List<financiera> financieras = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var a = FactorEntity.financiera.Include(f => f.persona);
                financieras = a.ToList();

            }
            return financieras;
        }

        public List<financiera> getFinancieras(string RFC)
        {
            List<financiera> financieras = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var a = FactorEntity.financiera.Include(f => f.persona);

                if (!String.IsNullOrEmpty(RFC))
                {
                    financieras = (from p in a
                                   where p.persona.RFC.Contains(RFC)
                                   select p).ToList();
                }
                else
                    financieras = a.ToList();

            }
            return financieras;
        }

        public List<planconsulta> getPlanes()
        {
            List<planconsulta> planesconsulta = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
                planesconsulta = FactorEntity.planconsulta.ToList();
            return planesconsulta;
        }



        public List<plancontratado> getPlanesPorFinanciera(int idFinanciera)
        {
            List<plancontratado> planesconsulta = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var a = FactorEntity.plancontratado.Include(f => f.planconsulta);
                planesconsulta = (from p in a
                                  where p.Activo == true && p.Financiera_idFinanciera == idFinanciera
                                  select p).ToList();
            }
            return planesconsulta;
        }

        public plancontratado getPlanPorSerial(string token)
        {
            plancontratado planesconsulta = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var a = FactorEntity.plancontratado.Include(f => f.planconsulta).Include(f=> f.financiera.persona);
                planesconsulta = (from p in a
                                  where p.Serial == token
                                  select p).FirstOrDefault();
            }
            return planesconsulta;
        }
    }
}
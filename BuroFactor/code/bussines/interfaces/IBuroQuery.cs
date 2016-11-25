using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuroFactor.Models.dao;

namespace BuroFactor.code.bussines.interfaces
{
    public interface IBuroQuery
    {
        List<financiera> getFinancieras();
        List<planconsulta> getPlanes();
        List<plancontratado> getPlanesPorFinanciera(int idFinanciera);
        List<financiera> getFinancieras(string RFC);
        plancontratado getPlanPorSerial(string token);
    }
}

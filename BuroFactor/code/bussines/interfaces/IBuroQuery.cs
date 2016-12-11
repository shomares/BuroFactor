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
        usuario validaUsuario(string user, string token);
        List<deuda> getDeudasActivas(Int32? emisor, DateTime? fechaIni, DateTime? fechaFin, string divisa, string financiera);
        List<deuda> getDeudasActivas(string financiera);
        List<divisa> getDivisas();
        deuda getDeuda(int idFactura);
        List<relacionclientefinanciera> getRelacionesPorFinanciera(string v);
        financiera getFinancieraPorRFC(string v);
        List<deuda> getDeudasVencidas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string v);
        List<deuda> getDeudasPagadas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string v);
        List<deuda> getDeudasCanceladas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string v);
    }
}

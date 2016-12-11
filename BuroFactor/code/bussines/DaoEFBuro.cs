using BuroFactor.code.bussines.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BuroFactor.Models.dao;
using BuroComun.src.security;

namespace BuroFactor.code.bussines
{
    public class DaoEFBuro : IBuroQuery
    {
        public IBuroFactory FactoryEntityFactory { get; set; }
        public IGeneraContrasena Generador { get; set; }

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

        public usuario validaUsuario(string user, string token)
        {
            usuario salida = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                salida = (from aux in FactorEntity.usuario
                          where aux.username == user && aux.activo.Value
                          select aux).FirstOrDefault();

                if (salida != null)
                {
                    var pass = Generador.generaPassword(salida.salt, token);
                    if (pass != salida.password)
                        salida = null;
                }
            }
            return salida;

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
                var a = FactorEntity.plancontratado.Include(f => f.planconsulta).Include(f => f.financiera.persona);
                planesconsulta = (from p in a
                                  where p.Serial == token
                                  select p).FirstOrDefault();
            }
            return planesconsulta;
        }

        public List<deuda> getDeudasActivas(Int32? emisor, DateTime? fechaIni, DateTime? fechaFin, string divisa, string idFinanciera)
        {
            List<deuda> deudas = null;
            DateTime fechaIniV;
            DateTime fechaFinalV;

            Int32 idEmisora = emisor == null ? Int32.MinValue : emisor.Value;

            fechaIniV = fechaIni == null ? DateTime.MinValue : fechaIni.Value;
            fechaFinalV = fechaFin == null ? DateTime.MaxValue : fechaFin.Value;


            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                int id = Int32.Parse(idFinanciera);
                deudas = (from aux in FactorEntity.deuda.Include(p => p.operacion)
                          .Include(p => p.relacionclientefinanciera)
                          .Include(p => p.relacionclientefinanciera.persona)
                          .Include(p => p.divisa)
                          .Include(p => p.relacionclientefinanciera1)
                          .Include(p => p.relacionclientefinanciera1.persona)
                          .Include(p => p.financiera)
                          .Include(p => p.financiera.persona)

                          where
                          aux.FechaPago > DateTime.Now
                          && aux.relacionclientefinanciera.idRelacionClienteFinanciera ==
                          (idEmisora == int.MinValue ? aux.relacionclientefinanciera.idRelacionClienteFinanciera : idEmisora)
                          && !aux.operacion.Where(s => s.Tipo == "C" || s.Tipo == "P").Any()
                          && aux.Financiera_idFinanciera == id
                          && (aux.FechaOperacion >= fechaIniV && aux.FechaPago <= fechaFinalV)
                          select aux
                          ).ToList();
            }
            return deudas;
        }



        public List<deuda> getDeudasActivas(string idFinanciera)
        {
            List<deuda> deudas = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                int id = Int32.Parse(idFinanciera);
                deudas = (from aux in FactorEntity.deuda.Include(p => p.operacion).Include(p => p.relacionclientefinanciera)
                          where aux.FechaPago > DateTime.Now
                          && !aux.operacion.Where(s => s.Tipo == "C" || s.Tipo == "P").Any()
                          && aux.Financiera_idFinanciera == id
                          select aux
                          ).ToList();

            }
            return deudas;
        }

        public List<divisa> getDivisas()
        {
            List<divisa> divisas = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                divisas = FactorEntity.divisa.ToList();
            }
            return divisas;
        }

        public deuda getDeuda(int idFactura)
        {
            deuda salida = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var deudas = FactorEntity.deuda.Include(p => p.relacionclientefinanciera);
                salida = deudas.Where(p => p.idDeuda == idFactura).FirstOrDefault();
            }
            return salida;
        }

        public List<relacionclientefinanciera> getRelacionesPorFinanciera(string idFinanciera)
        {
            List<relacionclientefinanciera> salida = null;
            int id = Int32.Parse(idFinanciera);
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                var idRelacion = (from aux in FactorEntity.relacionclientefinanciera
                                  join deuda deu in FactorEntity.deuda
                                  on aux.idRelacionClienteFinanciera equals deu.idEmisor
                                  where aux.Financiera_idFinanciera == id
                                  select aux.idRelacionClienteFinanciera).Distinct().ToList();
                salida = (from aux in FactorEntity.relacionclientefinanciera.Include(p => p.persona)
                          where idRelacion.Any(s => s == aux.idRelacionClienteFinanciera)
                          select aux).ToList();

            }
            return salida;
        }

        public List<deuda> getDeudasVencidas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string idFinanciera)
        {
            List<deuda> deudas = null;
            DateTime fechaIniV;
            DateTime fechaFinalV;

            Int32 idEmisora = empresa == null ? Int32.MinValue : empresa.Value;

            fechaIniV = fechaIni == null ? DateTime.MinValue : fechaIni.Value;
            fechaFinalV = fechaFin == null ? DateTime.MaxValue : fechaFin.Value;


            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                int id = Int32.Parse(idFinanciera);
                deudas = (from aux in FactorEntity.deuda.Include(p => p.operacion)
                          .Include(p => p.relacionclientefinanciera)
                          .Include(p => p.relacionclientefinanciera.persona)
                          .Include(p => p.divisa)
                          .Include(p => p.relacionclientefinanciera1)
                          .Include(p => p.relacionclientefinanciera1.persona)
                          .Include(p => p.financiera)
                          .Include(p => p.financiera.persona)

                          where
                          aux.FechaPago < DateTime.Now
                          && aux.relacionclientefinanciera.idRelacionClienteFinanciera ==
                          (idEmisora == int.MinValue ? aux.relacionclientefinanciera.idRelacionClienteFinanciera : idEmisora)
                          && !aux.operacion.Where(s => s.Tipo == "C" || s.Tipo == "P").Any()
                          && aux.Financiera_idFinanciera == id
                          && (aux.FechaOperacion >= fechaIniV && aux.FechaPago <= fechaFinalV)
                          select aux
                          ).ToList();
            }
            return deudas;
        }

        public List<deuda> getDeudasPagadas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string idFinanciera)
        {
            List<deuda> deudas = null;
            DateTime fechaIniV;
            DateTime fechaFinalV;

            Int32 idEmisora = empresa == null ? Int32.MinValue : empresa.Value;

            fechaIniV = fechaIni == null ? DateTime.MinValue : fechaIni.Value;
            fechaFinalV = fechaFin == null ? DateTime.MaxValue : fechaFin.Value;


            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                int id = Int32.Parse(idFinanciera);
                deudas = (from aux in FactorEntity.deuda.Include(p => p.operacion)
                          .Include(p => p.relacionclientefinanciera)
                          .Include(p => p.relacionclientefinanciera.persona)
                          .Include(p => p.divisa)
                          .Include(p => p.relacionclientefinanciera1)
                          .Include(p => p.relacionclientefinanciera1.persona)
                          .Include(p => p.financiera)
                          .Include(p => p.financiera.persona)

                          where
                          aux.relacionclientefinanciera.idRelacionClienteFinanciera ==
                          (idEmisora == int.MinValue ? aux.relacionclientefinanciera.idRelacionClienteFinanciera : idEmisora)
                          && aux.operacion.Where(s => s.Tipo == "P").Any()
                          && aux.Financiera_idFinanciera == id
                          && (aux.FechaOperacion >= fechaIniV && aux.FechaPago <= fechaFinalV)
                          select aux
                          ).ToList();
            }
            return deudas;
        }

        public List<deuda> getDeudasCanceladas(int? empresa, DateTime? fechaIni, DateTime? fechaFin, string divisa, string idFinanciera)
        {
            List<deuda> deudas = null;
            DateTime fechaIniV;
            DateTime fechaFinalV;

            Int32 idEmisora = empresa == null ? Int32.MinValue : empresa.Value;

            fechaIniV = fechaIni == null ? DateTime.MinValue : fechaIni.Value;
            fechaFinalV = fechaFin == null ? DateTime.MaxValue : fechaFin.Value;


            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                int id = Int32.Parse(idFinanciera);
                deudas = (from aux in FactorEntity.deuda.Include(p => p.operacion)
                          .Include(p => p.relacionclientefinanciera)
                          .Include(p => p.relacionclientefinanciera.persona)
                          .Include(p => p.divisa)
                          .Include(p => p.relacionclientefinanciera1)
                          .Include(p => p.relacionclientefinanciera1.persona)
                          .Include(p => p.financiera)
                          .Include(p => p.financiera.persona)

                          where
                          aux.relacionclientefinanciera.idRelacionClienteFinanciera ==
                          (idEmisora == int.MinValue ? aux.relacionclientefinanciera.idRelacionClienteFinanciera : idEmisora)
                          && aux.operacion.Where(s => s.Tipo == "C").Any()
                          && aux.Financiera_idFinanciera == id
                          && (aux.FechaOperacion >= fechaIniV && aux.FechaPago <= fechaFinalV)
                          select aux
                          ).ToList();
            }
            return deudas;
        }

        public financiera getFinancieraPorRFC(string rfc)
        {
            financiera salida = null;
            using (burofactorEntities FactorEntity = FactoryEntityFactory.Create())
            {
                salida = FactorEntity.financiera.Include(p => p.persona).Where(aux => aux.persona.RFC == rfc).FirstOrDefault();
            }
            return salida;
        }
    }
}
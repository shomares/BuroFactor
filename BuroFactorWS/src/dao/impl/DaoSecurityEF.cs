using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BuroFactorWS.model;
using BuroComun.src.security;
using System.Data.Entity;

namespace BuroFactorWS.src.dao.impl
{
    public class DaoSecurityEF : IDaoSecurity
    {
        public IBuroFactory Factory { get; set; }

        public IGeneraContrasena Generador { get; set; }

        public plancontratado getPlan(string userName)
        {
            plancontratado salida = null;
            using (var factorEntity = Factory.Create())
            {
                salida = (from aux in factorEntity.plancontratado
                          where aux.UsuarioWS == userName && aux.Activo == true
                          select aux).FirstOrDefault();
            }
            return salida;
        }

        public usuario getUser(string userName)
        {

            throw new NotImplementedException();
        }

        public bool validaUsuarioWs(string userName, string password)
        {

            plancontratado salida = null;
            using (var factorEntity = Factory.Create())
            {
                salida = (from aux in factorEntity.plancontratado.Include(p => p.planconsulta)
                          where aux.UsuarioWS == userName && aux.Activo == true
                          select aux).FirstOrDefault();

                if (salida != null && !String.IsNullOrEmpty(password))
                    password = Generador.generaPassword(salida.SalWS, password);
                else
                    password = salida.ContrasenaWS;
            }
            return salida != null && salida.planconsulta.FechaVencimiento > DateTime.Now && salida.ContrasenaWS == password;
        }
    }
}
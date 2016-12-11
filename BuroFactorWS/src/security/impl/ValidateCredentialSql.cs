using BuroComun.src.security;
using BuroFactorWS.model;
using BuroFactorWS.src.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace BuroFactorWS.src.security.impl
{
    public class ValidateCredentialSql : IValidateCredentials
    {

        private IEncripta Encripta { get; set; }

        private IDaoSecurity DaoSeguridad { get; set; }





        public bool validateCredentials(string userName, string password)
        {
            plancontratado planContratado = null;
            //La solicitud se hizo desde el portal-------------------

            planContratado = new plancontratado()
            {
                UsuarioWS = userName,
                ContrasenaWS = password
            };

            if (planContratado != null)
                return DaoSeguridad.validaUsuarioWs(planContratado.UsuarioWS, planContratado.ContrasenaWS);
            else
                return false;
        }
    }
}
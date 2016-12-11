using BuroFactor.code.bussines.interfaces;
using BuroFactor.Models.dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuroFactor.code.bussines
{
    public class ValidateTokenWebMatrix : ITokenOperaciones
    {
        public IBuroQuery DaoEFBuro { get; set; }

        public bool validaToken(string user, string token)
        {
            usuario usu = DaoEFBuro.validaUsuario(user, token);
            return usu != null;
        }
    }
}
using BuroFactorWS.src.service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace BuroFactorWS.src.security
{
    public class ValidateUsernameToken : System.IdentityModel.Selectors.UserNamePasswordValidator
    {


        public override void Validate(string userName, string password)
        {
            IValidateCredentials ValidateProvider = ServiceBuro.Instance.ValidateProvider;
            if (userName == null)
            {
                throw new ArgumentNullException();
            }
            if (!(ValidateProvider.validateCredentials(userName, password)))
                throw new FaultException("Incorrect Username or Password");

        }
    }
}
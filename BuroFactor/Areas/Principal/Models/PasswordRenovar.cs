using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Principal.Models
{
    public class PasswordRenovar
    {

        [Required]
        public string password { get; set; }

        [Required]
        [RegularExpression("^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$")]
        public string passwordNueva { get; set; }

        [Required]
        public string passwordCopia { get; set; }
    }
}
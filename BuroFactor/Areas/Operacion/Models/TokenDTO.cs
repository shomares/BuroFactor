using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BuroFactor.Areas.Operacion.Models
{
    public class TokenDTO
    {
        [StringLength(100)]
        public string Token { get; set; }
        [StringLength(100)]
        public string IdTicket { get; set; }

    }
}
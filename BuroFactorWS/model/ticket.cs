//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BuroFactorWS.model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ticket
    {
        public int idTicket { get; set; }
        public string Data { get; set; }
        public System.DateTime Fecha { get; set; }
        public int PlanContratado_idPlanContratado { get; set; }
        public string Serial { get; set; }
    
        public virtual plancontratado plancontratado { get; set; }
    }
}
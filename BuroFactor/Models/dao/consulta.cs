
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace BuroFactor.Models.dao
{

using System;
    using System.Collections.Generic;
    
public partial class consulta
{

    public int idConsulta { get; set; }

    public System.DateTime FechaConsulta { get; set; }

    public int PlanContratado_idPlanContratado { get; set; }

    public bool Interno { get; set; }

    public string IP { get; set; }



    public virtual plancontratado plancontratado { get; set; }

}

}


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
    
public partial class usuario
{

    public int idUsuarios { get; set; }

    public string username { get; set; }

    public string password { get; set; }

    public string salt { get; set; }

    public string aleatorio { get; set; }

    public Nullable<int> Financiera_idFinanciera { get; set; }

    public Nullable<bool> activo { get; set; }



    public virtual financiera financiera { get; set; }

}

}

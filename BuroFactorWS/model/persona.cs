
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
    
public partial class persona
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public persona()
    {

        this.direccion = new HashSet<direccion>();

        this.financiera = new HashSet<financiera>();

        this.relacionclientefinanciera = new HashSet<relacionclientefinanciera>();

    }


    public int idPersona { get; set; }

    public string RazonSocial { get; set; }

    public string RFC { get; set; }

    public System.DateTime FechaConstitucion { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<direccion> direccion { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<financiera> financiera { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<relacionclientefinanciera> relacionclientefinanciera { get; set; }

}

}


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
    
public partial class municipio
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public municipio()
    {

        this.colonia = new HashSet<colonia>();

    }


    public int idMunicipio { get; set; }

    public int Estado_idEstado { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<colonia> colonia { get; set; }

    public virtual estado estado { get; set; }

}

}

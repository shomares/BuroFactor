
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
    using System.ComponentModel.DataAnnotations;

    public partial class planconsulta
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public planconsulta()
        {

            this.plancontratado = new HashSet<plancontratado>();

        }



        public int idPlanConsulta { get; set; }


        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "# Consultas al Mes")]
        public int MaxConsultaMes { get; set; }

        [Display(Name = "Fecha Registro")]
        public System.DateTime FechaRegistro { get; set; }

        [Display(Name = "Fecha Vencimiento")]
        public System.DateTime FechaVencimiento { get; set; }

        [Display(Name = "Precio")]
        public decimal Precio { get; set; }

        public bool Activo { get; set; }

        public bool Ilimitado { get; set; }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<plancontratado> plancontratado { get; set; }

    }

}

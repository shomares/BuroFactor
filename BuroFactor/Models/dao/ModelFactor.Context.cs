﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class burofactorEntities : DbContext
    {
        public burofactorEntities()
            : base("name=burofactorEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<codigopostal> codigopostal { get; set; }
        public virtual DbSet<colonia> colonia { get; set; }
        public virtual DbSet<consulta> consulta { get; set; }
        public virtual DbSet<deuda> deuda { get; set; }
        public virtual DbSet<direccion> direccion { get; set; }
        public virtual DbSet<divisa> divisa { get; set; }
        public virtual DbSet<estado> estado { get; set; }
        public virtual DbSet<financiera> financiera { get; set; }
        public virtual DbSet<municipio> municipio { get; set; }
        public virtual DbSet<operacion> operacion { get; set; }
        public virtual DbSet<pais> pais { get; set; }
        public virtual DbSet<persona> persona { get; set; }
        public virtual DbSet<planconsulta> planconsulta { get; set; }
        public virtual DbSet<plancontratado> plancontratado { get; set; }
        public virtual DbSet<relacionclientefinanciera> relacionclientefinanciera { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
    }
}
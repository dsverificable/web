﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UAndes.ICC5103._202301.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class InscripcionesBrDbEntities : DbContext
    {
        public InscripcionesBrDbEntities()
            : base("name=InscripcionesBrDbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Adquiriente> Adquiriente { get; set; }
        public virtual DbSet<CNEOptions> CNEOptions { get; set; }
        public virtual DbSet<ComunaOptions> ComunaOptions { get; set; }
        public virtual DbSet<Enajenacion> Enajenacion { get; set; }
        public virtual DbSet<Enajenante> Enajenante { get; set; }
    }
    public class EnajenacionViewModel
    {
        public Enajenacion Enajenacion { get; set; }
        public List<Enajenacion> Enajenacions { get; set; }
        public List<Enajenante> Enajenantes { get; set; }
        public List<Adquiriente> Adquirientes { get; set; }
        public List<CNEOptions> CNEOptions { get; set; }
        public List<ComunaOptions> ComunaOptions { get; set; }
        public List<String> Descripcion { get; set; }
        public String SelectDescripcion { get; set; }
        public List<String> Comuna { get; set; }
        public String SelectComuna { get; set; }
        public int? Year { get; set; }

    }
}

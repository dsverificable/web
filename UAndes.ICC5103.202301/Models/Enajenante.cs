//------------------------------------------------------------------------------
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
    
    public partial class Enajenante
    {
        public int Id { get; set; }
        public string RutEnajenante { get; set; }
        public double PorcentajeEnajenante { get; set; }
        public bool CheckEnajenante { get; set; }
        public int IdEnajenacion { get; set; }
    
        public virtual Enajenacion Enajenacion { get; set; }
    }
}

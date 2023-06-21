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
    
    public partial class Enajenacion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Enajenacion()
        {
            this.Adquiriente = new HashSet<Adquiriente>();
            this.Enajenante = new HashSet<Enajenante>();
        }
    
        public int Id { get; set; }
        public bool Vigente { get; set; }
        public int CNE { get; set; }
        public int Comuna { get; set; }
        public int Manzana { get; set; }
        public int Predio { get; set; }
        public int Fojas { get; set; }
        public System.DateTime FechaInscripcion { get; set; }
        public int IdInscripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adquiriente> Adquiriente { get; set; }
        public virtual CNEOptions CNEOptions { get; set; }
        public virtual ComunaOptions ComunaOptions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enajenante> Enajenante { get; set; }
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace kredi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class LinesOfCredit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LinesOfCredit()
        {
            this.Movements = new HashSet<Movements>();
        }
    
        public int id { get; set; }
        [Required]
        public string customer { get; set; }
        [Required]
        public float amount { get; set; }
        [Required]
        public string currency { get; set; }
        [Required]
        public System.DateTime creationDate { get; set; }
        [Required]
        public string rateType { get; set; }
        [Required]
        public float rateValue { get; set; }
        [Required]
        public string capitalization { get; set; }
        [Required]
        public int user_id { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movements> Movements { get; set; }
    }
}

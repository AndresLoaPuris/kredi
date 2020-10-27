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

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El nombre del cliente es requirido !")]
        public string customer { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Debe ser un valor positivo !")]
        [Required(ErrorMessage = "El limite de crédito es requirido !")]
        public float amount { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El tipo de moneda es requirido !")]
        public string currency { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha de creación es requirido !")]
        public System.DateTime creationDate { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "El tipo de tasa es requirido !")]
        public string rateType { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Debe ser un valor positivo !")]
        [Required(ErrorMessage = "La tasa de interés es requirido !")]
        public float rateValue { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "La capitalización es requirido !")]
        public string capitalization { get; set; }

        [Required]
        public int user_id { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Movements> Movements { get; set; }
    }
}

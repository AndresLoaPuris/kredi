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

    public partial class Movements
    {
        public int id { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "La fecha es requirido !")]
        public System.DateTime motionDay { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "La descripción es requirido !")]
        public string description { get; set; }

        [Range(0, Double.PositiveInfinity, ErrorMessage = "Debe ser un valor positivo !")]
        [Required(ErrorMessage = "El monto es requirido !")]
        public float movementValue { get; set; }

        [Required]
        public bool isMoneyWithdrawal { get; set; }

        [Required]
        public float balance { get; set; }

        [Required]
        public int LinesOfCredit_id { get; set; }
    
        public virtual LinesOfCredit LinesOfCredit { get; set; }
    }
}

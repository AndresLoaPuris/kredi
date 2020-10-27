using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kredi.Models
{
	public class AuthUser
	{
		public int Id { get; set; }

		[DataType(DataType.EmailAddress)]
		[Required(ErrorMessage = "El correo es requirido !")]
		public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "La contraseña es requirido !")]
		public string Password { get; set; }
	}
}
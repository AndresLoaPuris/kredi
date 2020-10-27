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

		[Required(ErrorMessage = "Error : Ingresar correo")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Error : Ingresar contraseña")]
		public string Password { get; set; }
	}
}
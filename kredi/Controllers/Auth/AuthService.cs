using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kredi.Controllers.Auth
{
	public class AuthService
	{
		private SI642Entities db = new SI642Entities();

		public bool getAccess(AuthUser authUser)
		{
			return db.Users.Any(x => x.email == authUser.Email && x.password == authUser.Password);
		}

		public bool getUserExists(Users authUser)
		{
			return db.Users.Any(x => x.email == authUser.email || (x.names == authUser.names && x.surnames == authUser.surnames));
		}

		public bool getEmailExists(AuthUser authUser)
		{
			return db.Users.Any(x => x.email == authUser.Email);
		}
		public string getNameUser(AuthUser authUser)
		{
			return db.Users.Where(x => x.email == authUser.Email).FirstOrDefault<Users>().names +" "+ db.Users.Where(x => x.email == authUser.Email).FirstOrDefault<Users>().surnames;
		}

		public string getPassword(AuthUser authUser)
		{
			return db.Users.Where(x => x.email == authUser.Email).FirstOrDefault().password;
		}

		public void signUp(Users users)
		{
			db.Users.Add(users);
			db.SaveChanges();
		}
	}
}
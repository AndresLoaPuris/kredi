using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kredi.Controllers.Home
{
	public class HomeService
	{
		private SI642Entities db = new SI642Entities();

		public int getIdbyUser(string emailUser)
		{
			return db.Users.Where(x => x.email == emailUser).FirstOrDefault().id;
		}

		public IEnumerable<kredi.Models.LinesOfCredit> allClients(string emailUser)
		{
			return db.LinesOfCredit.Where(x => x.Users.email == emailUser);
		}
		public void addClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
			db.LinesOfCredit.Add(linesOfCredit);
			db.SaveChanges();
		}
	}
}
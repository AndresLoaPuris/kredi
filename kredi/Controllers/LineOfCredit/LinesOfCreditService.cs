using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kredi.Controllers.LineOfCredit
{
	public class LinesOfCreditService
	{
		private SI642Entities db = new SI642Entities();

		public IEnumerable<kredi.Models.Movements> last5Movements(int id)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id).OrderByDescending(x => x.motionDay).Take(5);
		}
		public IEnumerable<kredi.Models.Movements> allMovements(int id)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id).OrderBy(x => x.motionDay);
		}
		public void addMovement(kredi.Models.Movements movements)
		{
			db.Movements.Add(movements);
			db.SaveChanges();
		}
		public float lastBalance(int id)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id && x.id == db.Movements.Where(p => p.LinesOfCredit_id == id).Max(p => p.id)).FirstOrDefault().balance ;
		}

		public LinesOfCredit getLinesOfCreditById(int id)
		{
			return db.LinesOfCredit.Find(id);
		}

		public string currencyType(int id)
		{
			return db.LinesOfCredit.Where(x => x.id == id).FirstOrDefault().currency;
		}
	}
}

// last 5 movements
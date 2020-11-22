using kredi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace kredi.Controllers.LineOfCredit
{
	public class LinesOfCreditService
	{
		private SI642Entities db = new SI642Entities();

		public IEnumerable<kredi.Models.Movements> last5Movements(int id)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id).OrderByDescending(x => x.id).Take(5);
		}

		public float usedCreditLine(int id, DateTime hoy)
		{
			
			List<kredi.Models.Movements> movementsDue = db.Movements.Where(x => x.LinesOfCredit_id == id && x.isPaid == false && x.consumptionDate <= hoy && x.isEnabled == true).ToList();

			float amountDue = 0.0f;
			foreach (var item in movementsDue) {
				amountDue += item.movementValue;
			}

			float amountLineOfCredit = db.LinesOfCredit.Where(x => x.id == id).FirstOrDefault().amount;
			
			return (amountLineOfCredit - amountDue);
		}


		private float calculateInterest(int _movement, int LineOfCredit, DateTime datePay) { 
			// TODO
			var lineOfCredit = db.LinesOfCredit.Find(LineOfCredit);
			var movement = db.Movements.Find(_movement);
			

			return 5.0f;
		}

		public float amountToBePaid(int LineOfCredit, DateTime datePay)
		{
			
			List<kredi.Models.Movements> movementsDue = db.Movements.Where(x => x.LinesOfCredit_id == LineOfCredit && x.isPaid == false && x.consumptionDate <= datePay && x.isEnabled == true).ToList();

			float amountDue = 0.0f;
			foreach (var item in movementsDue)
			{
				amountDue += (item.movementValue + calculateInterest(item.id,LineOfCredit,datePay));
			}
			return amountDue;
		}


		public List<kredi.Models.Movements> movementsToCancel(int LineOfCredit, DateTime datePay)
		{
			
			var movementsDue = db.Movements.Where(x => x.LinesOfCredit_id == LineOfCredit && x.isPaid == false && x.consumptionDate <= datePay && x.isEnabled == true).ToList();

			return movementsDue;
		}

		public void editMovements(kredi.Models.Movements movement)
		{

			db.Entry(movement).State = EntityState.Modified;
			db.SaveChanges();
		}


		public IEnumerable<kredi.Models.Movements> allMovements(int id)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id).OrderBy(x => x.consumptionDate);
		}

		public void addMovement(kredi.Models.Movements movements)
		{
			db.Movements.Add(movements);
			db.SaveChanges();
		}
		

		public LinesOfCredit getLinesOfCreditById(int id)
		{
			return db.LinesOfCredit.Find(id);
		}

		public string currencyType(int id)
		{
			return db.LinesOfCredit.Where(x => x.id == id).FirstOrDefault().currency;
		}

		public class TemplateType
		{
			public int Index { get; set; }
			public string Name { get; set; }
		}

		public IEnumerable<TemplateType> IEcurrencyType = new List<TemplateType>() {
				new TemplateType { Index = 0, Name="PEN"},
				new TemplateType { Index = 1, Name="USD"}
			}.AsEnumerable();
	}


}


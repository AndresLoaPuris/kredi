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
			return db.Movements.Where(x => x.LinesOfCredit_id == id && x.isPaid == false).OrderByDescending(x => x.id).Take(5);
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

		static int rateTimeToDays(string rateTime)
		{
			int days = 0;

			if (rateTime == "Anual")
			{
				days = 360;
			}
			else if (rateTime == "Semestral")
			{
				days = 180;
			}
			else if (rateTime == "Cuatrimestral")
			{
				days = 120;
			}
			else if (rateTime == "Trimestral")
			{
				days = 90;
			}
			else if (rateTime == "Bimestral")
			{
				days = 60;
			}
			else if (rateTime == "Mensual")
			{
				days = 30;
			}
			else if (rateTime == "Quincenal")
			{
				days = 15;
			}
			else if (rateTime == "Semanal")
			{
				days = 7;
			}
			else if (rateTime == "Diaria")
			{
				days = 1;
			}
			return days;
		}

		private float calculateInterest(int _movement, int LineOfCredit, DateTime datePay) { 
			// TODO
			var lineOfCredit = db.LinesOfCredit.Find(LineOfCredit);
			var movement = db.Movements.Find(_movement);

			float interest = 0.0f;
			float C = movement.movementValue;
			float i = 0.0f;
			if (lineOfCredit.rateType == "Simple")
			{
				i = (lineOfCredit.rateValue*(360/ rateTimeToDays(lineOfCredit.rateTime))) / 100.0f;

			}
			else {
				i = lineOfCredit.rateValue / 100.0f;

			}
			TimeSpan tSpan = datePay - movement.consumptionDate;
			int days_passed = tSpan.Days;

			float n = 0.0f;
			float m = 0.0f;

			float NT = lineOfCredit.rateValue / 100.0f;

			if (!(lineOfCredit.rateType == "Efectiva" || lineOfCredit.rateType == "Simple")) {
				n = (float)days_passed / float.Parse(lineOfCredit.capitalization);
				m = (float)rateTimeToDays(lineOfCredit.rateTime) / float.Parse(lineOfCredit.capitalization);
			}
			

			float TEP = lineOfCredit.rateValue / 100.0f;
			int daysTEP = rateTimeToDays(lineOfCredit.rateTime);

			if (lineOfCredit.rateType == "Nominal")
			{

				interest = C * Convert.ToSingle(Math.Pow((1 + (NT / m)), n)) - C;
			}
			else if (lineOfCredit.rateType == "Simple")
			{
				interest = C * i * (days_passed / 360.0f);
			}
			else if (lineOfCredit.rateType == "Efectiva")
			{
				
				interest = C * Convert.ToSingle(Math.Pow((1 + TEP), ((float)days_passed / (float)daysTEP))) - C;
			}

			return Convert.ToSingle(Math.Round(interest, 1)); ;

			
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
			return db.Movements.Where(x => x.LinesOfCredit_id == id).OrderByDescending(x => x.id);
		}



		public IEnumerable<kredi.Models.Movements> allMovementsFilter(int id, DateTime date0, DateTime date1)
		{
			return db.Movements.Where(x => x.LinesOfCredit_id == id && x.consumptionDate >= date0 && x.consumptionDate <= date1).OrderByDescending(x => x.id);
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


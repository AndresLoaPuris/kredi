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

		public class TemplateType
		{
			public int Index { get; set; }
			public string Name { get; set; }
		}

		public IEnumerable<TemplateType> IEcurrencyType = new List<TemplateType>() {
				new TemplateType { Index = 0, Name="PEN"},
				new TemplateType { Index = 1, Name="USD"}
			}.AsEnumerable();


		public float calculatingDebtToPay(int id)
		{
			IEnumerable<kredi.Models.Movements> movementsList = allMovements(id);
			LinesOfCredit linesOfCredit = getLinesOfCreditById(id);

			switch (linesOfCredit.rateType)
			{
				case "Tasa de interés Simple Anual":

					break;

				case "Tasa de interés Nominal Anual":
					int m = 360 / int.Parse(linesOfCredit.capitalization);
					int n = (System.DateTime.Now.Date - linesOfCredit.creationDate.Date).Days / int.Parse(linesOfCredit.capitalization); // TODO : se redondea ?
					float sum = 0.0f;
					foreach (var item in movementsList)
					{
						if (item.isMoneyWithdrawal)
						{
							sum += Convert.ToSingle(Math.Pow(item.movementValue * (1 + ((linesOfCredit.rateValue / 100) / m)), n));
						}
					}
					return Math.Abs(lastBalance(id)) + sum;

				case "Tasa de interés Efectiva Anual":
					foreach (var item in movementsList)
					{

					}
					break;

			}

			return 0.0f;
		}
	}


}

// last 5 movements
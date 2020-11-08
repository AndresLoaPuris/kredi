using kredi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

		public kredi.Models.LinesOfCredit getClientById(int id)
		{
			return db.LinesOfCredit.Find(id);
		}

		public void addClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
			db.LinesOfCredit.Add(linesOfCredit);
			db.SaveChanges();
		}


		public void editClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
			
			db.Entry(linesOfCredit).State = EntityState.Modified;
			db.SaveChanges();
		}


		public class TemplateType
		{
			public string Value { get; set; }
			public string Name { get; set; }
		}

		public IEnumerable<TemplateType> IEcurrencyType = new List<TemplateType>() {
				new TemplateType { Value = "PEN", Name="PEN"},
				new TemplateType { Value = "USD", Name="USD"}
			}.AsEnumerable();

		public IEnumerable<TemplateType> IEcapitalizationType = new List<TemplateType>() {
				new TemplateType { Value = "1", Name="diaria"},
				new TemplateType { Value = "15", Name="quincenal"},
				new TemplateType { Value = "30", Name="mensual"},
				new TemplateType { Value = "60", Name="bimestral"},
				new TemplateType { Value = "90", Name="trimestal"},
				new TemplateType { Value = "120", Name="cuatrimestral"},
				new TemplateType { Value = "180", Name="semestral"},
				new TemplateType { Value = "360", Name="anual"},

			}.AsEnumerable();

		public IEnumerable<TemplateType> IErateType = new List<TemplateType>() {
				new TemplateType { Value = "Simple", Name="Simple"},
				new TemplateType { Value = "Nominal", Name="Nominal"},
				new TemplateType { Value = "Efectiva", Name="Efectiva"}
			}.AsEnumerable();

		public IEnumerable<TemplateType> IErateTime = new List<TemplateType>() {
				new TemplateType { Value = "1", Name="Diaria"},
				new TemplateType { Value = "15", Name="Quincenal"},
				new TemplateType { Value = "30", Name="Mensual"},
				new TemplateType { Value = "60", Name="Bimestral"},
				new TemplateType { Value = "90", Name="Trimestal"},
				new TemplateType { Value = "120", Name="Cuatrimestral"},
				new TemplateType { Value = "180", Name="Semestral"},
				new TemplateType { Value = "360", Name="Anual"},
			}.AsEnumerable();
	}
}
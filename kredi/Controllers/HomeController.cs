using kredi.Controllers.Home;
using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kredi.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private HomeService homeService = new HomeService();

		public class TemplateType {
			public string Value { get; set; }
			public string Name { get; set; }
		}

		private IEnumerable<TemplateType> IEcurrencyType = new List<TemplateType>() {
				new TemplateType { Value = "PEN", Name="PEN"},
				new TemplateType { Value = "USD", Name="USD"}
			}.AsEnumerable();

		private IEnumerable<TemplateType> IEcapitalizationType = new List<TemplateType>() {
				new TemplateType { Value = "1", Name="diaria"},
				new TemplateType { Value = "15", Name="quincenal"},
				new TemplateType { Value = "30", Name="mensual"},
				new TemplateType { Value = "60", Name="bimestral"},
				new TemplateType { Value = "90", Name="trimestal"},
				new TemplateType { Value = "120", Name="cuatrimestral"},
				new TemplateType { Value = "180", Name="semestral"},
				new TemplateType { Value = "360", Name="anual"},

			}.AsEnumerable();

		private IEnumerable<TemplateType> IErateType = new List<TemplateType>() {
				new TemplateType { Value = "TSA", Name="Tasa de interés Simple Anual"},
				new TemplateType { Value = "TNA", Name="Tasa de interés Nominal Anual"}, 
				new TemplateType { Value = "TEA", Name="Tasa de interés Efectiva Anual"}
			}.AsEnumerable();

		public ActionResult Index()
		{

			ViewBag.rateType = new SelectList(IErateType, "Name", "Name");
			ViewBag.capitalization = new SelectList(IEcapitalizationType, "Value", "Name");
			ViewBag.currency = new SelectList(IEcurrencyType, "Name", "Name");
			ViewBag.Clients = homeService.allClients(AuthController.staticEmail);

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult AddClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
						
			linesOfCredit.user_id = homeService.getIdbyUser(AuthController.staticEmail);
			homeService.addClient(linesOfCredit);
			return RedirectToAction("Index");

		}

	}
}
using kredi.Controllers.Home;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace kredi.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private HomeService homeService = new HomeService();
		private static System.DateTime staticCreationDate { get; set; }


		public ActionResult Index()
		{
			ViewBag.rateType = new SelectList(homeService.IErateType, "Name", "Name");
			ViewBag.rateTime = new SelectList(homeService.IErateTime, "Name", "Name");
			ViewBag.capitalization = new SelectList(homeService.IEcapitalizationType, "Value", "Name");
			ViewBag.currency = new SelectList(homeService.IEcurrencyType, "Name", "Name");
			ViewBag.Clients = homeService.allClients(AuthController.staticEmail);
			return View();
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult AddClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
			if (linesOfCredit.capitalization == null) 
			{
				linesOfCredit.capitalization = "no-capitalization";
			}

			linesOfCredit.user_id = homeService.getIdbyUser(AuthController.staticEmail);
			homeService.addClient(linesOfCredit);
			return RedirectToAction("Index");
		}


		public ActionResult EditClient(int id)
		{
			kredi.Models.LinesOfCredit linesOfCredit = homeService.getClientById(id);
			staticCreationDate = linesOfCredit.creationDate;
			ViewBag.rateType = new SelectList(homeService.IErateType, "Name", "Name",linesOfCredit.rateType);
			ViewBag.rateTime = new SelectList(homeService.IErateTime, "Name", "Name",linesOfCredit.rateTime);
			ViewBag.capitalization = new SelectList(homeService.IEcapitalizationType, "Value", "Name",linesOfCredit.capitalization);
			ViewBag.currency = new SelectList(homeService.IEcurrencyType, "Name", "Name",linesOfCredit.currency);
			ViewBag.Clients = homeService.allClients(AuthController.staticEmail);
			return View(linesOfCredit);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[HandleError]
		public ActionResult EditClient(kredi.Models.LinesOfCredit linesOfCredit)
		{
			linesOfCredit.user_id = homeService.getIdbyUser(AuthController.staticEmail);
			linesOfCredit.creationDate = staticCreationDate;
			if (linesOfCredit.capitalization == null)
			{
				linesOfCredit.capitalization = "no-capitalization";
			}

			homeService.editClient(linesOfCredit);
			return RedirectToAction("Index");
		}

	}
}
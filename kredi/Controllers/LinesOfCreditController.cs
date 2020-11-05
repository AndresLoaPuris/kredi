using HtmlAgilityPack;
using kredi.Controllers.LineOfCredit;
using kredi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kredi.Controllers
{
    [Authorize]
    public class LinesOfCreditController : Controller
    {
        private LinesOfCreditService linesOfCreditService = new LinesOfCreditService();
        public static int staticId { get; set; }
        private static float staticDollar { get; set; }


        public ActionResult Index(int id)
        {
            staticId = id;
            staticDollar = 3.61f; // TODO
            ViewBag.ToPay = linesOfCreditService.calculatingDebtToPay(id); // TODO
            ViewBag.Movements = linesOfCreditService.last5Movements(id);
            ViewBag.LinesOfCredit_id = new SelectList(linesOfCreditService.IEcurrencyType, "Index", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult RegisterExpenditure(kredi.Models.Movements movements)
        {
            if (linesOfCreditService.allMovements(staticId).Count() != 0) {
                movements.balance = linesOfCreditService.lastBalance(staticId);
            }
            
            movements.isMoneyWithdrawal = true;

            if (movements.LinesOfCredit_id == 0 && linesOfCreditService.currencyType(staticId) == "USD")
            {
                movements.movementValue = movements.movementValue / staticDollar;
            }

            if (movements.LinesOfCredit_id == 1 && linesOfCreditService.currencyType(staticId) == "PEN")
            {
                movements.movementValue = movements.movementValue * staticDollar;
            }

            movements.balance += (movements.movementValue)*-1;
            movements.LinesOfCredit_id = staticId;
            linesOfCreditService.addMovement(movements);
            return RedirectToAction("Index", new { id = staticId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult RegisterPayment(kredi.Models.Movements movements)
        {
            if (linesOfCreditService.allMovements(staticId).Count() != 0)
            {
                movements.balance = linesOfCreditService.lastBalance(staticId);
            }
            movements.isMoneyWithdrawal = false;

            if (movements.LinesOfCredit_id == 0 && linesOfCreditService.currencyType(staticId) == "USD")
            {
                movements.movementValue = movements.movementValue / staticDollar;
            }

            if (movements.LinesOfCredit_id == 1 && linesOfCreditService.currencyType(staticId) == "PEN")
            {
                movements.movementValue = movements.movementValue * staticDollar;
            }

            movements.motionDay = System.DateTime.Now.Date;
            movements.balance += movements.movementValue;
            movements.LinesOfCredit_id = staticId;
            movements.description = "ingreso de efectivo a la cuenta";
            linesOfCreditService.addMovement(movements);
            return RedirectToAction("Index", new { id = staticId });
        }
    }
}
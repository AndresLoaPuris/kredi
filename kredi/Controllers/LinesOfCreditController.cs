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
        public static float staticDollar { get; set; }

        public class TemplateType
        {
            public int Index { get; set; }
            public string Name { get; set; }
        }

        private IEnumerable<TemplateType> IEcurrencyType = new List<TemplateType>() {
                new TemplateType { Index = 0, Name="PEN"},
                new TemplateType { Index = 1, Name="USD"}
            }.AsEnumerable();

        public ActionResult Index(int id)
        {
            staticId = id;
            staticDollar = 3.61f;
            ViewBag.Dollar = staticDollar;
            ViewBag.ToPay = calculatingDebtToPay(id); // TODO logica ?
            ViewBag.Movements = linesOfCreditService.last5Movements(id);
            ViewBag.LinesOfCredit_id = new SelectList(IEcurrencyType, "Index", "Name");
            return View();
        }


        float calculatingDebtToPay(int id) {
            IEnumerable<kredi.Models.Movements> movementsList = linesOfCreditService.allMovements(id);
            LinesOfCredit linesOfCredit = linesOfCreditService.getLinesOfCreditById(id);

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
                        if (item.isMoneyWithdrawal) {
                            sum += Convert.ToSingle(Math.Pow(item.movementValue * (1 + ((linesOfCredit.rateValue / 100) / m)), n));
                        }
                    }
                    return Math.Abs(linesOfCreditService.lastBalance(id)) + sum;

                case "Tasa de interés Efectiva Anual":
                    foreach (var item in movementsList)
                    {

                    }
                    break;
 
            }

            return 0.0f;
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
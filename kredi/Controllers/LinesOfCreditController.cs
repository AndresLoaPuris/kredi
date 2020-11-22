using HtmlAgilityPack;
using kredi.Controllers.LineOfCredit;
using kredi.Models;
using kredi.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace kredi.Controllers
{
    [Authorize]
    public class LinesOfCreditController : Controller
    {
        private LinesOfCreditService linesOfCreditService = new LinesOfCreditService();
        public static int staticId { get; set; }

        public static bool filtrado { get; set; }

        public static IEnumerable<kredi.Models.Movements> elemtosfiltrados { get; set; }

        public static DateTime staticDatePay { get; set; }
        private static float staticDollar { get; set; }

        private ChangeService changeService = new ChangeService();

        [HttpPost]
        public JsonResult Temporalyu(string datePay)
        {
            staticDatePay = Convert.ToDateTime(datePay);
            string data = linesOfCreditService.amountToBePaid(staticId, staticDatePay).ToString() + "  " + linesOfCreditService.currencyType(staticId);


            return Json(data);
            
        }

        
                [HttpPost]
        public JsonResult entretiempo(string dayO, string day1)
        {
            elemtosfiltrados = linesOfCreditService.allMovementsFilter(staticId, Convert.ToDateTime(dayO).Date, Convert.ToDateTime(day1).Date);
            filtrado = true;
            return Json(true);

        }

        [HttpPost]
        public JsonResult saldoDisponible(string dataSaldo)
        {
           
            return Json(linesOfCreditService.usedCreditLine(staticId, Convert.ToDateTime(dataSaldo).Date).ToString("0.00") + "  "+ linesOfCreditService.currencyType(staticId));

        }

        public ActionResult Index(int id)
        {
            staticId = id;
            staticDollar = changeService.getData("https://app.dollarhouse.pe/calculadora", "//*[@id='buy-exchange-rate']", "//*[@id='sell-exchange-rate']", "Dollar House").buy; ; // TODO
            ViewBag.ToPay = 0.0f;
            ViewBag.Movements = linesOfCreditService.last5Movements(id);
            ViewBag.LinesOfCredit_id = new SelectList(linesOfCreditService.IEcurrencyType, "Index", "Name");
            filtrado = false;
            return View();
        }


        public ActionResult Report()
        {
            if (filtrado)
            {
                ViewBag.Movements = elemtosfiltrados;
                filtrado = false;

            }
            else {
                ViewBag.Movements = linesOfCreditService.allMovements(staticId);
            }
            
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult RegisterExpenditure(kredi.Models.Movements movement)
        {
            movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue,1));

            if (movement.LinesOfCredit_id == 0 && linesOfCreditService.currencyType(staticId) == "USD")
            {
                movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue / staticDollar, 1));
            }

            if (movement.LinesOfCredit_id == 1 && linesOfCreditService.currencyType(staticId) == "PEN")
            {
                movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue * staticDollar, 1));
            }

            movement.paymentDate = Convert.ToDateTime("01/01/0001");
            movement.isPaid = false;
            movement.isEnabled = true;
            movement.LinesOfCredit_id = staticId;
            linesOfCreditService.addMovement(movement);
            return RedirectToAction("Index", new { id = staticId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HandleError]
        public ActionResult RegisterPayment(kredi.Models.Movements movement)
        {   
            // -- primero : registrar el pago
            movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue, 1));

            if (movement.LinesOfCredit_id == 0 && linesOfCreditService.currencyType(staticId) == "USD")
            {
                movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue / staticDollar, 1));
            }

            if (movement.LinesOfCredit_id == 1 && linesOfCreditService.currencyType(staticId) == "PEN")
            {
                movement.movementValue = Convert.ToSingle(Math.Round(movement.movementValue * staticDollar, 1));
            }


            movement.description = "pago parcial de la cuenta";
            movement.paymentDate = staticDatePay.Date; 
            movement.consumptionDate = staticDatePay.Date; 
            movement.isPaid = true;
            movement.isEnabled = true;
            movement.LinesOfCredit_id = staticId;
            linesOfCreditService.addMovement(movement);


            // -- segundo : registrar "nuevo produtco " si falta pagar 
            if (movement.movementValue < linesOfCreditService.amountToBePaid(staticId, staticDatePay))
            {
                kredi.Models.Movements new_movement = new Movements();
                new_movement.movementValue = Convert.ToSingle(Math.Round(linesOfCreditService.amountToBePaid(staticId, staticDatePay) - movement.movementValue, 1));
                new_movement.description = "deuda a pagar";
                new_movement.paymentDate = Convert.ToDateTime("01/01/0001");
                new_movement.consumptionDate = staticDatePay; 
                new_movement.isPaid = false;
                new_movement.isEnabled = true;
                new_movement.LinesOfCredit_id = staticId;
                linesOfCreditService.addMovement(new_movement);


                var movementsToCancel = linesOfCreditService.movementsToCancel(staticId, staticDatePay);
                for (int i = 0; i < movementsToCancel.Count() - 1; i++)
                {
                    movementsToCancel[i].paymentDate = staticDatePay.Date; 
                    movementsToCancel[i].isEnabled = false;
                    linesOfCreditService.editMovements(movementsToCancel[i]);
                }
            }
            else{
                var movementsToCancel = linesOfCreditService.movementsToCancel(staticId, staticDatePay);
                for (int i = 0; i < movementsToCancel.Count(); i++)
                {
                    movementsToCancel[i].paymentDate = staticDatePay.Date; 
                    movementsToCancel[i].isEnabled = false;
                    linesOfCreditService.editMovements(movementsToCancel[i]);
                }
            }

            // tercero : cambiar los estados a true de los movimeintos a hoy

           
           
            return RedirectToAction("Index", new { id = staticId });
        }

    }
}
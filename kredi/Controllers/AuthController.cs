
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using kredi.Controllers.Auth;
using kredi.Models;

namespace kredi.Controllers
{
    public class AuthController : Controller
    {
        
        private AuthService authService = new AuthService();
        public static string staticEmail { get; set; }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthUser authUser)
        {
            if (authService.getAccess(authUser))
            {

                FormsAuthentication.SetAuthCookie(authService.getNameUser(authUser), false);
                staticEmail = authUser.Email;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "correo y/o contraseña invalido");
            return View();
        }

        public ActionResult passwordRecovery()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult passwordRecovery(AuthUser authUser)
        {
            if (authService.getEmailExists(authUser))
            {
                string password = authService.getPassword(authUser);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("company.kredi@gmail.com");
                mailMessage.To.Add(authUser.Email);
                mailMessage.Subject = ("Recuperación de su contraseña");
                mailMessage.Body = "¡Pero no te preocupes! Aqui esta su contraseña para el uso de nuestra aplicación Kredi : " + password;
                mailMessage.Priority = MailPriority.Normal;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Credentials = new NetworkCredential("company.kredi@gmail.com", "CompanyKredi#1");
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                return RedirectToAction("Login");
                
            }

            ModelState.AddModelError("", "correo no existente");
            return View();
        }

        public ActionResult SignUp()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "id,names,surnames,email,password,nameOfThePlace")] Users users)
        {
            if (!authService.getUserExists(users))
            {
                if (ModelState.IsValid)
                {
                    authService.signUp(users);
                    return RedirectToAction("Login");
                }
            }

            ModelState.AddModelError("", "usuario y/o correo existente");
            return View(users);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}

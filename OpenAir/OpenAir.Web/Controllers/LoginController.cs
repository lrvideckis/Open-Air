using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAir.Data;
using OpenAir.Data.Models;

namespace OpenAir.Web.Controllers
{
    /* Controller that handles all actions pertaining to logging into the application */
    public class LoginController : Controller
    {

        private readonly PilotRepo _db;

        /* Set context upon instantiation of controller */
        public LoginController(PilotRepo db)
        {
            _db = db;
        }

        /* Action for clicking on Login tab on the navigation bar shared between all views. Displays the login view */
        public IActionResult Login()
        {
            ViewBag.UserLoginStatus = "success";
            return View();
        }

        /* Action for clicking on Logout tab on the navigation bar shared between all views. Displays the index view  */
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            HttpContext.Session.Remove("name");
            HttpContext.Session.Remove("isAdmin");
            return View("../Home/Index");
        }

        /* Action for clicking on the Login submit button on the login page. This ensures that the information entered is correct */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> LoginSubmitAsync(string email, string pwd)
        {
            var user = await _db.GetUser(email);

            //Specify on next view what the error was using ViewBag to send message
            if (user == null || ShaHash.ComputeSha256Hash(pwd) != user.Pwd)
            {
                ViewBag.UserLoginStatus = "failed";
                return View("../Login/Login");
            }

            ViewBag.UserLoginStatus = "success";

            HttpContext.Session.SetString("username", email);
            HttpContext.Session.SetString("name", user.Name);
            HttpContext.Session.SetInt32("isAdmin", user.IsAdmin ? 1 : 0);

            return View("../Home/Index");
        }
    }
}
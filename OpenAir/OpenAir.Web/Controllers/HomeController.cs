using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAir.Data.Models;
using OpenAir.Web.Models;
using OpenAir.Data;

namespace OpenAir.Web.Controllers
{
    public class HomeController : Controller
    {
        PilotRepo _db_; 

        /*set pilot repo pattern upon instantiation of about controller*/
        public HomeController(PilotRepo db) {
            _db_ = db;
        }

        /*this controller returns the index view which is the main/home view. If the user is logged in, then
         that information is passed to the view, so that different tabs can be displayed on the top.*/
        public IActionResult Index()
        {
            /* //----Used for logging in faster for testing---
            string email = "admin@admin.com";
            var user = _db_.GetUser(email).Result;
            user.isAdmin = true;

            HttpContext.Session.SetString("username", email);
            HttpContext.Session.SetString("name", user.Name);
            HttpContext.Session.SetInt32("isAdmin", user.isAdmin ? 1 : 0);
            */
            return View();
        }
    }
}

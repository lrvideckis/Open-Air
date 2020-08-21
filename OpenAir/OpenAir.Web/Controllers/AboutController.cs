using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAir.Data.Models;
using OpenAir.Data;

namespace OpenAir.Web.Controllers
{
    public class AboutController : Controller
    {
        /*set pilot repo pattern upon instantiation of about controller*/
        public AboutController() {}
        /*controller which returns about razor view. It checks if the user is logged in. If the user is logged in,
         then the _Layout shared view will display something simalar to "hello <user's name> at the top*/
        public IActionResult About()
        {
            return View();
        }
    }
}
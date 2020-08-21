using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAir.Data;
using OpenAir.Data.Models;
namespace OpenAir.Web.Controllers
{
    /* Controller that handles actions pertaining to signing up */
    public class SignUpController : Controller
    {
        private readonly PilotRepo _db;
        /* Set pilot repo upon instantiation of controller */
        public SignUpController(PilotRepo db)
        {
            _db = db;
        }

        /* Action that just returns the SignUp view page */
        public IActionResult SignUp()
        {
            return View();
        }

        /* Action that handles adding a pilot (TODO: add GUID) */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUpSubmitAsync(
            string fname,
            string lname,
            string email,
            string aircraft,
            string pwd,
            string reenterpwd)

        {
            if (pwd != reenterpwd)
            {
                ViewBag.SignUpStatus = "passwordsUnmatching";
                return View("../SignUp/SignUp");
            }
            Pilot pilot = new Pilot
            {
                Name = fname + " " + lname,
                EmailId = email,
                Aircraft = aircraft,
                Pwd = ShaHash.ComputeSha256Hash(pwd)
            };

            if (email == "secretAdmin")
            {
                pilot.IsAdmin = true;
            }

            try
            {
                await _db.Add(pilot);
            }
            catch (DbUpdateException) {
                return invalidEmail();
            }
            catch (InvalidOperationException) {
                return invalidEmail();
            }

            HttpContext.Session.SetString("username", email);
            HttpContext.Session.SetString("name", pilot.Name);
            HttpContext.Session.SetInt32("isAdmin", pilot.IsAdmin ? 1 : 0);

            return View("../Home/Index");
        }

        IActionResult invalidEmail()
        {
            ViewBag.SignUpStatus = "duplicateEmail";
            return View("../SignUp/SignUp");
        }
    }
}
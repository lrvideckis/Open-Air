using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAir.Data;
using OpenAir.Data.Models;

namespace OpenAir.Web.Controllers
{
    public class FlightsController : Controller
    {
        private readonly FlightRepo _db_flights; 
        private readonly RouteRepo _db_route;
        private readonly PilotRepo _db_pilot;
        /*set pilot repo pattern upon instantiation of flights controller*/
        public FlightsController(FlightRepo db_flights, RouteRepo db_route, PilotRepo db_pilot)
        {
            _db_flights = db_flights;
            _db_route = db_route;
            _db_pilot = db_pilot;
        }
        /*this returns the flight razor view. A list of all the flights for the current user is passed to
         the view. The flights view will then display all the pilot's flights*/
        public async System.Threading.Tasks.Task<IActionResult> FlightsAsync(string email)
        {
            try
            {
                if (email == null)
                    email = HttpContext.Session.GetString("username");

                await SetViewBagAsync(email);
                return View();
            }
            catch (DbUpdateException) { }
            catch (InvalidOperationException) { }
            return Content("Todo 5");
        }

        public async System.Threading.Tasks.Task<IActionResult> RemoveFlightAsync(DateTime takeOff, string route, string email)
        {
            try 
            {
                await _db_flights.Delete(takeOff, route);
                if (email == null) 
                    email = HttpContext.Session.GetString("username");

                await SetViewBagAsync(email);
                return View("../Flights/Flights");
            }
            catch (DbUpdateException) { }
            catch (InvalidOperationException) { }
            return Content("Todo 6");
        }

        public async System.Threading.Tasks.Task<IActionResult> ModifyFlightAsync(DateTime oldDateTime, string oldRouteId, DateTime newDate, DateTime newTime, string newRouteId, string email)
        {
            try
            {
                IFlight flight = await _db_flights.GetFlight(oldDateTime, oldRouteId);
                flight.TimeOfFlight = newDate.AddHours(newTime.Hour).AddMinutes(newTime.Minute);
                flight.RouteId = newRouteId;
                await _db_flights.Update(flight);

                if (email == null)
                    email = HttpContext.Session.GetString("username");

                await SetViewBagAsync(email);
                return View("../Flights/Flights");
            }
            catch (DbUpdateException) { }
            catch (InvalidOperationException) { }
            catch (NullReferenceException) { /*ask nate about a found bug here*/ }
            return Content("Todo 7");
        }

        private async System.Threading.Tasks.Task SetViewBagAsync(string email)
        {
            var getAllFlights = _db_flights.GetAll(email);
            ViewBag.dateSelected = DateTime.Now;
            ViewBag.email = email;
            ViewBag.Flights = await getAllFlights;
            ViewBag.PilotList = await PilotListGenerator.GetPilotEmailsAsync(_db_pilot);
            ViewBag.RouteList = await RouteListGenerator.GenerateRouteListAsync("VR-140", _db_route);
        }
    }
}

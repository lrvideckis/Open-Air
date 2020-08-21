using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenAir.Data;
using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAir.Web.Controllers
{
    /* Controller that handles actions pertaining to scheduling  */
    public class ScheduleController : Controller
    {
        private readonly PilotRepo _db_pilots;
        private readonly FlightRepo _db_flights;
        private readonly RouteRepo _db_route;

        /* Set context upon instantiation of controller */
        public ScheduleController(PilotRepo db_pilots, FlightRepo db_flights, RouteRepo db_route)
        {
            _db_pilots = db_pilots;
            _db_flights = db_flights;
            _db_route = db_route;
        }
        
        /* Action for clicking the schedule tab on the navigation bar shared between all views */
        public async System.Threading.Tasks.Task<IActionResult> ScheduleAsync()
        {
            DateTime now = DateTime.Now;
            try
            {
                string email = HttpContext.Session.GetString("username");
                Task<IEnumerable<IFlight>> allFlights;
                if (HttpContext.Session.GetInt32("isAdmin") == 1)
                {
                    email = "";
                    allFlights = _db_flights.GetAll(now);
                }
                else
                {
                    allFlights = _db_flights.GetAll(now, "VR-140", email);
                }
                ViewBag.dateSelected = now;
                ViewBag.email = email;
                ViewBag.Flights = await allFlights;
                ViewBag.PilotList = await PilotListGenerator.GetPilotEmailsAsync(_db_pilots);
                ViewBag.RouteList = await RouteListGenerator.GenerateRouteListAsync("VR-140", _db_route);

                ViewBag.RouteListRaw = await _db_route.GetAll();
                ViewBag.RouteSelected = "VR-140";
                return View();
            }
            catch (ArgumentNullException ) { /*Dillon & Luke do something here*/}
            return Content("Todo 4");
        }

        public async System.Threading.Tasks.Task<IActionResult> ScheduleSubmitAsync(DateTime currentDate, 
                                                                                            int hours,
                                                                                            int minutes,
                                                                                            string Route, 
                                                                                            string Callsign, 
                                                                                            float flightSpeed,
                                                                                            string pilotEmail)
        {
            if (pilotEmail == null)
                pilotEmail = HttpContext.Session.GetString("username");
            var getPilot = _db_pilots.GetUser(pilotEmail);
            try
            {
                Flight flight = new Flight
                {
                    FlightSpeed = flightSpeed,
                    RouteId = Route,
                    TimeOfFlight = currentDate.AddHours(hours).AddMinutes(minutes),
                    PilotEmailId = pilotEmail,
                    Pilot = (Pilot)await getPilot,
                    Route = (Route)await _db_route.GetRoute(Route)
                };
                await _db_flights.Add(flight);
                return await UpdateScheduleViewFlightsAsync(flight.TimeOfFlight, Route, pilotEmail);
            }
            catch (DbUpdateException) {  
                return Content("debug 1");
            }
            catch (InvalidOperationException) {
                return await UpdateScheduleViewFlightsAsync(currentDate, Route, pilotEmail, true);
            }
        }

        /* Action for clicking the 'Update' button on the schedule view. This updates the schedule view, showing the flights pertaining only 
         * to the date selected. */
        public async System.Threading.Tasks.Task<IActionResult> UpdateScheduleViewFlightsAsync(DateTime currentDate, string Route, string email, bool scheduleFailed = false)
        {
            try
            {
                if (email == null) 
                    email = HttpContext.Session.GetString("username");

                var allFlights = _db_flights.GetAll(currentDate, Route, email);
                ViewBag.email = email;
                ViewBag.dateSelected = currentDate;
                if (scheduleFailed)
                {
                    ViewBag.scheduleStatus = "fail";
                }
                else
                {
                    ViewBag.scheduleStatus = "success";
                }
                ViewBag.Flights = await allFlights;
                ViewBag.PilotList = await PilotListGenerator.GetPilotEmailsAsync(_db_pilots);
                ViewBag.RouteList = await RouteListGenerator.GenerateRouteListAsync(Route, _db_route);

                ViewBag.RouteListRaw = await _db_route.GetAll();
                ViewBag.RouteSelected = Route;

                return View("../Schedule/Schedule");
            }
            catch (ArgumentNullException) { /*Luke & Dillon Do something*/}
            return Content("Todo 3");
        }
    }
}

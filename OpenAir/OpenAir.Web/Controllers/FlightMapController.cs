using Microsoft.AspNetCore.Mvc;

namespace OpenAir.Web.Controllers
{
    public class FlightMapController : Controller
    {
        public FlightMapController() { }
        public IActionResult FlightMap()
        {
            return View();
        }
    }
}

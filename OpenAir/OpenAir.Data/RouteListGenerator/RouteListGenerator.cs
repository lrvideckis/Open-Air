using OpenAir.Data.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OpenAir.Data
{
    public static class RouteListGenerator
    {
        public static async System.Threading.Tasks.Task<SelectList> GenerateRouteListAsync(string routeSelected, RouteRepo _db_route)
        {
            var RouteListTask = _db_route.GetAll();
            List<SelectListItem> list = new List<SelectListItem>();
            //var RouteList = RouteListTask.Result;
            foreach (IRoute route in await RouteListTask)
            {
                list.Add(new SelectListItem { Text = route.RouteId, Value = route.RouteId });
            }
            return new SelectList(list, "Value", "Text", routeSelected);
        }
    }
}

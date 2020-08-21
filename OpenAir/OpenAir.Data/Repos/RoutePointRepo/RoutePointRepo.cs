using OpenAir.Data.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAir.Data
{
    public class RoutePointRepo : IRoutePointRepo
    {
        private readonly OpenAirDbContext db;

        public RoutePointRepo(OpenAirDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Point>> GetPoints(Route route)
        {
            //TODO: point list should be ordered the same as how which points are reached first
            // test this as well, clean this up as well

            // get all the RoutePoints and go through routpoints to find RoutePoints with the route id
            var routePoints = await db.RoutePoints.ToListAsync();
            var routePointQuery = routePoints.Where(rp => rp.RouteId == route.RouteId);

            // get all the Points and put into a list
            var pointList = await db.Points.ToListAsync();
            var pointQuery = new List<Point>();

            // get the points that match and return
            foreach(Point p in pointList)
            {
                foreach(RoutePoint rp in routePointQuery)
                {
                    if(rp.PointId == p.PointId)
                    {
                        pointQuery.Add(p);
                    }
                }
            }
            return pointQuery.AsEnumerable();
        }

        public async Task<IEnumerable<Route>> GetRoutes(Point point)
        {
            // get all the RoutePoints and go through routpoints to find RoutePoints with the point id
            var routePoints = await db.RoutePoints.ToListAsync();
            var routePointQuery = routePoints.Where(rp => rp.PointId == point.PointId);

            // get all the Routes and put into a list
            var routeList = await db.Routes.ToListAsync();
            var routeQuery = new List<Route>();

            // get the points that match and return
            foreach (Route r in routeList)
            {
                foreach (RoutePoint rp in routePointQuery)
                {
                    if (rp.RouteId == r.RouteId)
                    {
                        routeQuery.Add(r);
                    }
                }
            }
            return routeQuery.AsEnumerable();
        }
    }
}

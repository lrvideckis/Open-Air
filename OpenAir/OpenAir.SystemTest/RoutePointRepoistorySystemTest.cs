using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;

namespace OpenAir.SystemTest
{
    public class RoutePointRepoistorySystemTest
    {
        private OpenAirDbContext _context;
        private RoutePointRepo repo;
        private Route route1;
        private Route route2;
        private Point point1;
        private Point point2;

        [SetUp]
        public async Task Setup()
        {
            /// ARANGE
            /// 
            // ask system test setup class to get the context instance
            _context = SingletonTestSetup.Instance().Get();

            // make the test routes and points
            route1 = new Route()
            {
                RouteId = "R-1"
            };
            route2 = new Route()
            { 
                RouteId = "R-2"
            };
            point1 = new Point()
            {
                Latitude = 1.0f,
                Longitude = 1.0f
            };
            point2 = new Point()
            {
                Latitude = 2.0f,
                Longitude = 2.0f
            };

            // insert the data so we can make RoutePoints
            await AddRoute(route1);
            await AddRoute(route2);
            await AddPoint(point1);
            await AddPoint(point2);

            // make the rp's with different points and same route
            var queryPoints = await _context.Points.ToListAsync();
            foreach(Point p in queryPoints)
            {
                var rp = new RoutePoint { Point = p, Route = route1,
                                          PointId = p.PointId, RouteId = route1.RouteId };
                await AddRoutePoint(rp);
            }

            // make rp's with same different routes and same point
            var queryRoutes = await _context.Routes.ToListAsync();
            foreach (Route r in queryRoutes)
            {
                try // avoids trying to insert duplicates
                {
                    var rp = new RoutePoint
                    {
                        Point = queryPoints[0],
                        Route = r,
                        PointId = queryPoints[0].PointId,
                        RouteId = r.RouteId
                    };
                    await AddRoutePoint(rp);
                }
                catch (DbUpdateException ex) { Console.WriteLine(ex.ToString()); }
                catch(InvalidOperationException ex) { Console.WriteLine(ex.ToString()); }
            }

            repo = new RoutePointRepo(_context);
        }

        [Test]
        public async Task TestGetPoints()
        {
            /// ACT
            /// 
            var result = await repo.GetPoints(route1);
           
            /// ASSERT
            /// 
            Assert.IsNotNull(result);

            foreach(Point pt in result)
            {
                Assert.IsNotNull(result.Where(p => p.PointId == pt.PointId));
            }
        }

        [Test]
        public async Task TestGetRoutes()
        {
            var points = await _context.Points.ToListAsync();        
            var result = await repo.GetRoutes(points[0]);
        }

        public async Task AddRoute(Route r)
        {
            await _context.Routes.AddAsync(r);
            await _context.SaveChangesAsync();
        }

        public async Task AddPoint(Point p)
        {
            await _context.Points.AddAsync(p);
            await _context.SaveChangesAsync();
        }

        public async Task AddRoutePoint(RoutePoint rp)
        {
            await _context.RoutePoints.AddAsync(rp);
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public async Task CleanUp()
        {
            // get all routes in the db
            List<RoutePoint> routePoints = await _context.RoutePoints.ToListAsync();

            _context.Remove(route1);
            _context.Remove(route2);

            // walk through and remove them
            for (int i = 0; i < routePoints.Count; i++)
            {
                RoutePoint rp = routePoints[i];
                _ = _context.Remove(rp);
                _context.Entry(rp).State = EntityState.Deleted;
            }
            var queryPoints = await _context.Points.ToListAsync();
            for (int i = 0; i < queryPoints.Count; i++)
            {           
                Point p = queryPoints[i];
                _ = _context.Remove(p);
                _context.Entry(p).State = EntityState.Deleted;                
            }
            // tell the db all the routes were removed
            await _context.SaveChangesAsync();
        }

    }
}

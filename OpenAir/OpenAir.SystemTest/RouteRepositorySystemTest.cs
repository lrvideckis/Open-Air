using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace OpenAir.SystemTest
{
    public class RouteRepositorySystemTest
    {
        private OpenAirDbContext _context;
        private RouteRepo repo;
        private Route route;
        /*
         * Author: Adrian Azan
         * Creates context of database for testing.
         * Connects to testing server
         */
        [SetUp]
        public async Task Setup()
        {
            /// ARRANGE

            // ask system test setup class to get the context instance
            _context = SingletonTestSetup.Instance().Get();
            //Wait
            route = new Route()
            {
                RouteId = "IR-140"
            };
            await AddRoute(route);
            repo = new RouteRepo(_context);
        }

        [Test]
        public async Task TestGetRoute()
        {
            /// ACT

            // VR-140 should exist in the database because of seeding done in onModelCreate in the context
            var resultRoute = await repo.GetRoute("VR-140");

            /// ASSERT

            Assert.IsNotNull(resultRoute);         
        }

        [Test]
        public async Task TestGetAll()
        {
            /// ACT
            
            var result = await repo.GetAll();

            /// ASSERT

            Assert.IsTrue(result.ToList().Count == 2);    
        }

        public async Task AddRoute(Route r)
        {
            await _context.Routes.AddAsync(r);
            await _context.SaveChangesAsync();
        }


        [TearDown]
        public async Task CleanUp()
        {
            // get all routes in the db
            List<Route> routes = await _context.Routes.ToListAsync();
            // walk through and remove them
            for (int i = 0; i < routes.Count; i++)
            {
                Route route = routes[i];
                if (route.RouteId == "VR-140") // delete all routes except vr-140
                {
                    continue;
                }
                _ = _context.Remove(route);
                _context.Entry(route).State = EntityState.Deleted;
            }
            // tell the db all the routes were removed
            await _context.SaveChangesAsync();
        }

    }


}

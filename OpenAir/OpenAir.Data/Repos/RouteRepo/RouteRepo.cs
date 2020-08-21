using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAir.Data.Models;

namespace OpenAir.Data
{
    public class RouteRepo
    {
        private readonly OpenAirDbContext db;

        public RouteRepo(OpenAirDbContext db)
        {
            this.db = db;
        }

        /*
         * GetAll method. Returns an IEnumerable of all the routes in the Routes table in the db.
         * The IEnumerable can be cast into the container of your choice later (ex. List). 
         */
        public async Task<IEnumerable<IRoute>> GetAll()
        {
            return await db.Routes.ToListAsync();
        }

        /*
         *  GetRoute Method. Uses the route id to lookup their entry in the db. 
         *  Returns an interface containing all the column data as attributes.
         */
        public async Task<IRoute> GetRoute(string id)
        {
            return await db.Routes.Where(r => r.RouteId == id).FirstOrDefaultAsync();
        }
    }
}

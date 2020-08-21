using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenAir.Data
{
    public interface IRoutePointRepo
    {
        public Task<IEnumerable<Point>> GetPoints(Route route);

        public Task<IEnumerable<Route>> GetRoutes(Point point);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data.Models
{
    public interface IRoutePoint
    {
        public Point Point { get; set; }
        public int PointId { get; set; }

        public Route Route { get; set; }
        public string RouteId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data.Models
{
    public class RoutePoint : IRoutePoint
    {
        // class attributes - foreign key references
        public Point Point { get; set; }
        public int PointId { get; set; } // PK
        public Route Route { get; set; }
        public string RouteId { get; set; } // PK
    }
}

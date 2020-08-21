using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data.Models
{
    public class Point : IPoint
    {
        // class attributes
        public int PointId { get; set; } // PK
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        // Point has zero to many RoutePoints
        public IEnumerable<RoutePoint> RoutePoints { get; set; }
    }
}

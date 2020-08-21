using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data.Models
{
    public interface IPoint
    {
        public int PointId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public IEnumerable<RoutePoint> RoutePoints { get; set; }
    }
}

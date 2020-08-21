using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenAir.Data
{
    public static class FlightUtility
    {
        private static readonly int R = 6371; // Radius of the earth in km
        private static readonly float PI = 3.1415926535897931f;
        private static readonly float KM = 1.85152f;
        public static double DistanceFromPoints(Point p1, Point p2)
        {
            var lat1 = p1.Latitude;
            var lat2 = p2.Latitude;
            var lon1 = p1.Longitude;
            var lon2 = p2.Longitude;

            // 1 knot = 1.852 km/h 
            var dLat = DegreeToRadian(lat2 - lat1);  
            var dLon = DegreeToRadian(lon2 - lon1);
            var a =
              Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
              Math.Cos(DegreeToRadian(lat1)) * Math.Cos(DegreeToRadian(lat2)) *
              Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (R * angle); // Distance in km
        }
      

        private static float DegreeToRadian(float deg)
        {
            return deg * (PI / 180);
        }

        public static float FlightDurationInHours(float flightSpeedKnots, float distanceKm)
        {
            var flightSpeedKm = KnotsToKm(flightSpeedKnots);
            return flightSpeedKm / distanceKm;
        }

        private static float KnotsToKm(float knots)
        {
            return knots * KM;
        }

    }
}

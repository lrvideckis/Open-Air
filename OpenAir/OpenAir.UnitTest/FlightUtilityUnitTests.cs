using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenAir.Data;
using OpenAir.Data.Models;
namespace OpenAir.UnitTest
{
    public class FlightUtilityUnitTests
    {

        [Test]
        public void TestDistanceBetweenPoints()
        {
            var point1 = new Point { Latitude = 0.0f, Longitude = 0.0f };
            var point2 = new Point { Latitude = 45.0f, Longitude = 45.0f };
            var expectedDistance = 6672f;

            var distance = FlightUtility.DistanceFromPoints(point1, point2);

            Assert.AreEqual(expectedDistance, distance, 1.0);
            // distance from a point to itself
            Assert.Zero((int)Math.Round(FlightUtility.DistanceFromPoints(point1, point1)));        
        }

        [Test]
        public void TestCalculateFlightDurationInHours()
        {
            var flight = new Flight { FlightSpeed = 1 };
            var distanceKm = 1f;
            var expectedTime = 1.852;

            var time = FlightUtility.FlightDurationInHours(flight.FlightSpeed, distanceKm);

            Assert.AreEqual(expectedTime, time, 0.5);
        }

    
    }
}

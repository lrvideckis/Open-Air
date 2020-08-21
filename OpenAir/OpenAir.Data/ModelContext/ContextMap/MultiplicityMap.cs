using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAir.Data.Models;
using System;
using System.Diagnostics.Contracts;

namespace OpenAir.Data
{
    /*
     * Class: MultiplicityMap
     * Description: This class gives the context instructions on how to setup the multiplicity of entities in the db
     */
    public class MultiplicityMap
    {
        /*
         * Constructor
         * Description: 
         */
        public MultiplicityMap(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoutePoint>().HasKey(rp => new { rp.PointId, rp.RouteId });
            modelBuilder.Entity<Pilot>().HasMany(p => p.Flights).WithOne(f => f.Pilot);
            modelBuilder.Entity<Route>().HasMany(r => r.RoutePoints).WithOne(rp => rp.Route);
            modelBuilder.Entity<Point>().HasMany(p => p.RoutePoints).WithOne(rp => rp.Point);
        }
    }
}

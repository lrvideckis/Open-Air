using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAir.Data.Models;
using System;
using System.Diagnostics.Contracts;

namespace OpenAir.Data
{
    /*
     * Class: FlightMap
     * Description: This class gives the context instructions on how to setup the Flight entity in the database
     */
    public class FlightMap
    {
        private Func<EntityTypeBuilder<Flight>> entity;

        /*
         * Constructor
         * Description: tells the entity builder which attribute is the key and that time of flight != null. The contract is to make VS happy.
         */
        public FlightMap(EntityTypeBuilder<Flight> entityBuilder)
        {
            Contract.Requires(entityBuilder != null);
            entityBuilder.HasKey(f => f.FlightId);
            entityBuilder.Property(f => f.TimeOfFlight).IsRequired();
            entityBuilder.Property(f => f.FlightSpeed).IsRequired();
            entityBuilder.Property(f => f.PilotEmailId).IsRequired();
            entityBuilder.Property(f => f.RouteId).IsRequired();
        }

        public FlightMap(Func<EntityTypeBuilder<Flight>> entity)
        {
            this.entity = entity;
        }

    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAir.Data.Models;
using System;
using System.Diagnostics.Contracts;

namespace OpenAir.Data
{
    public class RouteMap
    {
        private Func<EntityTypeBuilder<Route>> entity;

        public RouteMap(EntityTypeBuilder<Route> entityBuilder)
        {
            Contract.Requires(entityBuilder != null);
            entityBuilder.HasKey(p => p.RouteId);
        }

        public RouteMap(Func<EntityTypeBuilder<Route>> entity)
        {
            this.entity = entity;
        }

    }
}

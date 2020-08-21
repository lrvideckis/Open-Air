using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAir.Data.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace OpenAir.Data
{
    public class PointMap
    {
        private Func<EntityTypeBuilder<Point>> entity;

        public PointMap(EntityTypeBuilder<Point> entityBuilder)
        {
            Contract.Requires(entityBuilder != null);
            entityBuilder.HasKey(p => p.PointId);
            entityBuilder.Property(p => p.Latitude).IsRequired();
            entityBuilder.Property(p => p.Longitude).IsRequired();

        }

        public PointMap(Func<EntityTypeBuilder<Point>> entity)
        {
            this.entity = entity;
        }
    }
}

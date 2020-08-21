using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OpenAir.Data.Models;
using System;
using System.Diagnostics.Contracts;

namespace OpenAir.Data
{
    public class PilotMap
    {
        private Func<EntityTypeBuilder<Pilot>> entity;

        public PilotMap(EntityTypeBuilder<Pilot> entityBuilder)
        {
            Contract.Requires(entityBuilder != null);
            entityBuilder.HasKey(p => p.EmailId);
            entityBuilder.Property(p => p.Name).IsRequired();
            entityBuilder.Property(p => p.Pwd).IsRequired();

            entityBuilder.Property(p => p.IsAdmin).IsRequired();

        }

        public PilotMap(Func<EntityTypeBuilder<Pilot>> entity)
        {
            this.entity = entity;
        }

    }
}

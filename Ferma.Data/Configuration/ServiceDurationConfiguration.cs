using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{
      public class ServiceDurationConfiguration : IEntityTypeConfiguration<ServiceDuration>
    {
        public void Configure(EntityTypeBuilder<ServiceDuration> builder)
        {
            builder.Property(x => x.Amount).HasColumnType("decimal(18,2)");

        }
    }
}

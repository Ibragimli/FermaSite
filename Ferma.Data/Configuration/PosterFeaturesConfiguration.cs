using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{

    public class PosterFeaturesConfiguration : IEntityTypeConfiguration<PosterFeatures>
    {
        public void Configure(EntityTypeBuilder<PosterFeatures> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Describe).HasMaxLength(3000).IsRequired(true);
            builder.Property(x => x.Email).HasMaxLength(70).IsRequired(true);
            builder.Property(x => x.PhoneNumber).HasMaxLength(15).IsRequired(true);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");

        }
    }
}

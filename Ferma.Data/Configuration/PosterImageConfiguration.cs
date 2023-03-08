using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{
    public class PosterImageConfiguration : IEntityTypeConfiguration<PosterImage>
    {
        public void Configure(EntityTypeBuilder<PosterImage> builder)
        {
            builder.Property(x => x.Image).HasMaxLength(120).IsRequired(true);
        }
    }
}

using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{
    public class ImageSettingConfiguration : IEntityTypeConfiguration<ImageSetting>
    {
        public void Configure(EntityTypeBuilder<ImageSetting> builder)
        {
            builder.Property(x => x.Key).HasMaxLength(100).IsRequired(true);
            builder.Property(x => x.Value).HasMaxLength(500).IsRequired(true);
        }
    }
}

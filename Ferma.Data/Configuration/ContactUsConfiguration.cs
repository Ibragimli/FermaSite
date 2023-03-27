﻿using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{
    
    public class ContactUsConfiguration : IEntityTypeConfiguration<ContactUs>
    {
        public void Configure(EntityTypeBuilder<ContactUs> builder)
        {
            builder.Property(x => x.FullName).HasMaxLength(30).IsRequired(true);
            builder.Property(x => x.Email).HasMaxLength(70).IsRequired(false);
            builder.Property(x => x.Subject).HasMaxLength(70).IsRequired(true);
            builder.Property(x => x.Message).HasMaxLength(2500).IsRequired(true);
            builder.Property(x => x.Phone).HasMaxLength(20).IsRequired(false);
        }
    }
}

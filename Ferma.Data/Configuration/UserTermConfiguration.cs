using Ferma.Core.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ferma.Data.Configuration
{
    public class UserTermConfiguration : IEntityTypeConfiguration<UserTerm>
    {
        public void Configure(EntityTypeBuilder<UserTerm> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(80).IsRequired(true);
            builder.Property(x => x.Text).HasMaxLength(15000).IsRequired(true);
        }
    }
}

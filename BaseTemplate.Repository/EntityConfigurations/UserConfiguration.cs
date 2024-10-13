using BaseTemplate.Repository.EntityConfigurations.Common;
using BaseTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.EntityConfigurations
{
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Surname).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Username).HasMaxLength(256);
            builder.Property(x => x.PasswordHash).HasMaxLength(1024);
            builder.Property(x => x.RefreshToken).HasMaxLength(1024);
        }
    }
}

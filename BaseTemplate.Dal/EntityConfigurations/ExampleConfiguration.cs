using BaseTemplate.Dal.EntityConfigurations.Common;
using BaseTemplate.Entity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Dal.EntityConfigurations
{
    public class ExampleConfiguration : BaseEntityConfiguration<Example>
    {
        public override void Configure(EntityTypeBuilder<Example> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
        }
    }
}

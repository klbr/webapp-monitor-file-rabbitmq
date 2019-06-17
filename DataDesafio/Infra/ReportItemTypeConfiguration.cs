using DataDesafio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDesafio.Infra
{
    public class ReportItemTypeConfiguration : IEntityTypeConfiguration<ReportItem>
    {
        public void Configure(EntityTypeBuilder<ReportItem> builder)
        {
            builder.ToTable(nameof(ReportItem));
            builder.HasKey(x => x.Id);

            builder.Property(p => p.ToolName).IsRequired();
            builder.Property(p => p.Version).IsRequired();
            builder.Property(p => p.Update).IsRequired();
        }
    }
}

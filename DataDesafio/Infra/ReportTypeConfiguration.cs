using DataDesafio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDesafio.Infra
{
    public class ReportTypeConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable(nameof(Report));
            builder.HasKey(x => x.Id);

            builder.Property(p => p.JsonMd5).IsRequired();
            builder.Property(p => p.VerboseMsg).IsRequired();
            builder.Property(p => p.Filename).IsRequired();
            builder.Property(p => p.Resource).IsRequired();

            builder.HasMany(p => p.Items)
                .WithOne(p => p.Report)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

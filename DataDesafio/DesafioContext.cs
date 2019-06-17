using DataDesafio.Infra;
using DataDesafio.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataDesafio
{
    public class DesafioContext : DbContext
    {
        public DesafioContext(DbContextOptions<DesafioContext> options) : base(options)
        {
        }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReportTypeConfiguration()).
                ApplyConfiguration(new ReportItemTypeConfiguration());
        }
    }
}

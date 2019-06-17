﻿// <auto-generated />
using System;
using DataDesafio;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Desafio.Data.Migrations
{
    [DbContext(typeof(DesafioContext))]
    [Migration("20190617004948_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataDesafio.Models.Report", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedAt");

                    b.Property<string>("Filename")
                        .IsRequired();

                    b.Property<string>("JsonMd5")
                        .IsRequired();

                    b.Property<DateTimeOffset>("LastUpdatedAt");

                    b.Property<string>("Permalink");

                    b.Property<int>("Positives");

                    b.Property<string>("Resouce")
                        .IsRequired();

                    b.Property<string>("ScanId");

                    b.Property<int>("Total");

                    b.Property<string>("VerboseMsg")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Report");
                });

            modelBuilder.Entity("DataDesafio.Models.ReportItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Detected");

                    b.Property<Guid?>("ReportId");

                    b.Property<string>("Result");

                    b.Property<string>("ToolName")
                        .IsRequired();

                    b.Property<string>("Update")
                        .IsRequired();

                    b.Property<string>("Version")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ReportId");

                    b.ToTable("ReportItem");
                });

            modelBuilder.Entity("DataDesafio.Models.ReportItem", b =>
                {
                    b.HasOne("DataDesafio.Models.Report", "Report")
                        .WithMany("Items")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
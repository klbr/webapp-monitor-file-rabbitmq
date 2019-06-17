using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Desafio.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    JsonMd5 = table.Column<string>(nullable: false),
                    VerboseMsg = table.Column<string>(nullable: false),
                    Total = table.Column<int>(nullable: false),
                    Positives = table.Column<int>(nullable: false),
                    Resouce = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    LastUpdatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Filename = table.Column<string>(nullable: false),
                    ScanId = table.Column<string>(nullable: true),
                    Permalink = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReportId = table.Column<Guid>(nullable: true),
                    ToolName = table.Column<string>(nullable: false),
                    Detected = table.Column<bool>(nullable: false),
                    Version = table.Column<string>(nullable: false),
                    Result = table.Column<string>(nullable: true),
                    Update = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportItem_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportItem_ReportId",
                table: "ReportItem",
                column: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportItem");

            migrationBuilder.DropTable(
                name: "Report");
        }
    }
}

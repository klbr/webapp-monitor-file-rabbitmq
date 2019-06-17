using Microsoft.EntityFrameworkCore.Migrations;

namespace Desafio.Data.Migrations
{
    public partial class FixResourceName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resouce",
                table: "Report",
                newName: "Resource");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resource",
                table: "Report",
                newName: "Resouce");
        }
    }
}

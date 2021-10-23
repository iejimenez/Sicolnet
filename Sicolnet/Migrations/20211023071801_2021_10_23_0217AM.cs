using Microsoft.EntityFrameworkCore.Migrations;

namespace Sicolnet.Migrations
{
    public partial class _2021_10_23_0217AM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShortUrl",
                table: "Personas",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortUrl",
                table: "Personas");
        }
    }
}

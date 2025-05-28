using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    public partial class RemoveDayNumberFromPlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayNumber",
                table: "Places");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayNumber",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDayNumberToPlace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DayNumber",
                table: "Places",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayNumber",
                table: "Places");
        }
    }
}

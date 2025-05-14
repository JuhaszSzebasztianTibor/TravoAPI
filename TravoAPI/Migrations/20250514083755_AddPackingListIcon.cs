using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPackingListIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PackingListIcon",
                table: "PackingLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "PackingListIcon",
                value: "fas fa-utensils");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "PackingListIcon",
                value: "fas fa-umbrella-beach");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "PackingListIcon",
                value: "fas fa-briefcase");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "PackingListIcon",
                value: "fas fa-baby");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "PackingListIcon",
                value: "fas fa-exclamation-circle");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "PackingListIcon",
                value: "fas fa-hamburger");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackingListIcon",
                table: "PackingLists");
        }
    }
}

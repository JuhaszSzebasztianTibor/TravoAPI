using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPackingListTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "PackingLists");

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
                columns: new[] { "Name", "PackingListIcon" },
                values: new object[] { "Essentials", "fas fa-exclamation-circle" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "PackingLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Category", "PackingListIcon" },
                values: new object[] { "FancyDinner", "fas fa-clipboard-list" });

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "PackingListIcon" },
                values: new object[] { "Beach", "fas fa-clipboard-list" });

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "PackingListIcon" },
                values: new object[] { "Business", "fas fa-clipboard-list" });

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Name", "PackingListIcon" },
                values: new object[] { "Baby", "Baby", "fas fa-clipboard-list" });

            migrationBuilder.InsertData(
                table: "PackingLists",
                columns: new[] { "Id", "Category", "Name", "PackingListIcon", "TripId", "UserId" },
                values: new object[,]
                {
                    { 5, "Essentials", "Essentials", "fas fa-clipboard-list", 0, "SYSTEM" },
                    { 6, "Food", "Food", "fas fa-clipboard-list", 0, "SYSTEM" }
                });
        }
    }
}

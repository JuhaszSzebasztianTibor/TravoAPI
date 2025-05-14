using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddTripIdToPackingList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "PackingLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "TripId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "TripId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "TripId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "TripId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "TripId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "TripId",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripId",
                table: "PackingLists");
        }
    }
}

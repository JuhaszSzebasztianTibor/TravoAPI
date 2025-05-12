using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNamesToPackingSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackingLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SYSTEM",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "d11807c2-e1f8-45a3-a9b7-69ca3244bb57", "e3895b49-fc23-4461-a42b-643fc50fadaf" });

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Fancy Dinner");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Beach Trip");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Business");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Baby");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Essentials");

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Food");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "PackingLists",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SYSTEM",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "731c6788-a8d4-4b5e-aae1-dbfd641a83fc", "0be66c77-478c-4436-8320-84ad339a7c74" });

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: null);

            migrationBuilder.UpdateData(
                table: "PackingLists",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: null);
        }
    }
}

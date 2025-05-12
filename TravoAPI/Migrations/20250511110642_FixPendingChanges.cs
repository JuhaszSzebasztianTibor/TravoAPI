using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SYSTEM",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "11111111-1111-1111-1111-111111111111", "AQAAAAEAACcQAAAAEKX8d2J2lULBw4mYx4Zx05wZIjgj6UeQ7GFXSJiJTh+ZJ6Rqiw1j4fYSUQ2mLzdCjg==", "00000000-0000-0000-0000-000000000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "SYSTEM",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "237df991-c752-4769-ac2b-4f0e0cce46f4", null, "de46318c-8312-4a4a-bb50-1690690ff1e7" });
        }
    }
}

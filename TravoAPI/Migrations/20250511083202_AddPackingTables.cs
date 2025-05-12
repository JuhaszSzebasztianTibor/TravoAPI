using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    public partial class AddPackingTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Insert SYSTEM user FIRST with all required fields (if not exists)
            migrationBuilder.Sql(@"
        IF NOT EXISTS (SELECT * FROM AspNetUsers WHERE Id = 'SYSTEM')
        BEGIN
            INSERT INTO AspNetUsers (
                Id, 
                UserName, 
                NormalizedUserName, 
                Email, 
                NormalizedEmail, 
                EmailConfirmed, 
                PasswordHash, 
                SecurityStamp, 
                ConcurrencyStamp, 
                FirstName, 
                LastName, 
                AccessFailedCount, 
                LockoutEnabled, 
                PhoneNumberConfirmed, 
                TwoFactorEnabled
            ) VALUES (
                'SYSTEM', 
                'system@localhost', 
                'SYSTEM@LOCALHOST', 
                'system@localhost', 
                'SYSTEM@LOCALHOST', 
                1, 
                'AQAAAAEAACcQAAAAEKX8d2J2lULBw4mYx4Zx05wZIjgj6UeQ7GFXSJiJTh+ZJ6Rqiw1j4fYSUQ2mLzdCjg==', 
                '00000000-0000-0000-0000-000000000000', 
                '11111111-1111-1111-1111-111111111111', 
                'System', 
                'Account', 
                0, 
                0, 
                0, 
                0
            );
        END
    ");

            // 2. Create PackingLists and PackingItems tables
            migrationBuilder.CreateTable(
                name: "PackingLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackingLists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackingItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    PackingListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackingItems_PackingLists_PackingListId",
                        column: x => x.PackingListId,
                        principalTable: "PackingLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // 3. Seed PackingLists and PackingItems
            migrationBuilder.InsertData(
                table: "PackingLists",
                columns: new[] { "Id", "Category", "Name", "UserId" },
                values: new object[,]
                {
            { 1, 0, "Fancy Dinner", "SYSTEM" },
            { 2, 1, "Beach", "SYSTEM" },
            { 3, 2, "Business", "SYSTEM" },
            { 4, 3, "Baby", "SYSTEM" },
            { 5, 4, "Essentials", "SYSTEM" },
            { 6, 5, "Food", "SYSTEM" }
                });

            migrationBuilder.InsertData(
                table: "PackingItems",
                columns: new[] { "Id", "IsChecked", "Name", "PackingListId", "Quantity" },
                values: new object[,]
                {
            { 1, false, "Dress Shoes", 1, 1 },
            { 2, false, "Hair Dryer", 1, 1 },
            { 3, false, "Hair Products", 1, 1 },
            { 4, false, "Jacket", 1, 1 },
            { 5, false, "Jewelry", 1, 1 },
            { 6, false, "Pants", 1, 1 },
            { 7, false, "Pantyhose", 1, 1 },
            { 8, false, "Skirt", 1, 1 },
            { 50, false, "Sunscreen", 2, 1 },
            { 51, false, "Swimsuit", 2, 1 },
            { 52, false, "Towel", 2, 1 },
            { 53, false, "Flip Flops", 2, 1 },
            { 54, false, "Sunglasses", 2, 1 },
            { 55, false, "Beach Hat", 2, 1 },
            { 56, false, "Beach Ball", 2, 1 },
            { 57, false, "Cooler", 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackingItems_PackingListId",
                table: "PackingItems",
                column: "PackingListId");

            migrationBuilder.CreateIndex(
                name: "IX_PackingLists_UserId",
                table: "PackingLists",
                column: "UserId");
        }
    }
}

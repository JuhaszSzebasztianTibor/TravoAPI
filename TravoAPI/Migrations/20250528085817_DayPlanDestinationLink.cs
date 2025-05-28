using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class DayPlanDestinationLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                table: "DayPlans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DayPlans_DestinationId",
                table: "DayPlans",
                column: "DestinationId");

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlans_Destinations_DestinationId",
                table: "DayPlans",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlans_Destinations_DestinationId",
                table: "DayPlans");

            migrationBuilder.DropIndex(
                name: "IX_DayPlans_DestinationId",
                table: "DayPlans");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "DayPlans");
        }
    }
}

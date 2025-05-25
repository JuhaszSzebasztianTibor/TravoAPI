using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDayPlanEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlan_Trips_TripId",
                table: "DayPlan");

            migrationBuilder.DropForeignKey(
                name: "FK_Note_Place_PlaceId",
                table: "Note");

            migrationBuilder.DropForeignKey(
                name: "FK_Place_DayPlan_DayPlanId",
                table: "Place");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Place",
                table: "Place");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Note",
                table: "Note");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayPlan",
                table: "DayPlan");

            migrationBuilder.RenameTable(
                name: "Place",
                newName: "Places");

            migrationBuilder.RenameTable(
                name: "Note",
                newName: "Notes");

            migrationBuilder.RenameTable(
                name: "DayPlan",
                newName: "DayPlans");

            migrationBuilder.RenameIndex(
                name: "IX_Place_DayPlanId",
                table: "Places",
                newName: "IX_Places_DayPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Note_PlaceId",
                table: "Notes",
                newName: "IX_Notes_PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_DayPlan_TripId",
                table: "DayPlans",
                newName: "IX_DayPlans_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Places",
                table: "Places",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Notes",
                table: "Notes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayPlans",
                table: "DayPlans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlans_Trips_TripId",
                table: "DayPlans",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Places_PlaceId",
                table: "Notes",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Places_DayPlans_DayPlanId",
                table: "Places",
                column: "DayPlanId",
                principalTable: "DayPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DayPlans_Trips_TripId",
                table: "DayPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Places_PlaceId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_DayPlans_DayPlanId",
                table: "Places");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Places",
                table: "Places");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Notes",
                table: "Notes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DayPlans",
                table: "DayPlans");

            migrationBuilder.RenameTable(
                name: "Places",
                newName: "Place");

            migrationBuilder.RenameTable(
                name: "Notes",
                newName: "Note");

            migrationBuilder.RenameTable(
                name: "DayPlans",
                newName: "DayPlan");

            migrationBuilder.RenameIndex(
                name: "IX_Places_DayPlanId",
                table: "Place",
                newName: "IX_Place_DayPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Notes_PlaceId",
                table: "Note",
                newName: "IX_Note_PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_DayPlans_TripId",
                table: "DayPlan",
                newName: "IX_DayPlan_TripId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Place",
                table: "Place",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Note",
                table: "Note",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DayPlan",
                table: "DayPlan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DayPlan_Trips_TripId",
                table: "DayPlan",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Place_PlaceId",
                table: "Note",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Place_DayPlan_DayPlanId",
                table: "Place",
                column: "DayPlanId",
                principalTable: "DayPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

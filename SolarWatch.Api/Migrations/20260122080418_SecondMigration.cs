using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Api.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solar_Cities_CityId",
                table: "Solar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solar",
                table: "Solar");

            migrationBuilder.RenameTable(
                name: "Solar",
                newName: "SolarData");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Cities",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "longitude",
                table: "Cities",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "latitude",
                table: "Cities",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "SolarData",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_Solar_CityId",
                table: "SolarData",
                newName: "IX_SolarData_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolarData",
                table: "SolarData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SolarData_Cities_CityId",
                table: "SolarData",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolarData_Cities_CityId",
                table: "SolarData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolarData",
                table: "SolarData");

            migrationBuilder.RenameTable(
                name: "SolarData",
                newName: "Solar");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cities",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Cities",
                newName: "longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Cities",
                newName: "latitude");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Solar",
                newName: "date");

            migrationBuilder.RenameIndex(
                name: "IX_SolarData_CityId",
                table: "Solar",
                newName: "IX_Solar_CityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solar",
                table: "Solar",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Solar_Cities_CityId",
                table: "Solar",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

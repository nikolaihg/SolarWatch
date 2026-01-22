using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolarWatch.Api.Migrations
{
    /// <inheritdoc />
    public partial class ThirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SolarData_Cities_CityId",
                table: "SolarData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SolarData",
                table: "SolarData");

            migrationBuilder.DropIndex(
                name: "IX_SolarData_CityId",
                table: "SolarData");

            migrationBuilder.RenameTable(
                name: "SolarData",
                newName: "Solars");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Solars",
                table: "Solars",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                table: "Cities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solars_CityId_Date",
                table: "Solars",
                columns: new[] { "CityId", "Date" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Solars_Cities_CityId",
                table: "Solars",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solars_Cities_CityId",
                table: "Solars");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Name",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Solars",
                table: "Solars");

            migrationBuilder.DropIndex(
                name: "IX_Solars_CityId_Date",
                table: "Solars");

            migrationBuilder.RenameTable(
                name: "Solars",
                newName: "SolarData");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Cities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolarData",
                table: "SolarData",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SolarData_CityId",
                table: "SolarData",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SolarData_Cities_CityId",
                table: "SolarData",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

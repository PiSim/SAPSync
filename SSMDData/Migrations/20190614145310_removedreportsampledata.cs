using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class removedreportsampledata : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasRollArrived",
                table: "TestReports",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RollArrivalDate",
                table: "TestReports",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RollStatus",
                table: "TestReports",
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasRollArrived",
                table: "TestReports");

            migrationBuilder.DropColumn(
                name: "RollArrivalDate",
                table: "TestReports");

            migrationBuilder.DropColumn(
                name: "RollStatus",
                table: "TestReports");
        }

        #endregion Methods
    }
}
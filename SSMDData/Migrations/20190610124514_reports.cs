using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class reports : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestReports_Orders_OrderNumber",
                table: "TestReports");

            migrationBuilder.DropIndex(
                name: "IX_TestReports_OrderNumber",
                table: "TestReports");

            migrationBuilder.DropColumn(
                name: "RollArrivalDate",
                table: "TestReports");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RollArrivalDate",
                table: "TestReports",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_TestReports_OrderNumber",
                table: "TestReports",
                column: "OrderNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_TestReports_Orders_OrderNumber",
                table: "TestReports",
                column: "OrderNumber",
                principalTable: "Orders",
                principalColumn: "Number",
                onDelete: ReferentialAction.Restrict);
        }

        #endregion Methods
    }
}
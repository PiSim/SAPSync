using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class orderdataextfinal : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSampleArrived",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleArrivalDate",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SampleRollStatus",
                table: "Orders",
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSampleArrived",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SampleArrivalDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SampleRollStatus",
                table: "Orders");
        }

        #endregion Methods
    }
}
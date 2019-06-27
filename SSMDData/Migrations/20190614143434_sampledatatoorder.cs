using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class sampledatatoorder : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
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

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasSampleArrived",
                table: "Orders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleArrivalDate",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SampleRollStatus",
                table: "Orders",
                nullable: true);
        }

        #endregion Methods
    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class orderdataext : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasSampleArrived",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "SampleArrivalDate",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "SampleRollStatus",
                table: "OrderData");

            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "TestReports",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "TestReports",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<bool>(
                name: "HasSampleArrived",
                table: "OrderData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "SampleArrivalDate",
                table: "OrderData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SampleRollStatus",
                table: "OrderData",
                nullable: true);
        }

        #endregion Methods
    }
}
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class orderdataextfix : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SampleArrivalDate",
                table: "OrderData",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SampleArrivalDate",
                table: "OrderData",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        #endregion Methods
    }
}
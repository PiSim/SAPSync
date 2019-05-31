using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class rework : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRework",
                table: "OrderConfirmations");

            migrationBuilder.DropColumn(
                name: "ReworkAmount",
                table: "OrderConfirmations");

            migrationBuilder.DropColumn(
                name: "ReworkScrap",
                table: "OrderConfirmations");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRework",
                table: "OrderConfirmations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "ReworkAmount",
                table: "OrderConfirmations",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReworkScrap",
                table: "OrderConfirmations",
                nullable: false,
                defaultValue: 0.0);
        }

        #endregion Methods
    }
}
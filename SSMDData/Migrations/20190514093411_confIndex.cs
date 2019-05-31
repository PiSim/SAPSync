using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class confIndex : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderConfirmations_DeletionFlag",
                table: "OrderConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_OrderConfirmations_StartTime",
                table: "OrderConfirmations");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_DeletionFlag",
                table: "OrderConfirmations",
                column: "DeletionFlag");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_StartTime",
                table: "OrderConfirmations",
                column: "StartTime");
        }

        #endregion Methods
    }
}
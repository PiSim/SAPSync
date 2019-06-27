using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class operationvalues : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderData_Materials_MaterialID",
                table: "OrderData");

            migrationBuilder.DropIndex(
                name: "IX_OrderData_MaterialID",
                table: "OrderData");

            migrationBuilder.DropColumn(
                name: "BaseQuantity",
                table: "RoutingOperations");

            migrationBuilder.DropColumn(
                name: "MaterialID",
                table: "OrderData");

            migrationBuilder.AddColumn<int>(
                name: "MaterialID",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MaterialID",
                table: "Orders",
                column: "MaterialID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Materials_MaterialID",
                table: "Orders",
                column: "MaterialID",
                principalTable: "Materials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseQuantity",
                table: "RoutingOperations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Materials_MaterialID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_MaterialID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaterialID",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "MaterialID",
                table: "OrderData",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderData_MaterialID",
                table: "OrderData",
                column: "MaterialID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderData_Materials_MaterialID",
                table: "OrderData",
                column: "MaterialID",
                principalTable: "Materials",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        #endregion Methods
    }
}
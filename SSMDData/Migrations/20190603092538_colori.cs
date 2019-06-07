using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class colori : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoutingOperations_OrderData_RoutingNumber",
                table: "RoutingOperations");

            migrationBuilder.DropTable(
                name: "OrderData");

            migrationBuilder.DropColumn(
                name: "ControlPlan",
                table: "Materials");

            migrationBuilder.AddColumn<double>(
                name: "PlannedQuantity",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedQuantity",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ControlPlan",
                table: "Materials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderData",
                columns: table => new
                {
                    OrderNumber = table.Column<int>(nullable: false),
                    RoutingNumber = table.Column<long>(nullable: false),
                    PlannedQuantity = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderData", x => x.OrderNumber);
                    table.UniqueConstraint("AK_OrderData_RoutingNumber", x => x.RoutingNumber);
                    table.ForeignKey(
                        name: "FK_OrderData_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RoutingOperations_OrderData_RoutingNumber",
                table: "RoutingOperations",
                column: "RoutingNumber",
                principalTable: "OrderData",
                principalColumn: "RoutingNumber",
                onDelete: ReferentialAction.Cascade);
        }

        #endregion Methods
    }
}
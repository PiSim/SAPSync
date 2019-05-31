using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class routing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ControlPlanNumber",
                table: "Orders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PlannedQuantity",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "RoutingNumber",
                table: "Orders",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "MaterialFamilyLevels",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InspectionCharacteristics",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Components",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Components",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoutingOperations",
                columns: table => new
                {
                    RoutingNumber = table.Column<long>(nullable: false),
                    RoutingCounter = table.Column<int>(nullable: false),
                    WorkCenterID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutingOperations", x => new { x.RoutingNumber, x.RoutingCounter });
                    table.ForeignKey(
                        name: "FK_RoutingOperations_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_Name",
                table: "Components",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_RoutingOperations_WorkCenterID",
                table: "RoutingOperations",
                column: "WorkCenterID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoutingOperations");

            migrationBuilder.DropIndex(
                name: "IX_Components_Name",
                table: "Components");

            migrationBuilder.DropColumn(
                name: "ControlPlanNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PlannedQuantity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "RoutingNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Components");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "MaterialFamilyLevels",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "InspectionCharacteristics",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Components",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

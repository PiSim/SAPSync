using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class workcenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkCenters",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCenters", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_WorkCenterID",
                table: "OrderConfirmations",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_ShortName",
                table: "WorkCenters",
                column: "ShortName");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderConfirmations_WorkCenters_WorkCenterID",
                table: "OrderConfirmations",
                column: "WorkCenterID",
                principalTable: "WorkCenters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderConfirmations_WorkCenters_WorkCenterID",
                table: "OrderConfirmations");

            migrationBuilder.DropTable(
                name: "WorkCenters");

            migrationBuilder.DropIndex(
                name: "IX_OrderConfirmations_WorkCenterID",
                table: "OrderConfirmations");
        }
    }
}

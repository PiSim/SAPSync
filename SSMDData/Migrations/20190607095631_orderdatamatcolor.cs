using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class orderdatamatcolor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColorComponentID",
                table: "Materials",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ColorComponentID",
                table: "Materials",
                column: "ColorComponentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Components_ColorComponentID",
                table: "Materials",
                column: "ColorComponentID",
                principalTable: "Components",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Components_ColorComponentID",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ColorComponentID",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ColorComponentID",
                table: "Materials");
        }
    }
}

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class wbsfix3 : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WBSRelations_Projects_ID",
                table: "WBSRelations");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "WBSRelations",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_ProjectID",
                table: "WBSRelations",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_WBSRelations_Projects_ProjectID",
                table: "WBSRelations",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WBSRelations_Projects_ProjectID",
                table: "WBSRelations");

            migrationBuilder.DropIndex(
                name: "IX_WBSRelations_ProjectID",
                table: "WBSRelations");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "WBSRelations",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_WBSRelations_Projects_ID",
                table: "WBSRelations",
                column: "ID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        #endregion Methods
    }
}
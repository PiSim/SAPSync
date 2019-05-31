using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class uniquematcode : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materials_Code",
                table: "Materials");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Materials_Code",
                table: "Materials");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code",
                unique: true);
        }

        #endregion Methods
    }
}
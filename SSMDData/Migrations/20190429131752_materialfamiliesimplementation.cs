using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class materialfamiliesimplementation : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L1ID",
                table: "MaterialFamilies");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L2ID",
                table: "MaterialFamilies");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L3ID",
                table: "MaterialFamilies");

            migrationBuilder.DropTable(
                name: "MaterialFamilyLevels");

            migrationBuilder.DropIndex(
                name: "IX_MaterialFamilies_L1ID",
                table: "MaterialFamilies");

            migrationBuilder.DropIndex(
                name: "IX_MaterialFamilies_L2ID",
                table: "MaterialFamilies");

            migrationBuilder.DropIndex(
                name: "IX_MaterialFamilies_L3ID",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L1ID",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L2ID",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L3ID",
                table: "MaterialFamilies");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L1",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L1Description",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L2",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L2Description",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L3",
                table: "MaterialFamilies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "L3Description",
                table: "MaterialFamilies",
                nullable: true);
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L1",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L1Description",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L2",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L2Description",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L3",
                table: "MaterialFamilies");

            migrationBuilder.DropColumn(
                name: "L3Description",
                table: "MaterialFamilies");

            migrationBuilder.AddColumn<int>(
                name: "L1ID",
                table: "MaterialFamilies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "L2ID",
                table: "MaterialFamilies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "L3ID",
                table: "MaterialFamilies",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MaterialFamilyLevels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialFamilyLevels", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialFamilies_L1ID",
                table: "MaterialFamilies",
                column: "L1ID");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialFamilies_L2ID",
                table: "MaterialFamilies",
                column: "L2ID");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialFamilies_L3ID",
                table: "MaterialFamilies",
                column: "L3ID");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialFamilyLevels_Level_Code",
                table: "MaterialFamilyLevels",
                columns: new[] { "Level", "Code" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L1ID",
                table: "MaterialFamilies",
                column: "L1ID",
                principalTable: "MaterialFamilyLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L2ID",
                table: "MaterialFamilies",
                column: "L2ID",
                principalTable: "MaterialFamilyLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialFamilies_MaterialFamilyLevels_L3ID",
                table: "MaterialFamilies",
                column: "L3ID",
                principalTable: "MaterialFamilyLevels",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        #endregion Methods
    }
}
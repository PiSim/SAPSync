using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class wbs : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Projects_ProjectID",
                table: "Materials");

            migrationBuilder.DropTable(
                name: "WBSRelations");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Materials_ProjectID",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Materials");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectID",
                table: "Materials",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    WBSLevel = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WBSRelations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<int>(nullable: false),
                    UpID = table.Column<int>(nullable: true),
                    DownID = table.Column<int>(nullable: true),
                    RightID = table.Column<int>(nullable: true),
                    LeftID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WBSRelations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_DownID",
                        column: x => x.DownID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_LeftID",
                        column: x => x.LeftID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_RightID",
                        column: x => x.RightID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_UpID",
                        column: x => x.UpID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProjectID",
                table: "Materials",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Code",
                table: "Projects",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_DownID",
                table: "WBSRelations",
                column: "DownID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_LeftID",
                table: "WBSRelations",
                column: "LeftID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_ProjectID",
                table: "WBSRelations",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_RightID",
                table: "WBSRelations",
                column: "RightID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_UpID",
                table: "WBSRelations",
                column: "UpID");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Projects_ProjectID",
                table: "Materials",
                column: "ProjectID",
                principalTable: "Projects",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        #endregion Methods
    }
}
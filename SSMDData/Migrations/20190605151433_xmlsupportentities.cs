using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class xmlsupportentities : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestReports");

            migrationBuilder.DropTable(
                name: "WorkPhaseLabData");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TestReports",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    HasRollArrived = table.Column<bool>(nullable: false),
                    RollStatus = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Weight = table.Column<double>(nullable: true),
                    Thickness = table.Column<double>(nullable: true),
                    ColorJudgement = table.Column<string>(nullable: true),
                    Gloss = table.Column<double>(nullable: true),
                    GlossZ = table.Column<double>(nullable: true),
                    FlammabilityEvaluation = table.Column<string>(nullable: true),
                    DetachForceL = table.Column<double>(nullable: true),
                    DetachForceT = table.Column<double>(nullable: true),
                    BreakingLoadL = table.Column<double>(nullable: true),
                    BreakingLoadT = table.Column<double>(nullable: true),
                    BreakingElongationL = table.Column<double>(nullable: true),
                    BreakingElongationT = table.Column<double>(nullable: true),
                    StretchL = table.Column<double>(nullable: true),
                    StretchT = table.Column<double>(nullable: true),
                    SetL = table.Column<double>(nullable: true),
                    SetT = table.Column<double>(nullable: true),
                    OtherTests = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestReports", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "WorkPhaseLabData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    WorkPhase = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPhaseLabData", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkPhaseLabData_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkPhaseLabData_OrderNumber",
                table: "WorkPhaseLabData",
                column: "OrderNumber");
        }

        #endregion Methods
    }
}
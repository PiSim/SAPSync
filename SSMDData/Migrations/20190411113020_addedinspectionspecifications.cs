using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class addedinspectionspecifications : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InspectionPoints_InspectionSpecifications_InspectionLotNumbe~",
                table: "InspectionPoints");

            migrationBuilder.DropTable(
                name: "InspectionSpecifications");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionSpecifications",
                columns: table => new
                {
                    InspectionLotNumber = table.Column<long>(nullable: false),
                    NodeNumber = table.Column<int>(nullable: false),
                    CharacteristicNumber = table.Column<int>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    InspectionCharacteristicID = table.Column<int>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    UpperSpecificationLimit = table.Column<double>(nullable: false),
                    LowerSpecificationLimit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionSpecifications", x => new { x.InspectionLotNumber, x.NodeNumber, x.CharacteristicNumber });
                    table.ForeignKey(
                        name: "FK_InspectionSpecifications_InspectionCharacteristics_Inspectio~",
                        column: x => x.InspectionCharacteristicID,
                        principalTable: "InspectionCharacteristics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionSpecifications_InspectionLots_InspectionLotNumber",
                        column: x => x.InspectionLotNumber,
                        principalTable: "InspectionLots",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionSpecifications_InspectionCharacteristicID",
                table: "InspectionSpecifications",
                column: "InspectionCharacteristicID");

            migrationBuilder.AddForeignKey(
                name: "FK_InspectionPoints_InspectionSpecifications_InspectionLotNumbe~",
                table: "InspectionPoints",
                columns: new[] { "InspectionLotNumber", "NodeNumber", "CharNumber" },
                principalTable: "InspectionSpecifications",
                principalColumns: new[] { "InspectionLotNumber", "NodeNumber", "CharacteristicNumber" },
                onDelete: ReferentialAction.Cascade);
        }

        #endregion Methods
    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class regeneratedDb : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionPoints");

            migrationBuilder.DropTable(
                name: "OrderComponents");

            migrationBuilder.DropTable(
                name: "OrderConfirmations");

            migrationBuilder.DropTable(
                name: "ScrapCauses");

            migrationBuilder.DropTable(
                name: "InspectionCharacteristics");

            migrationBuilder.DropTable(
                name: "InspectionLots");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialFamilies");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionCharacteristics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UpperSpecificationLimit = table.Column<double>(nullable: false),
                    LowerSpecificationLimit = table.Column<double>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionCharacteristics", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialFamilies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    L1 = table.Column<string>(nullable: true),
                    L1Description = table.Column<string>(nullable: true),
                    L2 = table.Column<string>(nullable: true),
                    L2Description = table.Column<string>(nullable: true),
                    L3 = table.Column<string>(nullable: true),
                    L3Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialFamilies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ScrapCauses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapCauses", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    MaterialFamilyID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialFamilies_MaterialFamilyID",
                        column: x => x.MaterialFamilyID,
                        principalTable: "MaterialFamilies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ID = table.Column<int>(nullable: false),
                    TotalQuantity = table.Column<double>(nullable: false),
                    TotalScrap = table.Column<double>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false),
                    OrderType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Orders_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionLots",
                columns: table => new
                {
                    Number = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionLots", x => x.Number);
                    table.ForeignKey(
                        name: "FK_InspectionLots_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderComponents",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComponentID = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderComponents", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderComponents_Components_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "Components",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderComponents_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderConfirmations",
                columns: table => new
                {
                    ConfirmationCounter = table.Column<int>(nullable: false),
                    ConfirmationNumber = table.Column<int>(nullable: false),
                    DeletionFlag = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    Yield = table.Column<double>(nullable: false),
                    Scrap = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    ScrapCause = table.Column<string>(nullable: true),
                    WIPIn = table.Column<string>(nullable: true),
                    WIPOut = table.Column<string>(nullable: true),
                    WorkCenterID = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderConfirmations", x => new { x.ConfirmationNumber, x.ConfirmationCounter });
                    table.ForeignKey(
                        name: "FK_OrderConfirmations_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionPoints",
                columns: table => new
                {
                    InspectionLotNumber = table.Column<long>(nullable: false),
                    NodeNumber = table.Column<int>(nullable: false),
                    CharNumber = table.Column<int>(nullable: false),
                    SampleNumber = table.Column<int>(nullable: false),
                    InspectionDate = table.Column<DateTime>(nullable: false),
                    MaxValue = table.Column<double>(nullable: false),
                    MinValue = table.Column<double>(nullable: false),
                    AvgValue = table.Column<double>(nullable: false),
                    InspectionCharacteristicID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionPoints", x => new { x.InspectionLotNumber, x.NodeNumber, x.CharNumber, x.SampleNumber });
                    table.ForeignKey(
                        name: "FK_InspectionPoints_InspectionCharacteristics_InspectionCharact~",
                        column: x => x.InspectionCharacteristicID,
                        principalTable: "InspectionCharacteristics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionPoints_InspectionLots_InspectionLotNumber",
                        column: x => x.InspectionLotNumber,
                        principalTable: "InspectionLots",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionCharacteristics_Name",
                table: "InspectionCharacteristics",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLots_OrderNumber",
                table: "InspectionLots",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPoints_InspectionCharacteristicID",
                table: "InspectionPoints",
                column: "InspectionCharacteristicID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialFamilyID",
                table: "Materials",
                column: "MaterialFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComponents_ComponentID",
                table: "OrderComponents",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComponents_OrderNumber",
                table: "OrderComponents",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_OrderNumber",
                table: "OrderConfirmations",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MaterialID",
                table: "Orders",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderType",
                table: "Orders",
                column: "OrderType");
        }

        #endregion Methods
    }
}
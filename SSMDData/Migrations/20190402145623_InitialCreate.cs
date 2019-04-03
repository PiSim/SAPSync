using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SSMD.Migrations
{
    public partial class InitialCreate : Migration
    {
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
                    Description = table.Column<string>(nullable: true)
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
                    Code = table.Column<string>(nullable: true)
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
                    InspectionLotNumber = table.Column<int>(nullable: true),
                    TotalQuantity = table.Column<double>(nullable: false),
                    TotalScrap = table.Column<double>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false)
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
                    LotNumber = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionLots", x => x.LotNumber);
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
                    OrderNumber = table.Column<int>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    InspectionCharacteristicNumber = table.Column<int>(nullable: false)
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
                name: "InspectionOperations",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InspectionCharacteristicID = table.Column<int>(nullable: false),
                    InspectionLotNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionOperations", x => x.Number);
                    table.ForeignKey(
                        name: "FK_InspectionOperations_InspectionCharacteristics_InspectionCha~",
                        column: x => x.InspectionCharacteristicID,
                        principalTable: "InspectionCharacteristics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InspectionOperations_InspectionLots_InspectionLotNumber",
                        column: x => x.InspectionLotNumber,
                        principalTable: "InspectionLots",
                        principalColumn: "LotNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionPoints",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InspectionDate = table.Column<DateTime>(nullable: false),
                    InspectionOperationNumber = table.Column<int>(nullable: false),
                    UM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionPoints", x => x.Number);
                    table.ForeignKey(
                        name: "FK_InspectionPoints_InspectionOperations_InspectionOperationNum~",
                        column: x => x.InspectionOperationNumber,
                        principalTable: "InspectionOperations",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLots_OrderNumber",
                table: "InspectionLots",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionOperations_InspectionCharacteristicID",
                table: "InspectionOperations",
                column: "InspectionCharacteristicID");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionOperations_InspectionLotNumber",
                table: "InspectionOperations",
                column: "InspectionLotNumber");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionPoints_InspectionOperationNumber",
                table: "InspectionPoints",
                column: "InspectionOperationNumber");

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
        }

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
                name: "InspectionOperations");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "InspectionCharacteristics");

            migrationBuilder.DropTable(
                name: "InspectionLots");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "MaterialFamilies");
        }
    }
}

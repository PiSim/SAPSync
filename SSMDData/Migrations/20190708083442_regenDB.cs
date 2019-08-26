using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SSMD.Migrations
{
    public partial class regenDB : Migration
    {
        #region Methods

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoodMovements");

            migrationBuilder.DropTable(
                name: "InspectionPoints");

            migrationBuilder.DropTable(
                name: "MaterialCustomers");

            migrationBuilder.DropTable(
                name: "OrderComponents");

            migrationBuilder.DropTable(
                name: "OrderConfirmations");

            migrationBuilder.DropTable(
                name: "RoutingOperations");

            migrationBuilder.DropTable(
                name: "ScrapCauses");

            migrationBuilder.DropTable(
                name: "SyncElementData");

            migrationBuilder.DropTable(
                name: "TestReports");

            migrationBuilder.DropTable(
                name: "WBSRelations");

            migrationBuilder.DropTable(
                name: "WorkPhaseLabData");

            migrationBuilder.DropTable(
                name: "InspectionSpecifications");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "OrderData");

            migrationBuilder.DropTable(
                name: "WorkCenters");

            migrationBuilder.DropTable(
                name: "InspectionCharacteristics");

            migrationBuilder.DropTable(
                name: "InspectionLots");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Components");

            migrationBuilder.DropTable(
                name: "MaterialFamilies");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "MaterialFamilyLevels");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Components",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Components", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Name2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InspectionCharacteristics",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    LowerSpecificationLimit = table.Column<double>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    UpperSpecificationLimit = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionCharacteristics", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MaterialFamilyLevels",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialFamilyLevels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ControlPlanNumber = table.Column<int>(nullable: false),
                    ID = table.Column<int>(nullable: false),
                    OrderType = table.Column<string>(nullable: true),
                    RoutingNumber = table.Column<long>(nullable: false),
                    TotalQuantity = table.Column<double>(nullable: false),
                    TotalScrap = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Description2 = table.Column<string>(nullable: true),
                    WBSLevel = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ID);
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
                name: "SyncElementData",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ElementType = table.Column<string>(nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    UpdateInterval = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncElementData", x => x.ID);
                });

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

            migrationBuilder.CreateTable(
                name: "MaterialFamilies",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    L1ID = table.Column<int>(nullable: false),
                    L2ID = table.Column<int>(nullable: false),
                    L3ID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialFamilies", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MaterialFamilies_MaterialFamilyLevels_L1ID",
                        column: x => x.L1ID,
                        principalTable: "MaterialFamilyLevels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialFamilies_MaterialFamilyLevels_L2ID",
                        column: x => x.L2ID,
                        principalTable: "MaterialFamilyLevels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialFamilies_MaterialFamilyLevels_L3ID",
                        column: x => x.L3ID,
                        principalTable: "MaterialFamilyLevels",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoodMovements",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ComponentID = table.Column<int>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoodMovements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GoodMovements_Components_ComponentID",
                        column: x => x.ComponentID,
                        principalTable: "Components",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoodMovements_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
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
                name: "TestReports",
                columns: table => new
                {
                    Number = table.Column<int>(nullable: false),
                    BreakingElongationL = table.Column<double>(nullable: true),
                    BreakingElongationT = table.Column<double>(nullable: true),
                    BreakingLoadL = table.Column<double>(nullable: true),
                    BreakingLoadT = table.Column<double>(nullable: true),
                    ColorJudgement = table.Column<string>(nullable: true),
                    DetachForceL = table.Column<double>(nullable: true),
                    DetachForceT = table.Column<double>(nullable: true),
                    FlammabilityEvaluation = table.Column<string>(nullable: true),
                    Gloss = table.Column<double>(nullable: true),
                    GlossZ = table.Column<double>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Notes2 = table.Column<string>(nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    OrderNumber = table.Column<int>(nullable: true),
                    OtherTests = table.Column<string>(nullable: true),
                    SetL = table.Column<double>(nullable: true),
                    SetT = table.Column<double>(nullable: true),
                    StretchL = table.Column<double>(nullable: true),
                    StretchT = table.Column<double>(nullable: true),
                    Thickness = table.Column<double>(nullable: true),
                    Weight = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestReports", x => x.Number);
                    table.ForeignKey(
                        name: "FK_TestReports_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkPhaseLabData",
                columns: table => new
                {
                    OrderNumber = table.Column<int>(nullable: false),
                    Actions = table.Column<string>(nullable: true),
                    Analysis = table.Column<string>(nullable: true),
                    NotesC = table.Column<string>(nullable: true),
                    NotesG = table.Column<string>(nullable: true),
                    NotesP = table.Column<string>(nullable: true),
                    NotesS = table.Column<string>(nullable: true),
                    TrialScope = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkPhaseLabData", x => x.OrderNumber);
                    table.ForeignKey(
                        name: "FK_WorkPhaseLabData_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WBSRelations",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    DownID = table.Column<int>(nullable: true),
                    LeftID = table.Column<int>(nullable: true),
                    ProjectID = table.Column<int>(nullable: false),
                    RightID = table.Column<int>(nullable: true),
                    UpID = table.Column<int>(nullable: true)
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
                        name: "FK_WBSRelations_Projects_ID",
                        column: x => x.ID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WBSRelations_Projects_LeftID",
                        column: x => x.LeftID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.CreateTable(
                name: "OrderConfirmations",
                columns: table => new
                {
                    ConfirmationCounter = table.Column<int>(nullable: false),
                    ConfirmationNumber = table.Column<int>(nullable: false),
                    DeletionFlag = table.Column<bool>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: true),
                    IsRework = table.Column<bool>(nullable: false),
                    OrderNumber = table.Column<int>(nullable: false),
                    ReworkAmount = table.Column<double>(nullable: false),
                    ReworkScrap = table.Column<double>(nullable: false),
                    Scrap = table.Column<double>(nullable: false),
                    ScrapCause = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    WIPIn = table.Column<string>(nullable: true),
                    WIPOut = table.Column<string>(nullable: true),
                    WorkCenterID = table.Column<int>(nullable: false),
                    Yield = table.Column<double>(nullable: false)
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
                    table.ForeignKey(
                        name: "FK_OrderConfirmations_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    ColorComponentID = table.Column<int>(nullable: true),
                    ControlPlan = table.Column<int>(nullable: false),
                    MaterialFamilyID = table.Column<int>(nullable: true),
                    ProjectID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Materials_Components_ColorComponentID",
                        column: x => x.ColorComponentID,
                        principalTable: "Components",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_MaterialFamilies_MaterialFamilyID",
                        column: x => x.MaterialFamilyID,
                        principalTable: "MaterialFamilies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionSpecifications",
                columns: table => new
                {
                    CharacteristicNumber = table.Column<int>(nullable: false),
                    InspectionLotNumber = table.Column<long>(nullable: false),
                    NodeNumber = table.Column<int>(nullable: false),
                    InspectionCharacteristicID = table.Column<int>(nullable: false),
                    LowerSpecificationLimit = table.Column<double>(nullable: false),
                    TargetValue = table.Column<double>(nullable: false),
                    UM = table.Column<string>(nullable: true),
                    UpperSpecificationLimit = table.Column<double>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "MaterialCustomers",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialCustomers", x => new { x.MaterialID, x.CustomerID });
                    table.ForeignKey(
                        name: "FK_MaterialCustomers_Customers_CustomerID",
                        column: x => x.CustomerID,
                        principalTable: "Customers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialCustomers_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderData",
                columns: table => new
                {
                    OrderNumber = table.Column<int>(nullable: false),
                    HasSampleArrived = table.Column<bool>(nullable: false),
                    MaterialID = table.Column<int>(nullable: false),
                    PlannedQuantity = table.Column<double>(nullable: false),
                    RoutingNumber = table.Column<long>(nullable: false),
                    SampleArrivalDate = table.Column<DateTime>(nullable: true),
                    SampleRollStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderData", x => x.OrderNumber);
                    table.UniqueConstraint("AK_OrderData_RoutingNumber", x => x.RoutingNumber);
                    table.ForeignKey(
                        name: "FK_OrderData_Materials_MaterialID",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderData_Orders_OrderNumber",
                        column: x => x.OrderNumber,
                        principalTable: "Orders",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InspectionPoints",
                columns: table => new
                {
                    CharNumber = table.Column<int>(nullable: false),
                    InspectionLotNumber = table.Column<long>(nullable: false),
                    NodeNumber = table.Column<int>(nullable: false),
                    SampleNumber = table.Column<int>(nullable: false),
                    AvgValue = table.Column<double>(nullable: false),
                    InspectionDate = table.Column<DateTime>(nullable: false),
                    MaxValue = table.Column<double>(nullable: false),
                    MinValue = table.Column<double>(nullable: false),
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
                    table.ForeignKey(
                        name: "FK_InspectionPoints_InspectionSpecifications_InspectionLotNumbe~",
                        columns: x => new { x.InspectionLotNumber, x.NodeNumber, x.CharNumber },
                        principalTable: "InspectionSpecifications",
                        principalColumns: new[] { "InspectionLotNumber", "NodeNumber", "CharacteristicNumber" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutingOperations",
                columns: table => new
                {
                    RoutingCounter = table.Column<int>(nullable: false),
                    RoutingNumber = table.Column<long>(nullable: false),
                    BaseQuantity = table.Column<int>(nullable: false),
                    WorkCenterID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutingOperations", x => new { x.RoutingNumber, x.RoutingCounter });
                    table.ForeignKey(
                        name: "FK_RoutingOperations_OrderData_RoutingNumber",
                        column: x => x.RoutingNumber,
                        principalTable: "OrderData",
                        principalColumn: "RoutingNumber",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoutingOperations_WorkCenters_WorkCenterID",
                        column: x => x.WorkCenterID,
                        principalTable: "WorkCenters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Components_Name",
                table: "Components",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GoodMovements_ComponentID",
                table: "GoodMovements",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_GoodMovements_OrderNumber",
                table: "GoodMovements",
                column: "OrderNumber");

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
                name: "IX_InspectionSpecifications_InspectionCharacteristicID",
                table: "InspectionSpecifications",
                column: "InspectionCharacteristicID");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialCustomers_CustomerID",
                table: "MaterialCustomers",
                column: "CustomerID");

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

            migrationBuilder.CreateIndex(
                name: "IX_Materials_Code",
                table: "Materials",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ColorComponentID",
                table: "Materials",
                column: "ColorComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_MaterialFamilyID",
                table: "Materials",
                column: "MaterialFamilyID");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ProjectID",
                table: "Materials",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComponents_ComponentID",
                table: "OrderComponents",
                column: "ComponentID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderComponents_OrderNumber",
                table: "OrderComponents",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_DeletionFlag",
                table: "OrderConfirmations",
                column: "DeletionFlag");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_OrderNumber",
                table: "OrderConfirmations",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_StartTime",
                table: "OrderConfirmations",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_OrderConfirmations_WorkCenterID",
                table: "OrderConfirmations",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderData_MaterialID",
                table: "OrderData",
                column: "MaterialID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderType",
                table: "Orders",
                column: "OrderType");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Code",
                table: "Projects",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_RoutingOperations_WorkCenterID",
                table: "RoutingOperations",
                column: "WorkCenterID");

            migrationBuilder.CreateIndex(
                name: "IX_TestReports_OrderNumber",
                table: "TestReports",
                column: "OrderNumber");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_DownID",
                table: "WBSRelations",
                column: "DownID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_LeftID",
                table: "WBSRelations",
                column: "LeftID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_RightID",
                table: "WBSRelations",
                column: "RightID");

            migrationBuilder.CreateIndex(
                name: "IX_WBSRelations_UpID",
                table: "WBSRelations",
                column: "UpID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_ShortName",
                table: "WorkCenters",
                column: "ShortName");
        }

        #endregion Methods
    }
}
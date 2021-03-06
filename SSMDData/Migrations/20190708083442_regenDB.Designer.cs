﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SSMD;

namespace SSMD.Migrations
{
    [DbContext(typeof(SSMDContext))]
    [Migration("20190708083442_regenDB")]
    partial class regenDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SSMD.Component", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("Name");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("SSMD.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Name2");

                    b.HasKey("ID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("SSMD.GoodMovement", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentID");

                    b.Property<int>("OrderNumber");

                    b.Property<double>("Quantity");

                    b.Property<string>("UM");

                    b.HasKey("ID");

                    b.HasIndex("ComponentID");

                    b.HasIndex("OrderNumber");

                    b.ToTable("GoodMovements");
                });

            modelBuilder.Entity("SSMD.InspectionCharacteristic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("LowerSpecificationLimit");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<double>("TargetValue");

                    b.Property<string>("UM");

                    b.Property<double>("UpperSpecificationLimit");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("InspectionCharacteristics");
                });

            modelBuilder.Entity("SSMD.InspectionLot", b =>
                {
                    b.Property<long>("Number")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OrderNumber");

                    b.HasKey("Number");

                    b.HasIndex("OrderNumber");

                    b.ToTable("InspectionLots");
                });

            modelBuilder.Entity("SSMD.InspectionPoint", b =>
                {
                    b.Property<long>("InspectionLotNumber");

                    b.Property<int>("NodeNumber");

                    b.Property<int>("CharNumber");

                    b.Property<int>("SampleNumber");

                    b.Property<double>("AvgValue");

                    b.Property<int?>("InspectionCharacteristicID");

                    b.Property<DateTime>("InspectionDate");

                    b.Property<double>("MaxValue");

                    b.Property<double>("MinValue");

                    b.HasKey("InspectionLotNumber", "NodeNumber", "CharNumber", "SampleNumber");

                    b.HasIndex("InspectionCharacteristicID");

                    b.ToTable("InspectionPoints");
                });

            modelBuilder.Entity("SSMD.InspectionSpecification", b =>
                {
                    b.Property<long>("InspectionLotNumber");

                    b.Property<int>("NodeNumber");

                    b.Property<int>("CharacteristicNumber");

                    b.Property<int>("InspectionCharacteristicID");

                    b.Property<double>("LowerSpecificationLimit");

                    b.Property<double>("TargetValue");

                    b.Property<string>("UM");

                    b.Property<double>("UpperSpecificationLimit");

                    b.HasKey("InspectionLotNumber", "NodeNumber", "CharacteristicNumber");

                    b.HasIndex("InspectionCharacteristicID");

                    b.ToTable("InspectionSpecifications");
                });

            modelBuilder.Entity("SSMD.Material", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<int?>("ColorComponentID");

                    b.Property<int>("ControlPlan");

                    b.Property<int?>("MaterialFamilyID");

                    b.Property<int?>("ProjectID");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("ColorComponentID");

                    b.HasIndex("MaterialFamilyID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("SSMD.MaterialCustomer", b =>
                {
                    b.Property<int>("MaterialID");

                    b.Property<int>("CustomerID");

                    b.HasKey("MaterialID", "CustomerID");

                    b.HasIndex("CustomerID");

                    b.ToTable("MaterialCustomers");
                });

            modelBuilder.Entity("SSMD.MaterialFamily", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("L1ID");

                    b.Property<int>("L2ID");

                    b.Property<int>("L3ID");

                    b.HasKey("ID");

                    b.HasIndex("L1ID");

                    b.HasIndex("L2ID");

                    b.HasIndex("L3ID");

                    b.ToTable("MaterialFamilies");
                });

            modelBuilder.Entity("SSMD.MaterialFamilyLevel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<int>("Level");

                    b.HasKey("ID");

                    b.HasIndex("Level", "Code")
                        .IsUnique();

                    b.ToTable("MaterialFamilyLevels");
                });

            modelBuilder.Entity("SSMD.Order", b =>
                {
                    b.Property<int>("Number")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ControlPlanNumber");

                    b.Property<int>("ID");

                    b.Property<string>("OrderType");

                    b.Property<long>("RoutingNumber");

                    b.Property<double>("TotalQuantity");

                    b.Property<double>("TotalScrap");

                    b.HasKey("Number");

                    b.HasIndex("OrderType");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("SSMD.OrderComponent", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentID");

                    b.Property<int>("OrderNumber");

                    b.HasKey("ID");

                    b.HasIndex("ComponentID");

                    b.HasIndex("OrderNumber");

                    b.ToTable("OrderComponents");
                });

            modelBuilder.Entity("SSMD.OrderConfirmation", b =>
                {
                    b.Property<int>("ConfirmationNumber");

                    b.Property<int>("ConfirmationCounter");

                    b.Property<bool>("DeletionFlag");

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime?>("EntryDate");

                    b.Property<bool>("IsRework");

                    b.Property<int>("OrderNumber");

                    b.Property<double>("ReworkAmount");

                    b.Property<double>("ReworkScrap");

                    b.Property<double>("Scrap");

                    b.Property<string>("ScrapCause");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("UM");

                    b.Property<string>("WIPIn");

                    b.Property<string>("WIPOut");

                    b.Property<int>("WorkCenterID");

                    b.Property<double>("Yield");

                    b.HasKey("ConfirmationNumber", "ConfirmationCounter");

                    b.HasIndex("DeletionFlag");

                    b.HasIndex("OrderNumber");

                    b.HasIndex("StartTime");

                    b.HasIndex("WorkCenterID");

                    b.ToTable("OrderConfirmations");
                });

            modelBuilder.Entity("SSMD.OrderData", b =>
                {
                    b.Property<int>("OrderNumber");

                    b.Property<bool>("HasSampleArrived");

                    b.Property<int>("MaterialID");

                    b.Property<double>("PlannedQuantity");

                    b.Property<long>("RoutingNumber");

                    b.Property<DateTime?>("SampleArrivalDate");

                    b.Property<string>("SampleRollStatus");

                    b.HasKey("OrderNumber");

                    b.HasIndex("MaterialID");

                    b.ToTable("OrderData");
                });

            modelBuilder.Entity("SSMD.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<string>("Description2");

                    b.Property<int>("WBSLevel");

                    b.HasKey("ID");

                    b.HasIndex("Code");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("SSMD.RoutingOperation", b =>
                {
                    b.Property<long>("RoutingNumber");

                    b.Property<int>("RoutingCounter");

                    b.Property<int>("BaseQuantity");

                    b.Property<int>("WorkCenterID");

                    b.HasKey("RoutingNumber", "RoutingCounter");

                    b.HasIndex("WorkCenterID");

                    b.ToTable("RoutingOperations");
                });

            modelBuilder.Entity("SSMD.ScrapCause", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.HasKey("ID");

                    b.ToTable("ScrapCauses");
                });

            modelBuilder.Entity("SSMD.SyncElementData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ElementType");

                    b.Property<DateTime>("LastUpdate");

                    b.Property<int>("UpdateInterval");

                    b.HasKey("ID");

                    b.ToTable("SyncElementData");
                });

            modelBuilder.Entity("SSMD.TestReport", b =>
                {
                    b.Property<int>("Number");

                    b.Property<double?>("BreakingElongationL");

                    b.Property<double?>("BreakingElongationT");

                    b.Property<double?>("BreakingLoadL");

                    b.Property<double?>("BreakingLoadT");

                    b.Property<string>("ColorJudgement");

                    b.Property<double?>("DetachForceL");

                    b.Property<double?>("DetachForceT");

                    b.Property<string>("FlammabilityEvaluation");

                    b.Property<double?>("Gloss");

                    b.Property<double?>("GlossZ");

                    b.Property<string>("Notes");

                    b.Property<string>("Notes2");

                    b.Property<string>("Operator");

                    b.Property<int?>("OrderNumber");

                    b.Property<string>("OtherTests");

                    b.Property<double?>("SetL");

                    b.Property<double?>("SetT");

                    b.Property<double?>("StretchL");

                    b.Property<double?>("StretchT");

                    b.Property<double?>("Thickness");

                    b.Property<double?>("Weight");

                    b.HasKey("Number");

                    b.HasIndex("OrderNumber");

                    b.ToTable("TestReports");
                });

            modelBuilder.Entity("SSMD.WBSRelation", b =>
                {
                    b.Property<int>("ID");

                    b.Property<int?>("DownID");

                    b.Property<int?>("LeftID");

                    b.Property<int>("ProjectID");

                    b.Property<int?>("RightID");

                    b.Property<int?>("UpID");

                    b.HasKey("ID");

                    b.HasIndex("DownID");

                    b.HasIndex("LeftID");

                    b.HasIndex("RightID");

                    b.HasIndex("UpID");

                    b.ToTable("WBSRelations");
                });

            modelBuilder.Entity("SSMD.WorkCenter", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ShortName");

                    b.HasKey("ID");

                    b.HasIndex("ShortName");

                    b.ToTable("WorkCenters");
                });

            modelBuilder.Entity("SSMD.WorkPhaseLabData", b =>
                {
                    b.Property<int>("OrderNumber");

                    b.Property<string>("Actions");

                    b.Property<string>("Analysis");

                    b.Property<string>("NotesC");

                    b.Property<string>("NotesG");

                    b.Property<string>("NotesP");

                    b.Property<string>("NotesS");

                    b.Property<string>("TrialScope");

                    b.HasKey("OrderNumber");

                    b.ToTable("WorkPhaseLabData");
                });

            modelBuilder.Entity("SSMD.GoodMovement", b =>
                {
                    b.HasOne("SSMD.Component", "Component")
                        .WithMany("GoodMovements")
                        .HasForeignKey("ComponentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("GoodMovements")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.InspectionLot", b =>
                {
                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("InspectionLots")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.InspectionPoint", b =>
                {
                    b.HasOne("SSMD.InspectionCharacteristic")
                        .WithMany("InspectionPoints")
                        .HasForeignKey("InspectionCharacteristicID");

                    b.HasOne("SSMD.InspectionLot", "InspectionLot")
                        .WithMany("InspectionPoints")
                        .HasForeignKey("InspectionLotNumber")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.InspectionSpecification", "InspectionSpecification")
                        .WithMany("InspectionPoints")
                        .HasForeignKey("InspectionLotNumber", "NodeNumber", "CharNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.InspectionSpecification", b =>
                {
                    b.HasOne("SSMD.InspectionCharacteristic", "InspectionCharacteristic")
                        .WithMany()
                        .HasForeignKey("InspectionCharacteristicID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.InspectionLot", "InspectionLot")
                        .WithMany()
                        .HasForeignKey("InspectionLotNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.Material", b =>
                {
                    b.HasOne("SSMD.Component", "ColorComponent")
                        .WithMany()
                        .HasForeignKey("ColorComponentID");

                    b.HasOne("SSMD.MaterialFamily", "MaterialFamily")
                        .WithMany("Materials")
                        .HasForeignKey("MaterialFamilyID");

                    b.HasOne("SSMD.Project", "Project")
                        .WithMany("Materials")
                        .HasForeignKey("ProjectID");
                });

            modelBuilder.Entity("SSMD.MaterialCustomer", b =>
                {
                    b.HasOne("SSMD.Customer", "Customer")
                        .WithMany("MaterialCustomers")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Material", "Material")
                        .WithMany("MaterialCustomer")
                        .HasForeignKey("MaterialID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.MaterialFamily", b =>
                {
                    b.HasOne("SSMD.MaterialFamilyLevel", "L1")
                        .WithMany()
                        .HasForeignKey("L1ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.MaterialFamilyLevel", "L2")
                        .WithMany()
                        .HasForeignKey("L2ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.MaterialFamilyLevel", "L3")
                        .WithMany()
                        .HasForeignKey("L3ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.OrderComponent", b =>
                {
                    b.HasOne("SSMD.Component", "Component")
                        .WithMany("OrderComponents")
                        .HasForeignKey("ComponentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("OrderComponents")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.OrderConfirmation", b =>
                {
                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("OrderConfirmations")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.WorkCenter", "WorkCenter")
                        .WithMany()
                        .HasForeignKey("WorkCenterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.OrderData", b =>
                {
                    b.HasOne("SSMD.Material", "Material")
                        .WithMany("Orders")
                        .HasForeignKey("MaterialID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("OrderData")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.RoutingOperation", b =>
                {
                    b.HasOne("SSMD.OrderData", "OrderData")
                        .WithMany("RoutingOperations")
                        .HasForeignKey("RoutingNumber")
                        .HasPrincipalKey("RoutingNumber")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.WorkCenter", "WorkCenter")
                        .WithMany()
                        .HasForeignKey("WorkCenterID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SSMD.TestReport", b =>
                {
                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("TestReports")
                        .HasForeignKey("OrderNumber");
                });

            modelBuilder.Entity("SSMD.WBSRelation", b =>
                {
                    b.HasOne("SSMD.Project", "Down")
                        .WithMany("WBSUpRelations")
                        .HasForeignKey("DownID");

                    b.HasOne("SSMD.Project", "Project")
                        .WithMany("WBSRelations")
                        .HasForeignKey("ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Project", "Left")
                        .WithMany("WBSRightRelations")
                        .HasForeignKey("LeftID");

                    b.HasOne("SSMD.Project", "Right")
                        .WithMany("WBSLeftRelations")
                        .HasForeignKey("RightID");

                    b.HasOne("SSMD.Project", "Up")
                        .WithMany("WBSDownRelations")
                        .HasForeignKey("UpID");
                });

            modelBuilder.Entity("SSMD.WorkPhaseLabData", b =>
                {
                    b.HasOne("SSMD.Order", "Order")
                        .WithMany("WorkPhaseLabData")
                        .HasForeignKey("OrderNumber")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

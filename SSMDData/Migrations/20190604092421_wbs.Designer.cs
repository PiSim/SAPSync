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
    [Migration("20190604092421_wbs")]
    partial class wbs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
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

                    b.Property<int>("ControlPlan");

                    b.Property<int?>("MaterialFamilyID");

                    b.Property<int?>("ProjectID");

                    b.HasKey("ID");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.HasIndex("MaterialFamilyID");

                    b.HasIndex("ProjectID");

                    b.ToTable("Materials");
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

                    b.Property<int>("MaterialID");

                    b.Property<string>("OrderType");

                    b.Property<long>("RoutingNumber");

                    b.Property<double>("TotalQuantity");

                    b.Property<double>("TotalScrap");

                    b.HasKey("Number");

                    b.HasIndex("MaterialID");

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

                    b.Property<double>("PlannedQuantity");

                    b.Property<long>("RoutingNumber");

                    b.HasKey("OrderNumber");

                    b.ToTable("OrderData");
                });

            modelBuilder.Entity("SSMD.Project", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Description");

                    b.Property<int>("WBSLevel");

                    b.HasKey("ID");

                    b.HasIndex("Code");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("SSMD.RoutingOperation", b =>
                {
                    b.Property<long>("RoutingNumber");

                    b.Property<int>("RoutingCounter");

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

            modelBuilder.Entity("SSMD.WBSRelation", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("DownID");

                    b.Property<int?>("LeftID");

                    b.Property<int>("ProjectID");

                    b.Property<int?>("RightID");

                    b.Property<int?>("UpID");

                    b.HasKey("ID");

                    b.HasIndex("DownID");

                    b.HasIndex("LeftID");

                    b.HasIndex("ProjectID");

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
                    b.HasOne("SSMD.MaterialFamily", "MaterialFamily")
                        .WithMany("Materials")
                        .HasForeignKey("MaterialFamilyID");

                    b.HasOne("SSMD.Project", "Project")
                        .WithMany("Materials")
                        .HasForeignKey("ProjectID");
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

            modelBuilder.Entity("SSMD.Order", b =>
                {
                    b.HasOne("SSMD.Material", "Material")
                        .WithMany("Orders")
                        .HasForeignKey("MaterialID")
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
                    b.HasOne("SSMD.Order", "Order")
                        .WithMany()
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

            modelBuilder.Entity("SSMD.WBSRelation", b =>
                {
                    b.HasOne("SSMD.Project", "Down")
                        .WithMany()
                        .HasForeignKey("DownID");

                    b.HasOne("SSMD.Project", "Left")
                        .WithMany()
                        .HasForeignKey("LeftID");

                    b.HasOne("SSMD.Project", "Project")
                        .WithMany("WBSRelations")
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SSMD.Project", "Right")
                        .WithMany()
                        .HasForeignKey("RightID");

                    b.HasOne("SSMD.Project", "Up")
                        .WithMany()
                        .HasForeignKey("UpID");
                });
#pragma warning restore 612, 618
        }
    }
}

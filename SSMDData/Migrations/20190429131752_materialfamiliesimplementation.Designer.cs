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
    [Migration("20190429131752_materialfamiliesimplementation")]
    partial class materialfamiliesimplementation
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

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Components");
                });

            modelBuilder.Entity("SSMD.InspectionCharacteristic", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<double>("LowerSpecificationLimit");

                    b.Property<string>("Name");

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

                    b.Property<int?>("MaterialFamilyID");

                    b.HasKey("ID");

                    b.HasIndex("Code");

                    b.HasIndex("MaterialFamilyID");

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

                    b.Property<string>("Code");

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

                    b.Property<int>("ID");

                    b.Property<int>("MaterialID");

                    b.Property<string>("OrderType");

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

                    b.Property<int>("OrderNumber");

                    b.Property<double>("Scrap");

                    b.Property<string>("ScrapCause");

                    b.Property<DateTime>("StartTime");

                    b.Property<string>("UM");

                    b.Property<string>("WIPIn");

                    b.Property<string>("WIPOut");

                    b.Property<int>("WorkCenterID");

                    b.Property<double>("Yield");

                    b.HasKey("ConfirmationNumber", "ConfirmationCounter");

                    b.HasIndex("OrderNumber");

                    b.HasIndex("WorkCenterID");

                    b.ToTable("OrderConfirmations");
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
#pragma warning restore 612, 618
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using System;

namespace SSMD
{
    public class SSMDContext : DbContext
    {
        #region Constructors

        public SSMDContext(DbContextOptions<SSMDContext> options) : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<Component> Components { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<GoodMovement> GoodMovements { get; set; }
        public DbSet<InspectionCharacteristic> InspectionCharacteristics { get; set; }
        public DbSet<InspectionLot> InspectionLots { get; set; }
        public DbSet<InspectionPoint> InspectionPoints { get; set; }
        public DbSet<InspectionSpecification> InspectionSpecifications { get; set; }
        public DbSet<MaterialCustomer> MaterialCustomers { get; set; }
        public DbSet<MaterialFamily> MaterialFamilies { get; set; }
        public DbSet<MaterialFamilyLevel> MaterialFamilyLevels { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<OrderComponent> OrderComponents { get; set; }
        public DbSet<OrderConfirmation> OrderConfirmations { get; set; }
        public DbSet<OrderData> OrderData { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<RoutingOperation> RoutingOperations { get; set; }
        public DbSet<ScrapCause> ScrapCauses { get; set; }
        public DbSet<SyncElementData> SyncElementData { get; set; }
        public DbSet<TestReport> TestReports { get; set; }
        public DbSet<WBSRelation> WBSRelations { get; set; }
        public DbSet<WorkCenter> WorkCenters { get; set; }
        public DbSet<WorkPhaseLabData> WorkPhaseLabData { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .HasKey(comp => comp.ID);

            modelBuilder.Entity<Component>()
                .HasIndex(comp => comp.Name)
                .IsUnique();

            modelBuilder.Entity<InspectionCharacteristic>()
                .HasIndex(insc => insc.Name)
                .IsUnique();

            modelBuilder.Entity<GoodMovement>()
                .HasKey(gom => new Tuple<long, int>(gom.DocumentNumber, gom.ItemNumber));

            modelBuilder.Entity<InspectionPoint>()
                .HasKey(insp => new Tuple<long, int, int, int>(insp.InspectionLotNumber, insp.NodeNumber, insp.CharNumber, insp.SampleNumber));

            modelBuilder.Entity<InspectionPoint>()
                .HasOne(inspp => inspp.InspectionSpecification)
                .WithMany(inspp => inspp.InspectionPoints)
                .HasForeignKey(inspp => new Tuple<long, int, int>(inspp.InspectionLotNumber, inspp.NodeNumber, inspp.CharNumber));

            modelBuilder.Entity<InspectionSpecification>()
                .HasKey(inspsp => new Tuple<long, int, int>(inspsp.InspectionLotNumber, inspsp.NodeNumber, inspsp.CharacteristicNumber));

            modelBuilder.Entity<Material>()
                .HasKey(mat => mat.ID);

            modelBuilder.Entity<Material>()
                .HasIndex(mat => mat.Code)
                .IsUnique();

            modelBuilder.Entity<Material>()
                .HasOne(mat => mat.MaterialFamily)
                .WithMany(mfa => mfa.Materials)
                .HasForeignKey(mat => mat.MaterialFamilyID);

            modelBuilder.Entity<MaterialCustomer>()
                .HasKey(mac => new Tuple<int, int>(mac.MaterialID, mac.CustomerID));

            modelBuilder.Entity<MaterialCustomer>()
                .HasOne(mac => mac.Material)
                .WithMany(mat => mat.MaterialCustomer)
                .HasForeignKey(mac => mac.MaterialID);

            modelBuilder.Entity<MaterialCustomer>()
                .HasOne(mac => mac.Customer)
                .WithMany(cus => cus.MaterialCustomers)
                .HasForeignKey(mac => mac.CustomerID);

            modelBuilder.Entity<MaterialFamily>()
                .HasOne(matf => matf.L1)
                .WithMany()
                .HasForeignKey(matf => matf.L1ID);

            modelBuilder.Entity<MaterialFamily>()
                .HasOne(matf => matf.L2)
                .WithMany()
                .HasForeignKey(matf => matf.L2ID);

            modelBuilder.Entity<MaterialFamily>()
                .HasOne(matf => matf.L2)
                .WithMany()
                .HasForeignKey(matf => matf.L2ID);

            modelBuilder.Entity<MaterialFamilyLevel>()
                .HasIndex(matfl => new Tuple<int, string>(matfl.Level, matfl.Code))
                .IsUnique();

            modelBuilder.Entity<Order>()
                .HasKey(ord => ord.Number);

            modelBuilder.Entity<Order>()
                .HasIndex(ord => ord.OrderType);

            modelBuilder.Entity<OrderData>()
                .HasOne(ord => ord.Material)
                .WithMany(mat => mat.Orders)
                .HasForeignKey(ord => ord.MaterialID);

            modelBuilder.Entity<Order>()
                .HasMany(btc => btc.InspectionLots)
                .WithOne(ispl => ispl.Order)
                .HasForeignKey(ispl => ispl.OrderNumber);

            modelBuilder.Entity<OrderComponent>()
                .HasKey(orco => orco.ID);

            modelBuilder.Entity<OrderComponent>()
                .HasOne(orco => orco.Component)
                .WithMany(com => com.OrderComponents)
                .HasForeignKey(orco => orco.ComponentID);

            modelBuilder.Entity<OrderComponent>()
                .HasOne(orco => orco.Order)
                .WithMany(ord => ord.OrderComponents)
                .HasForeignKey(orco => orco.OrderNumber);

            modelBuilder.Entity<OrderConfirmation>()
                .HasKey(ordc => new { ordc.ConfirmationNumber, ordc.ConfirmationCounter });

            modelBuilder.Entity<OrderConfirmation>()
                .HasIndex(ordc => ordc.DeletionFlag);

            modelBuilder.Entity<OrderConfirmation>()
                .HasIndex(ordc => ordc.StartTime);

            modelBuilder.Entity<OrderConfirmation>()
                .HasOne(ordc => ordc.Order)
                .WithMany(ord => ord.OrderConfirmations)
                .HasForeignKey(ordc => ordc.OrderNumber);

            modelBuilder.Entity<Project>()
                .HasIndex(prj => prj.Code);

            modelBuilder.Entity<RoutingOperation>()
                .HasKey(rop => new Tuple<long, int>(rop.RoutingNumber, rop.RoutingCounter));

            modelBuilder.Entity<RoutingOperation>()
                .HasOne(rop => rop.OrderData)
                .WithMany(odd => odd.RoutingOperations)
                .HasForeignKey(rop => rop.RoutingNumber)
                .HasPrincipalKey(odd => odd.RoutingNumber);

            modelBuilder.Entity<TestReport>()
                .HasOne(trp => trp.Order)
                .WithMany(ord => ord.TestReports)
                .HasForeignKey(trp => trp.OrderNumber);

            modelBuilder.Entity<GoodMovement>()
                .HasOne(gmo => gmo.Component)
                .WithMany(comp => comp.GoodMovements);

            modelBuilder.Entity<GoodMovement>()
                .HasOne(gmo => gmo.Order)
                .WithMany(ord => ord.GoodMovements);

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Project)
                .WithMany(prj => prj.WBSRelations)
                .HasForeignKey(wbr => wbr.ID);

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Up)
                .WithMany(prj => prj.WBSDownRelations)
                .HasForeignKey(wbr => wbr.UpID);

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Left)
                .WithMany(prj => prj.WBSRightRelations)
                .HasForeignKey(wbr => wbr.LeftID);

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Down)
                .WithMany(prj => prj.WBSUpRelations)
                .HasForeignKey(wbr => wbr.DownID);

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Right)
                .WithMany(prj => prj.WBSLeftRelations)
                .HasForeignKey(wbr => wbr.RightID);

            modelBuilder.Entity<WorkCenter>()
                .HasIndex(wc => wc.ShortName);

            modelBuilder.Entity<WorkPhaseLabData>()
                .HasOne(wpld => wpld.Order)
                .WithMany(ord => ord.WorkPhaseLabData)
                .HasForeignKey(ord => ord.OrderNumber);

            modelBuilder.Entity<WorkPhaseLabData>()
                .HasKey(wpld => wpld.OrderNumber);
        }

        #endregion Methods
    }
}
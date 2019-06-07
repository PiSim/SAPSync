using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace SSMD
{
    public class SSMDContext : DbContext
    {
        #region Constructors

        public SSMDContext()
        {
        }

        #endregion Constructors

        #region Properties

        public DbSet<Component> Components { get; set; }
        public DbSet<InspectionCharacteristic> InspectionCharacteristics { get; set; }
        public DbSet<InspectionLot> InspectionLots { get; set; }
        public DbSet<InspectionPoint> InspectionPoints { get; set; }
        public DbSet<InspectionSpecification> InspectionSpecifications { get; set; }
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
        public DbSet<TestReport> TestReports { get; set; }
        public DbSet<WBSRelation> WBSRelations { get; set; }
        public DbSet<WorkCenter> WorkCenters { get; set; }
        public DbSet<WorkPhaseLabData> WorkPhaseLabData { get; set; }

        #endregion Properties

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["SSMD"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>()
                .HasKey(comp => comp.ID);

            modelBuilder.Entity<Component>()
                .HasIndex(comp => comp.Name);

            modelBuilder.Entity<InspectionCharacteristic>()
                .HasIndex(insc => insc.Name)
                .IsUnique();

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

            modelBuilder.Entity<Order>()
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

            modelBuilder.Entity<WBSRelation>()
                .HasOne(wbr => wbr.Project)
                .WithMany(prj => prj.WBSRelations)
                .HasForeignKey(wbr => wbr.ProjectID);

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
        }

        #endregion Methods
    }
}
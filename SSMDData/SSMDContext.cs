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

        public DbSet<Order> Orders { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<InspectionCharacteristic> InspectionCharacteristics { get; set; }
        public DbSet<InspectionLot> InspectionLots { get; set; }
        public DbSet<InspectionOperation> InspectionOperations { get; set; }
        public DbSet<InspectionPoint> InspectionPoints { get; set; }
        public DbSet<MaterialFamily> MaterialFamilies { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<OrderComponent> OrderComponents { get; set; }
        public DbSet<OrderConfirmation> OrderConfirmations { get; set; }
        public DbSet<ScrapCause> ScrapCauses { get; set; }

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

            modelBuilder.Entity<InspectionCharacteristic>()
                .HasKey(insc => insc.ID);
            
            modelBuilder.Entity<InspectionLot>()
                .HasKey(ispl => ispl.LotNumber);

            modelBuilder.Entity<InspectionOperation>()
                .HasKey(inspop => inspop.Number);

            modelBuilder.Entity<InspectionOperation>()
                .HasOne(inspop => inspop.InspectionLot)
                .WithMany(insplo => insplo.InspectionOperations)
                .HasForeignKey(inspop => inspop.InspectionLotNumber);

            modelBuilder.Entity<InspectionOperation>()
                .HasOne(inspop => inspop.InspectionCharacteristic)
                .WithMany(inspchar => inspchar.InspectionOperations)
                .HasForeignKey(inspop => inspop.InspectionCharacteristicID);

            modelBuilder.Entity<InspectionPoint>()
                .HasKey(inspp => inspp.Number);

            modelBuilder.Entity<InspectionPoint>()
                .HasOne(inspp => inspp.InspectionOperation)
                .WithMany(inspop => inspop.InspectionPoints)
                .HasForeignKey(inspp => inspp.InspectionOperationNumber);
            
            modelBuilder.Entity<Material>()
                .HasKey(mat => mat.ID);

            modelBuilder.Entity<Material>()
                .HasIndex(mat => mat.Code);

            modelBuilder.Entity<Material>()
                .HasOne(mat => mat.MaterialFamily)
                .WithMany(mfa => mfa.Materials)
                .HasForeignKey(mat => mat.MaterialFamilyID);

            modelBuilder.Entity<Order>()
                .HasKey(ord => ord.Number);

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
                .HasOne(ordc => ordc.Order)
                .WithMany(ord => ord.OrderConfirmations)
                .HasForeignKey(ordc => ordc.OrderNumber);

            modelBuilder.Entity<ScrapCause>()
                .HasKey(scc => scc.ID);
        }

        #endregion Methods
    }
}
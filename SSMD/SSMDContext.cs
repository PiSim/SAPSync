using DataAccessCore;
using Microsoft.EntityFrameworkCore;


namespace SSMD
{
    public class SSMDContext : DbContext
    {
        public SSMDContext()
        {

        }

        public DbSet<Order> Batches { get; set; }
        public DbSet<InspectionLot> InspectionLots { get; set; }
        public DbSet<OrderComponent> OrderComponents { get; set; }
        public DbSet<InspectionOperation> InspectionOperations { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<InspectionPoint> InspectionPoints {get;set;}
        public DbSet<Component> Components { get; set; }
        public DbSet<OrderConfirmation> OrderConfirmations { get; set; }
        public DbSet<MaterialFamily> MaterialFamilies { get; set; }
        public DbSet<ScrapCause> ScrapCauses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=192.168.1.22;user id=SSMDClient;Pwd=271828;persistsecurityinfo=True;database=ssmd;port=3306;SslMode=none");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InspectionLot>()
                .HasKey(ispl => ispl.LotNumber);


            modelBuilder.Entity<Order>()
                .HasMany(btc => btc.InspectionLots)
                .WithOne(ispl => ispl.Order)
                .HasForeignKey(ispl => ispl.OrderNumber);


        }
    }
}

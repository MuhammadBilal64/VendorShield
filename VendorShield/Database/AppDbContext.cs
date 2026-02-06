using Microsoft.EntityFrameworkCore;
using VendorShield.Model;

namespace VendorShield.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderLine> PurchaseOrderLines { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<ScoringConfig>ScoringConfigs { get; set; }
        public DbSet<AdminUser>AdminUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Vendor)
                .WithMany(v => v.PurchaseOrders)
                .HasForeignKey(po => po.VendorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.Vendor)
                .WithMany(v => v.Incidents)
                .HasForeignKey(i => i.VendorId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Incident>()
                .HasOne(i => i.PurchaseOrder)
                .WithMany() 
                .HasForeignKey(i => i.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchaseOrderLine>()
                .HasOne(pol => pol.PurchaseOrder)
                .WithMany() 
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Delivery>()
                .HasOne(d => d.PurchaseOrder)
                .WithMany() 
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

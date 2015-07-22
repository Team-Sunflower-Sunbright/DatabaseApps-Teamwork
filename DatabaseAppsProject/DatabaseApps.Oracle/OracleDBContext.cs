using System.Data.Entity;

namespace DatabaseApps.Oracle
{
    using Models;

    public class OracleDbContext : DbContext
    {
        public OracleDbContext()
            : base("OracleDbContext")
        {
            
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public DbSet<Vendor> Vendors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("MIHAYLOFF");
        }
    }
}
namespace DatabaseApps.MsSql
{
    using System.Data.Entity;
    using Migrations;
    using Models;

    public class MsSqlContext : DbContext
    {
        public MsSqlContext()
            : base("name=MsSqlContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MsSqlContext, Configuration>());
        }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Measure> Measures { get; set; }

        public IDbSet<Vendor> Vendors { get; set; }

        public IDbSet<Income> Sales { get; set; }

    }
}
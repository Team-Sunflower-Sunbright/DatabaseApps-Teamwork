namespace DatabaseApps.MySql
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using global::MySql.Data.Entity;
    using Initializations;
    using Migrations;
    using Models;

    public class MySQLContext : DbContext
    {
        // Your context has been configured to use a 'MySQLContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DatabaseApps.MySql.MySQLContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'MySQLContext' 
        // connection string in the application configuration file.
        public MySQLContext()
            : base("name=MySQLContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MySQLContext, Configuration>());
        }

        static MySQLContext()
        {
            DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            Database.SetInitializer(new DbInitializer());
        }

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }

    }
}
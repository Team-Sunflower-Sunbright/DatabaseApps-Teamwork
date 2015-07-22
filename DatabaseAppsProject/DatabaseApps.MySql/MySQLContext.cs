namespace DatabaseApps.MySql
{
    using System.Data.Entity;
    using System.Data.Entity.Migrations.History;
    using global::MySql.Data.Entity;
    using Initializations;
    using Models;

    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySQLContext : DbContext
    {
        public MySQLContext()
            : base("name=MySQLContext")
        {

            Database.SetInitializer(new DbInitializer());
        }

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Income> Sales { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<Measure> Measures { get; set; }
    }
}
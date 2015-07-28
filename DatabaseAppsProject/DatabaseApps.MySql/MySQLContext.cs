namespace DatabaseApps.MySql
{
    using System.Data.Entity;
    using global::MySql.Data.Entity;
    using Models;

    // [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class MySQLContext : DbContext
    {
        public MySQLContext()
            : base("name=MySQLContext")
        {

            // Database.SetInitializer(new DbInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Ignore(p => p.MeasureId)
                .Ignore(p => p.Measure);
            modelBuilder.Entity<Income>()
                .Ignore(i => i.SupermarketId)
                .Ignore(i => i.Supermarket);
            modelBuilder.Entity<Income>()
                .Ignore(i => i.Date);
            modelBuilder.Entity<Expense>()
                .Ignore(e => e.Date);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
    }
}
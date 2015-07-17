namespace DatabaseApps.MySql
{
    using System;
    using System.Data.Entity;
    using System.Linq;
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
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

         public virtual DbSet<Vendor> Vendors { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}
namespace DatabaseApps.MsSql
{
    using DatabaseApps.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class MsSqlContext : DbContext
    {
        public MsSqlContext()
            : base("name=MsSqlContext")
        {
        }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Measure> Measures { get; set; }

        public IDbSet<Vendor> Vendors { get; set; }

        public IDbSet<Income> Incomes { get; set; }

        public IDbSet<Expense> Expenses { get; set; }
    }
}
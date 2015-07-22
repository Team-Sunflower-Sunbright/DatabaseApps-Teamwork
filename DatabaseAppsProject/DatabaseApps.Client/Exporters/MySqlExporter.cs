namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;
    using Models;

    /// <summary>
    /// Exports data from MySql.
    /// </summary>
    public static class MySqlExporter
    {
        /// <summary>
        /// Exports all vendors and selected data - vendor name, all products (name and income)
        /// and total expenses per vendor.
        /// </summary>
        /// <returns>Collection containing vendors name, all his products, total expenses.</returns>
        public static IEnumerable<MySqlExportData> GetAllVendors()
        {
            var mysqlManager = new MysqlDBManager();
            // var mysqlVendors = mysqlManager.MySqlContext.Vendors.Count();
            var mysqlVendors = mysqlManager.MySqlContext.Vendors
                .Select(v => new
                {
                    VendorName = v.Name,
                    Products = v.Products.Select(p => new
                    {
                        ProductName = p.Name,
                        Income = p.Incomes.Select(s => s.Quantity * (double)p.Price).Sum()
                    }),
                    Expense = v.Expenses.Select(e => e.Amount)
                })
                .ToList();

            var vendorsData = new List<MySqlExportData>();

            mysqlVendors.ForEach(v =>
            {
                var products = v.Products.ToDictionary(product => product.ProductName, product => product.Income);

                var vendor = new MySqlExportData()
                {
                    Expenses = v.Expense.Sum(),
                    VendorName = v.VendorName,
                    Products = products
                };

                vendorsData.Add(vendor);
            });

            Console.WriteLine("MySQL data gathered.");

            return vendorsData;
        } 
    }
}

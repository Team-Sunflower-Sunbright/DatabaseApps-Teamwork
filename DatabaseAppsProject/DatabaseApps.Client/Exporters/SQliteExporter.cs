namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;

    /// <summary>
    /// Exports data from SQLite.
    /// </summary>
    public static class SQliteExporter
    {
        /// <summary>
        /// Exports all product's data - product name and product tax.
        /// </summary>
        /// <returns>All products and their tax.</returns>
        public static IDictionary<string, double?> GetAllProducts()
        {
            var sqliteManager = new SQLiteDBManager();
            var context = sqliteManager.SQLiteContext;
            var products = context.Taxes
                .Select(p => new
                {
                    p.ProductName,
                    p.TaxRate
                })
                .ToDictionary(p => p.ProductName, p => p.TaxRate);

            Console.WriteLine("SQLite data gathered.");

            return products;
        }
    }
}

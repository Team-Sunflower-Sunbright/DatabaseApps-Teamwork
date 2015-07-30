namespace DatabaseApps.Client.Importers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DbManagers;
    using Models;

    /// <summary>
    /// Imports data to MySql.
    /// </summary>
    public static class MySqlImporter
    {
        /// <summary>
        /// Imports Products to MySql.
        /// </summary>
        public static void ImportToMySql(IEnumerable<Product> products)
        {
            var mysqlManager = new MysqlDBManager();
            var mySqlContext = mysqlManager.MySqlContext;

            foreach (var product in products)
            {
                var vendorName = product.Vendor.Name;
                var vendor = mySqlContext.Vendors.FirstOrDefault(v => v.Name == vendorName);
                if (vendor == null)
                {
                    vendor = new Vendor()
                    {
                        Name = vendorName,
                        Expenses = new List<Expense>()
                    };
                }

                var mySqlProduct = DeepCopyProduct(product, vendor);
                mySqlContext.Products.AddOrUpdate(mySqlProduct);
                mySqlContext.SaveChanges();
            }

            Console.WriteLine("Data imported to MySQL!");
        }

        /// <summary>
        /// Makes deep copy of Product.
        /// Entity Framework cannot keep track of 1 item in 2 contexts, so in order to
        /// export something and import it in other database it needs deep copy of it.
        /// </summary>
        /// <param name="product">Product for copy.</param>
        /// <param name="vendor">Product's vendor.</param>
        /// <returns>Deep copy of the product</returns>
        private static Product DeepCopyProduct(Product product, Vendor vendor)
        {
            var incomes = product.Incomes
                .Select(income => new Income()
                {
                    Quantity = income.Quantity,
                    SalePrice = income.SalePrice
                })
                .ToList();

            var expenses = product.Vendor.Expenses
                .Select(expense => new Expense()
                {
                    Amount = expense.Amount,
                })
                .ToList();

            foreach (var expense in expenses)
            {
                vendor.Expenses.Add(expense);
            }

            var deepCopyProduct = new Product()
            {
                Name = product.Name,
                BuyingPrice = product.BuyingPrice,
                Incomes = incomes,
                Vendor = vendor
            };

            return deepCopyProduct;
        }
    }
}

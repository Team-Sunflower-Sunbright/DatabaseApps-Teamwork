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
                var mySqlProduct = DeepCopyProduct(product);
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
        /// <param name="product">Product for copy</param>
        /// <returns>Deep copy of the product</returns>
        private static Product DeepCopyProduct(Product product)
        {
            var incomes = product.Incomes
                .Select(income => new Income()
                {
                    Date = income.Date,
                    Quantity = income.Quantity
                })
                .ToList();

            var expenses = product.Vendor.Expenses
                .Select(expense => new Expense()
                {
                    Date = expense.Date,
                    Amount = expense.Amount,
                })
                .ToList();

            var deepCopyProduct = new Product()
            {
                Name = product.Name,
                Price = product.Price,
                Incomes = incomes,
                Measure = new Measure() { Name = product.Measure.Name },
                Vendor = new Vendor()
                {
                    Name = product.Vendor.Name,
                    Expenses = expenses
                }
            };

            return deepCopyProduct;
        }
    }
}

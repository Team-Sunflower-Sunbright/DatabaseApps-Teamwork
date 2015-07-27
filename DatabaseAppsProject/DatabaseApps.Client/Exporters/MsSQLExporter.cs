namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DbManagers;
    using Models;
    using MsSql;

    /// <summary>
    /// Exports data from MsSQL server.
    /// </summary>
    public static class MsSQLExporter
    {
        /// <summary>
        /// Export all products. Including their Incomes, Measures, Vendor and Vendor's Expenses.
        /// </summary>
        /// <returns>All products and their data.</returns>
        public static IEnumerable<Product> ExportProducts()
        {
            var msssqlManager = new MsSqlDBManager();
            var msSqlContext = msssqlManager.MsSqlContext;

            // When the other parts of the project are placed comment the next lines.
            if (!msSqlContext.Products.Any())
            {
                SeedMsSql(msSqlContext);
            }

            var products = msSqlContext.Products
                .Include(p => p.Incomes)
                .Include(p => p.Measure)
                .Include(p => p.Vendor)
                .Include(p => p.Vendor.Expenses)
                .ToList();
            Console.WriteLine("MS SQL Server data gathered.");

            return products;
        }

        /// <summary>
        /// Seed simple data to MsSql in case it is empty. For test purposes.
        /// </summary>
        /// <param name="context">MsSQL context</param>
        private static void SeedMsSql(MsSqlContext context)
        {
            var product = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some"
                },
                Name = "Chocolate “Milka”",
                BuyingPrice = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10,
                        Date = DateTime.Now
                    },
                    new Income()
                    {
                        Quantity = 10,
                        Date = DateTime.Now
                    }
                },
                Vendor = new Vendor()
                {
                    Name = "Nestle Sofia Corp.",
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Amount = 15m,
                            Date = DateTime.Now
                        },
                        new Expense()
                        {
                            Amount = 15m,
                            Date = DateTime.Now
                        }
                    }
                }
            };

            context.Products.AddOrUpdate(product);

            var product2 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some2"
                },
                Name = "Beer “Zagorka”",
                BuyingPrice = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10,
                        Date = DateTime.Now
                    }
                },
                Vendor = new Vendor()
                {
                    Name = "Zagorka Corp.",
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Amount = 120m,
                            Date = DateTime.Now
                        }
                    }
                }
            };

            context.Products.AddOrUpdate(product2);

            var product3 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some3"
                },
                Name = "Vodka “Targovishte”",
                BuyingPrice = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10,
                        Date = DateTime.Now
                    }
                },
                Vendor = new Vendor()
                {
                    Name = "Targovishte Bottling Company Ltd.",
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Amount = 200m,
                            Date = DateTime.Now
                        }
                    }
                }
            };

            context.Products.AddOrUpdate(product3);

            context.SaveChanges();

            Console.WriteLine("MS SQL Server seeded with sample data.");
        }
    }
}

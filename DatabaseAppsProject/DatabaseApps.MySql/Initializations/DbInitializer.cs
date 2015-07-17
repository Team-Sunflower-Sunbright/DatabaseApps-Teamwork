namespace DatabaseApps.MySql.Initializations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models;

    public class DbInitializer : CreateDatabaseIfNotExists<MySQLContext>
    {
        protected override void Seed(MySQLContext context)
        {
            var product = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some"
                },
                Name = "Beer “Beck’s”",
                Price = 10m,
                Sales = new List<Sale>()
                {
                    new Sale()
                    {
                        TotalIncome = 135.70m,
                        TotalSold = 10
                    }
                },
                Vendor = new Vendor()
                {
                    Name = "Nestle Sofia Corp.",
                    Expenses = new List<Expense>()
                    {
                        new Expense()
                        {
                            Amount = 30m,
                            Period = DateTime.Now
                        }
                    }
                }
            };

            context.Products.Add(product);

            var product2 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some2"
                },
                Name = "Beer “Zagorka”",
                Price = 10m,
                Sales = new List<Sale>()
                {
                    new Sale()
                    {
                        TotalIncome = 872.19m,
                        TotalSold = 10
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
                            Period = DateTime.Now
                        }
                    }
                }
            };

            context.Products.Add(product2);

            var product3 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some3"
                },
                Name = "Vodka “Targovishte”",
                Price = 10m,
                Sales = new List<Sale>()
                {
                    new Sale()
                    {
                        TotalIncome = 1155.90m,
                        TotalSold = 10
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
                            Period = DateTime.Now
                        }
                    }
                }
            };

            context.Products.Add(product3);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}

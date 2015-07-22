namespace DatabaseApps.MySql.Initializations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
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
                Name = "Chocolate “Milka”",
                Price = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10
                    },
                    new Income()
                    {
                        Quantity = 10
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

            context.Products.Add(product);

            var product2 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some2"
                },
                Name = "Beer “Zagorka”",
                Price = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10
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

            context.Products.Add(product2);

            var product3 = new Product()
            {
                Measure = new Measure()
                {
                    Name = "Some3"
                },
                Name = "Vodka “Targovishte”",
                Price = 10m,
                Incomes = new List<Income>()
                {
                    new Income()
                    {
                        Quantity = 10
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

            base.Seed(context);
        }
    }
}

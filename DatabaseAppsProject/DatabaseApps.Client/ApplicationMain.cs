namespace DatabaseApps.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;
    using Exporters;
    using Models;

    public class ApplicationMain
    {
        static void Main()
        {
            // Oracle Management Things
            // OracleDBManager oracleManager = new OracleDBManager();
            // oracleManager.ImportMeasuresFromCSVFile("SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("SeedFiles/Products.txt");

            ExportToExcel();
        }

        /// <summary>
        /// Problem 7
        /// </summary>
        private static void SeedDataToMySQL()
        {
            // Insert MSSQL context and select data

            var mysqlManager = new MysqlDBManager();

            // Feed the data
            // mysqlManager.MySqlContext.Products.Add();
        }

        /// <summary>
        /// Problem 8
        /// </summary>
        private static void ExportToExcel()
        {
            // SQLite
            var sqliteManager = new SQLiteDBManager();
            var sqliteProducts = sqliteManager.GetAllProducts();
            // foreach (var product in sqliteProducts)
            // {
            //     Console.WriteLine("{0} {1}%", product.Key, product.Value);
            // }

            // MySQL
            var mysqlManager = new MysqlDBManager();
            // var mysqlVendors = mysqlManager.MySqlContext.Vendors.Count();
            var mysqlVendors = mysqlManager.MySqlContext.Vendors
                .Select(v => new
                {
                    VendorName = v.Name,
                    Products = v.Products.Select(p => new
                    {
                        ProductName = p.Name,
                        Income = p.Sales.Select(s => s.TotalIncome).Sum()
                    }),
                    Expense = v.Expenses.Select(e => e.Amount)
                })
                .ToList();

            var excelReportDatas = new List<ExcelReportData>();

            mysqlVendors.ForEach(v =>
            {
                var totalTaxes = 0m;
                var totalIncome = 0m;
                v.Products.ToList().ForEach(p =>
                {
                    var tax = 0m;
                    if (sqliteProducts.ContainsKey(p.ProductName))
                    {
                        tax = (decimal)sqliteProducts[p.ProductName];
                    }

                    totalTaxes += p.Income * tax / 100;
                    totalIncome += p.Income;
                });

                var financialResult = totalIncome - totalTaxes - v.Expense.Sum();

                var data = new ExcelReportData()
                {
                    FinancialResult = financialResult,
                    TotalExpense = v.Expense.Sum(),
                    TotalIncome = totalIncome,
                    TotalTaxes = totalTaxes,
                    VendorName = v.VendorName
                };

                excelReportDatas.Add(data);
            });

            // excelReportDatas.ForEach(Console.WriteLine);

            ExcelExporter.ExportToExcel(excelReportDatas);
        }
    }
}

namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;
    using Models;

    public static class SQLiteAndMySQLExporter
    {
        public static List<ExcelReportData> GetReportDate()
        {
            // SQLite
            var sqliteManager = new SQLiteDBManager();
            var sqliteProducts = sqliteManager.GetAllProducts();
            // System.Console.WriteLine(sqliteProducts.Count());
            Console.WriteLine("SQLite data gathered.");

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
            Console.WriteLine("MySQL data gathered.");

            var excelReportData = new List<ExcelReportData>();

            // Aggregation of data
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

                excelReportData.Add(data);
            });

            // excelReportDatas.ForEach(Console.WriteLine);
            Console.WriteLine("Excel report issued.");

            return excelReportData;
        } 
    }
}

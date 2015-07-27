using System;

namespace DatabaseApps.Client
{
    using DatabaseApps.MsSql;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;
    using Exporters;
    using Importers;
    using Models;
    using MySql;
    using DatabaseApps.Client.Exporters.PDF;
    using DatabaseApps.Client.Importers.XML;

    public class ApplicationMain
    {
        static void Main()
        {
            // #1 - Oracle
            // OracleDBManager oracleManager = new OracleDBManager ();
            // oracleManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");

            // SQL Server Manager - Use this to import data to SQL Server
            SQLServerDBManager sqlManager = new SQLServerDBManager();

            // Sample Data
            // sqlManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            // sqlManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            // sqlManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");
            
            // #2 - Export Data from Oracle to MS SQL Server
            // oracleManager.ExportDataToMSSQLContext(sqlManager.SqlServerContext);

            
            // #5 - Anton : Test of the JsonExport and import into Mongo
            // var startDate = new DateTime();
            // var endDate = new DateTime();

            // JsonExporter.ExportSalesReportsToJson(sqlManager.SqlServerContext, startDate, endDate);
            // MongoImporter.ImportSalesReportsIntoDatabase();

            // #4 - Sales Report
            // CreateSaleReport("../../Output-Files/sales report.pdf");

            // #6 - XML Import
            XMLImporter.ImportExpensesByMonth(sqlManager.SqlServerContext, "../../Output-Files/Sample-Vendor-Expenses.xml");

            // #7 - Export Data from MS SQL to MySQL
            // SeedDataToMySql();

            // #8 - Export Data to Excel
            // ExportToExcel();
        }

        /// <summary>
        /// Problem 7
        /// </summary>
        private static void SeedDataToMySql()
        {
            var products = MsSQLExporter.ExportProducts();
            MySqlImporter.ImportToMySql(products);
        }

        /// <summary>
        /// Problem 8
        /// </summary>
        private static void ExportToExcel()
        {
            var mySqlData = MySqlExporter.GetAllVendors();
            var sqliteData = SQliteExporter.GetAllProducts();
            ExcelExporter.ExportToExcel(mySqlData, sqliteData);
        }

        private static void CreateSaleReport(string path)
        {
            var context = new MsSqlContext();
            IQueryable<IGrouping<GroupingByDate, Income>> sales = context.Incomes
               .GroupBy(p => new GroupingByDate()
               {
                   Day = p.Date.Day,
                   Month = p.Date.Month,
                   Year = p.Date.Year
               });

            PDFSalesReport.CreateReport(path, sales);
        }
    }
}

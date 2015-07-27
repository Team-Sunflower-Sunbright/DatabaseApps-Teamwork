using System;
using FromZipToSql;

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
            // Oracle Management things
            // OracleDBManager oracleManager = new OracleDBManager ();
            // oracleManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");

            // SQL Server Manager - Use this to import data to SQL Server
            /*SQLServerDBManager sqlManager = new SQLServerDBManager();
            sqlManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            sqlManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            sqlManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");*/

            ZipImporter.Import();
            
            // Export Data from Oracle to MS SQL Server
            // oracleManager.ExportDataToMSSQLContext(sqlManager.SqlServerContext);

            // Export Data from MS SQL to MySQL
            //SeedDataToMySql();
            
            // Export Data to Excel
            //ExportToExcel();
            
            //Anton : Test of the JsonExport and import into Mongo
            //var dbContext = new MsSqlContext();
            //var startDate = new DateTime();
            //var endDate = new DateTime();

            //JsonExporter.ExportSalesReportsToJson(dbContext, startDate, endDate);
            //MongoImporter.ImportSalesReportsIntoDatabase();

            //CreateSaleReport("sales report.pdf");
            //var mssqlContext = new MsSqlContext();
            //XMLImporter.ImportExpensesByMonth(mssqlContext, "Sample-Vendor-Expenses.xml");
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

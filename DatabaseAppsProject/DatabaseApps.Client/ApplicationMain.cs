using System;
using System.Text.RegularExpressions;
using FromZipToSql;

namespace DatabaseApps.Client
{
    using System.Linq;
    using DbManagers;
    using Exporters;
    using Exporters.PDF;
    using Importers;
    using Models;
    using MsSql;

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

            // SQLServerDBManager sqlManager = new SQLServerDBManager();

            // Sample Data
            // sqlManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            // sqlManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            // sqlManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");


            // #2 - Export Data from Oracle to MS SQL Server
            // oracleManager.ExportDataToMSSQLContext(sqlManager.SqlServerContext);

            // #2 - ZIP Import
            //ZipImporter.Import();

            // #3 - Sales Report
            // CreateSaleReport("../../Output-Files/sales report.pdf");

            // #4 - XML Export
            //var startDate = new DateTime();
            //var endDate = new DateTime();
            //XMLExporter.ExportToXML(startDate, endDate);

            // #5 - Anton : Test of the JsonExport and import into Mongo
            // var startDate = new DateTime(2014, 07, 20);
            // var endDate = new DateTime(2014, 07, 21);

            // JsonExporter.ExportSalesReportsToJson(sqlManager.SqlServerContext, startDate, endDate);
            // MongoImporter.ImportSalesReportsIntoDatabase();


            // #6 - XML Import
            // XMLImporter.ImportExpensesByMonth(sqlManager.SqlServerContext, "../../Output-Files/Sample-Vendor-Expenses.xml");

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

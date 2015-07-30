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
    using Importers.XML;
    using Models;
    using MsSql;
    using MySql;
    using Org.BouncyCastle.Cms;

    public class ApplicationMain
    {
        static void Main()
        {
            ProcessInputCommands();

            return;
            // MySQLContext context = new MySQLContext();
            // context.Expenses.Select(a => a).ToList();
            // return;

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

            // #2 - ZIP Import
            // ZipImporter.Import();

            // #3 - Sales Report
            // CreateSaleReport("../../Output-Files/sales report.pdf");

            // #4 - XML Export
            // var startDate = new DateTime(2010, 07, 20);
            // var endDate = new DateTime(2014, 07, 21);
            // XMLExporter.ExportToXML(startDate, endDate);

            // #5 - Anton : Test of the JsonExport and import into Mongo
            // var startDate = new DateTime(2010, 07, 20);
            // var endDate = new DateTime(2014, 07, 21);
            
            // JsonExporter.ExportSalesReportsToJson(sqlManager.SqlServerContext, startDate, endDate);
            // MongoImporter.ImportSalesReportsIntoDatabase();


            // #6 - XML Import
            // XMLImporter.ImportExpensesByMonth(sqlManager.SqlServerContext, "../../Output-Files/Sample-Vendor-Expenses.xml");

            // #7 - Export Data from MS SQL to MySQL
            SeedDataToMySql();

            // #8 - Export Data to Excel
            ExportToExcel();
        }

        private static void ProcessInputCommands()
        {
            Console.Write("> ");
            string command = Console.ReadLine();
            OracleDBManager oracleManager = new OracleDBManager();
            SQLServerDBManager sqlManager = new SQLServerDBManager();

            while (command != "End")
            {
                Console.Write("> ");

                switch (command)
                {
                    case "oracle import":
                        Console.WriteLine("Importing data to oracle...");

                        oracleManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
                        oracleManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
                        oracleManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");

                        Console.WriteLine("Data has been imported");
                        break;
                    case "oracle export":
                        Console.WriteLine("Exporting data from Oracle to MS SQL Server...");

                        oracleManager.ExportDataToMSSQLContext(sqlManager.SqlServerContext);

                        Console.WriteLine("Data exported successfully");
                        break;
                    case "zip import":
                        Console.WriteLine("Importing data from ZIP");

                        ZipImporter.Import();

                        Console.WriteLine("Data imported successfully");
                        break;
                    case "pdf export":
                        Console.Write("Input the start date: ");
                        DateTime startDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Input the end date: ");
                        DateTime endDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Creating the PDF Sales Report.");

                        CreateSaleReport("../../Output-Files/sales report.pdf", startDate, endDate);

                        Console.WriteLine("Sales report generated successfully.");
                        break;
                    case "xml export":
                        Console.Write("Input the start date: ");
                        startDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Input the end date: ");
                        endDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Generating XML Report");

                        XMLExporter.ExportToXML(startDate, endDate);

                        Console.WriteLine("Report generated successfully");
                        break;
                    case "xml import":
                        Console.WriteLine("Importing Expenses Data from XML.");

                        XMLImporter.ImportExpensesByMonth(sqlManager.SqlServerContext, "../../Output-Files/Sample-Vendor-Expenses.xml");

                        Console.WriteLine("Data imported successfully.");
                        break;
                    case "json export":
                        Console.Write("Input the start date: ");
                        startDate = DateTime.Parse(Console.ReadLine());

                        Console.Write("Input the end date: ");
                        endDate = DateTime.Parse(Console.ReadLine());

                        Console.WriteLine("Generating sales report to JSON");

                        JsonExporter.ExportSalesReportsToJson(sqlManager.SqlServerContext, startDate, endDate);

                        Console.WriteLine("Sales report generated successfully.");
                        break;
                    case "mongodb import":
                        Console.WriteLine("Importing data to MongoDB");

                        MongoImporter.ImportSalesReportsIntoDatabase();

                        Console.WriteLine("Data imported successfully");
                        break;
                    case "mysql import":

                        Console.WriteLine("Exporting data from MS SQL to MySQL");

                        SeedDataToMySql();

                        Console.WriteLine("Data exported successfully.");
                        break;
                    case "excel export":

                        Console.WriteLine("Exporting financial result report to excel.");

                        ExportToExcel();

                        Console.WriteLine("financial result exported successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }

                command = Console.ReadLine();
            }
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

        private static void CreateSaleReport(string path, DateTime start, DateTime end)
        {
            var context = new MsSqlContext();
            IQueryable<IGrouping<GroupingByDate, Income>> sales = context.Incomes
                // Where
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

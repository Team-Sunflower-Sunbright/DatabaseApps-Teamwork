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

    public class ApplicationMain
    {
        static void Main()
        {
            bool isRunning = true;

            do
            {
                try
                {
                    ProcessInputCommands();
                    isRunning = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            } while (isRunning);
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
                    case "help":
                        PrintHelp();
                        break;
                    default:
                        Console.WriteLine("Invalid command");
                        break;
                }


                Console.Write("> ");
                command = Console.ReadLine();
            }
        }

        private static void PrintHelp()
        {
            Console.WriteLine(new string('-', 30) + " HELP " + new string('-', 30));

            Console.WriteLine("oracle import - Imports the sample data into the Oracle DB from text files.");
            Console.WriteLine("oracle export - Exports the data from the oracle DB to the SQL Server DB");
            Console.WriteLine("zip import - Imports Incomes data from zip files into SQL Server DB");
            Console.WriteLine("pdf export - Generates PDF sales report for the given period");
            Console.WriteLine("xml export - Generates XML sales report for the given period");
            Console.WriteLine("json export - Generates JSON sales reports for the given period");
            Console.WriteLine("mongo import - Imports the JSON Sales reports into the MongoDB Database");
            Console.WriteLine("xml import - Imports Expenses reports from XML files in the SQL Server DB");
            Console.WriteLine("mysql import - exports the Data from SQL Server DB to the MySQL DB");
            Console.WriteLine("excel report - Generates an excel report with the financial result for the different vendors. It takes data from the MySQL DB and SQLite");

            Console.WriteLine(new string('-', 30) + " HELP " + new string('-', 30));
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
               .Where(i => i.Date >= start && i.Date <= end)
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

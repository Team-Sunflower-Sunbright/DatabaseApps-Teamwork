using System;

namespace DatabaseApps.Client
{
    using DatabaseApps.MsSql;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using DbManagers;
    using Exporters;
    using Models;
    using MySql;

    public class ApplicationMain
    {
        static void Main()
        {
            // Oracle Management things
            // OracleDBManager oracleManager = new OracleDBManager ();
            // oracleManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");

            Database.SetInitializer(new DropCreateDatabaseAlways<MySQLContext>());

            // SQL Server Manager - Use this to import data to SQL Server
            // SQLServerDBManager sqlManager = new SQLServerDBManager();
            //sqlManager.ImportMeasuresFromCSVFile("../../Output-Files/SeedFiles/Measures.txt");
            //sqlManager.ImportVendorsFromCSVFile("../../Output-Files/SeedFiles/Vendors.txt");
            //sqlManager.ImportProductsFromCSVFile("../../Output-Files/SeedFiles/Products.txt");
            
            // Export Data from Oracle to MS SQL Server
            // oracleManager.ExportDataToMSSQLContext(sqlManager.SqlServerContext);

            // MySQLContext mysqlContext = new MySQLContext();
            // sqlManager.ExportDataToMySQLContext(mysqlContext);
            
            // Export Data to Excel
            // ExportToExcel();

            //Create MSSQL Database
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
            var excelReportData = SQLiteAndMySQLExporter.GetReportDate();
            ExcelExporter.ExportToExcel(excelReportData);
        }
    }
}

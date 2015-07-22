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

    public class ApplicationMain
    {
        static void Main()
        {

            // Oracle Management Things
            // OracleDBManager oracleManager = new OracleDBManager();
            // oracleManager.ImportMeasuresFromCSVFile("SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("SeedFiles/Products.txt");

            //SeedDataToMySql();

            //ExportToExcel();
            //var mysqlManager = new MysqlDBManager();
            //var mysqlVendors = mysqlManager.MySqlContext.Vendors.Count();
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
    }
}

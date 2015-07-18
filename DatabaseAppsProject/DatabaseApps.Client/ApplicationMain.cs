namespace DatabaseApps.Client
{
    using DbManagers;
    using Exporters;

    public class ApplicationMain
    {
        static void Main()
        {
            // Oracle Management Things
            // OracleDBManager oracleManager = new OracleDBManager();
            // oracleManager.ImportMeasuresFromCSVFile("SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("SeedFiles/Products.txt");

            // ExportToExcel();
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

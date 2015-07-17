namespace DatabaseApps.Client
{
    public class ApplicationMain
    {
        static void Main()
        {
            // Oracle Management Things
            // OracleDBManager oracleManager = new OracleDBManager();
            // oracleManager.ImportMeasuresFromCSVFile("SeedFiles/Measures.txt");
            // oracleManager.ImportVendorsFromCSVFile("SeedFiles/Vendors.txt");
            // oracleManager.ImportProductsFromCSVFile("SeedFiles/Products.txt");

            // SQLite
            ////var sqliteManager = new SQLiteDBManager();
            ////var products = sqliteManager.GetAllProducts();
            ////foreach (var product in products)
            ////{
            ////    Console.WriteLine("{0} {1}%", product.Key, product.Value);
            ////}

            // MySQL
            ////var mysqlManager = new MysqlDBManager();
            ////var testConnection = mysqlManager.MySqlContext.Vendors.Count();
            ////Console.WriteLine(testConnection);
        }
    }
}

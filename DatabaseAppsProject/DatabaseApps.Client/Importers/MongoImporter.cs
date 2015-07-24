namespace DatabaseApps.Client.Importers
{
    using MongoDB;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    public static class MongoImporter
    {
        public static void ImportSalesReportsIntoDatabase()
        {
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");
            string JsonDirectory = "../../Json-Reports/" + currentDate;

            var reports = GetProductReportFiles(JsonDirectory);

            var db = new MongoConnection("SalesDB");
            db.CreateCollection<ProductReport>("SalesByProductReports");

            foreach (var report in reports)
            {
                //System.Console.WriteLine(report);
                db.Collections["SalesByProductReports"].Insert(report);
            }
        }

        private static IEnumerable<ProductReport> GetProductReportFiles(string directory)
        {
            var products = new Collection<ProductReport>();

            var reportFiles = Directory.EnumerateFiles(directory);

            foreach (var file in reportFiles)
            {
                var currentReportFile = ReadFileAsJson(file);
                var currReport = new ProductReport(currentReportFile);
                products.Add(currReport);
            }

            return products;
        }

        private static string ReadFileAsJson(string file)
        {
            string readFileToJson;

            using (var stream = new FileStream(file, FileMode.Open))
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    readFileToJson = reader.ReadToEnd();
                }
            }

            return readFileToJson;
        }
    }
}
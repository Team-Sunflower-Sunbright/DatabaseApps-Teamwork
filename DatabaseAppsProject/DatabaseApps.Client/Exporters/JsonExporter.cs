namespace DatabaseApps.Client.Exporters
{
    using DatabaseApps.MsSql;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    public static class JsonExporter
    {
        private const string JsonFileName = "{0}.json";

        public static void ExportSalesReportsToJson(MsSqlContext dbContext, DateTime startDate, DateTime endDate)
        {
            string dateOfExecution = DateTime.Now.ToString("yyyy-MM-dd");
            string directoryName = "../../Json-Reports/" + dateOfExecution + Path.DirectorySeparatorChar;

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            var productSales = dbContext.Products
                .Select(p => new
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    VendorName = p.Vendor.Name,
                    TotalQuantitySold = p.Incomes.ToList().Sum(i => i.Quantity) == null ? 0 :
                        p.Incomes.ToList().Sum(i => i.Quantity),
                    TotalIncomes = p.Incomes.ToList().Sum(i => i.Quantity * (double)i.SalePrice) == null ? 0 :
                        p.Incomes.ToList().Sum(i => i.Quantity * (double)i.SalePrice)
                })
                .ToList();

            foreach (var saleReport in productSales)
            {
                var serializedObject = JsonConvert.SerializeObject(saleReport, Formatting.Indented);
                var reportFilename = string.Format("{0}{1}", saleReport.Id, ".json");

                using (var writer = new StreamWriter(Path.Combine(directoryName, reportFilename)))
                {
                    writer.WriteLine(serializedObject);
                }
            }
        }
    }
}
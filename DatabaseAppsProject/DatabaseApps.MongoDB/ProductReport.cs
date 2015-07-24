namespace MongoDB
{
    using MongoDB.Bson;
    using Newtonsoft.Json.Linq;
    public class ProductReport
    {
        public ProductReport(string json)
        {
            JObject jObject = JObject.Parse(json);
            productName = (string)jObject["ProductName"];
            vendorName = (string)jObject["VendorName"];
            totalQuantitySold = (double)jObject["TotalQuantitySold"];
            totalIncomes = (double)jObject["TotalIncomes"];
        }

        public ObjectId id;

        public string productName;

        public string vendorName;

        public double totalQuantitySold;

        public double totalIncomes;
    }
}
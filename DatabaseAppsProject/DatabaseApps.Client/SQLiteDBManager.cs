namespace DatabaseApps.Client
{
    using System.Collections.Generic;
    using System.Linq;
    using DatabasseApps.SQLite;

    public class SQLiteDBManager
    {
        private readonly SQLiteContext context;

        public SQLiteDBManager()
        {
            this.context = new SQLiteContext();
        }

        public SQLiteContext SQLiteContext
        {
            get
            {
                return this.context;
            }
        }

        public Dictionary<string, double?> GetAllProducts()
        {
            var products = this.context.Taxes
                .Select(p => new
                {
                    p.ProductName,
                    p.TaxRate
                })
                .ToDictionary(p => p.ProductName, p => p.TaxRate);

            return products;
        }
    }
}

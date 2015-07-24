namespace DatabaseApps.Client.DbManagers
{
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using Models;
    using MsSql;
    using MySql;

    public class SQLServerDBManager
    {
        private MsSqlContext sqlServerContext;

        public SQLServerDBManager()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<MsSqlContext>());
            this.sqlServerContext = new MsSqlContext();
        }

        public MsSqlContext SqlServerContext
        {
            get
            {
                return this.sqlServerContext;
            }
        }

        public void ImportProductsFromCSVFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);

            while (!reader.EndOfStream)
            {
                string[] currentElements = reader.ReadLine().Split(',');
                int vendorId = int.Parse(currentElements[0]);
                string productName = currentElements[1];
                int measureId = int.Parse(currentElements[2]);
                decimal price = decimal.Parse(currentElements[3]);

                Product product = new Product();
                product.Name = productName;
                product.VendorId = vendorId;
                product.MeasureId = measureId;
                product.Price = price;


                this.sqlServerContext.Products.Add(product);
                this.sqlServerContext.SaveChanges();
            }

            reader.Close();
        }

        public void ImportVendorsFromCSVFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);

            while (!reader.EndOfStream)
            {
                string[] currentElements = reader.ReadLine().Split(',');
                string vendorName = currentElements[0];

                Vendor vendor = new Vendor();
                vendor.Name = vendorName;

                this.sqlServerContext.Vendors.Add(vendor);
                this.sqlServerContext.SaveChanges();
            }

            reader.Close();
        }

        public void ImportMeasuresFromCSVFile(string filename)
        {
            StreamReader reader = new StreamReader(filename);
         
            while (!reader.EndOfStream)
            {
                string[] currentElements = reader.ReadLine().Split(',');
                string measureName = currentElements[0];

                Measure measure = new Measure();
                measure.Name = measureName;

                this.sqlServerContext.Measures.Add(measure);
                this.sqlServerContext.SaveChanges();
            }

            reader.Close();
        }

        public void ExportDataToMySQLContext(MySQLContext context)
        {
            var measures = this.sqlServerContext.Measures.ToList();
            var products = this.sqlServerContext.Products.ToList();
            var vendors = this.sqlServerContext.Vendors.ToList();
            var incomes = this.sqlServerContext.Incomes.ToList();
            var expenses = this.sqlServerContext.Expenses.ToList();

            foreach (var measure in measures)
            {
                if (!context.Measures.Any(m => m.Name == measure.Name))
                {
                    context.Measures.Add(measure);
                }
            }

            foreach (var vendor in vendors)
            {
                if (!context.Vendors.Any(v => v.Name == vendor.Name))
                {
                    context.Vendors.Add(vendor);
                }
            }

            foreach (var product in products)
            {
                if (!context.Products.Any(p => p.Name == p.Name))
                {
                    context.Products.Add(product);
                }
            }

            foreach (var income in incomes)
            {
                if (!context.Incomes.Any(i => i.Date == income.Date))
                {
                    context.Incomes.Add(income);
                }
            }

            foreach (var expense in expenses)
            {
                if (!context.Expenses.Any(e => e.Date == expense.Date))
                {
                    context.Expenses.Add(expense);
                }
            }

            context.SaveChanges();
        }
    }
}

namespace DatabaseApps.Client.DbManagers
{
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using Models;
    using MsSql;
    using Oracle;

    public class OracleDBManager
    {
        private OracleDbContext oracleContext;

        public OracleDBManager()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<OracleDbContext>());
            this.oracleContext = new OracleDbContext();
        }

        public OracleDbContext OracleDbContext
        {
            get
            {
                return this.oracleContext;
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
                product.BuyingPrice = price;


                this.oracleContext.Products.Add(product);
                this.oracleContext.SaveChanges();
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

                this.oracleContext.Vendors.Add(vendor);
                this.oracleContext.SaveChanges();
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

                this.oracleContext.Measures.Add(measure);
                this.oracleContext.SaveChanges();
            }

            reader.Close();
        }

        public void ExportDataToMSSQLContext(MsSqlContext context)
        {
            var measures = this.oracleContext.Measures.ToList();
            var products = this.oracleContext.Products.ToList();
            var vendors = this.oracleContext.Vendors.ToList();
            var incomes = this.oracleContext.Incomes.ToList();
            var expenses = this.oracleContext.Expenses.ToList();

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

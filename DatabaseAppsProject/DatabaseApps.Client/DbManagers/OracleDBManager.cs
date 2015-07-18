namespace DatabaseApps.Client.DbManagers
{
    using System.Data.Entity;
    using System.IO;
    using Models;
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
                int vendorId = int.Parse(currentElements[1]);
                string productName = currentElements[2];
                int measureId = int.Parse(currentElements[3]);
                decimal price = decimal.Parse(currentElements[4]);

                Product product = new Product();
                product.Name = productName;
                product.VendorId = vendorId;
                product.MeasureId = measureId;
                product.Price = price;


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
                string vendorName = currentElements[1];

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
                string measureName = currentElements[1];

                Measure measure = new Measure();
                measure.Name = measureName;

                this.oracleContext.Measures.Add(measure);
                this.oracleContext.SaveChanges();
            }

            reader.Close();
        }
    }
}

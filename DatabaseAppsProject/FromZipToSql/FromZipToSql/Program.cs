using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace FromZipToSql
{
    public class Program
    {
        private static readonly ProductsEntities context = new ProductsEntities();

        static void Main()
        {
            string filePath = "D:\\Programming\\workspace\\DatabaseApplications\\FromZipToSql\\Sample-Sales-Reports.zip";
            string tempFolderName = "tempXlsFolder";

            FindAndExtactAllXlsFiles(filePath);
            
            GetAllXmlFiles(tempFolderName);

            ClearXlsDirectory();
        }

        public static void XlsReader(string fileLocation)
        {
            string con =
                string.Format(
                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}\{1};" +
                    @"Extended Properties='Excel 8.0;HDR=Yes;'", Directory.GetCurrentDirectory(), fileLocation);

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sales$]", connection);
                using (OleDbDataReader dr = command.ExecuteReader())
                {
                    int rowCounter = 0;
                    int vendorId = 0;
                    while (dr.Read())
                    {
                        if (rowCounter == 0)
                        {
                            var vendor = new Vendor()
                            {
                                Name = dr[0].ToString()
                            };

                            context.Vendors.AddOrUpdate(v => v.Name, vendor);
                            vendorId = vendor.Id;
                        }

                        if (!string.IsNullOrEmpty(dr[1].ToString()))
                        {
                            string name = dr[0].ToString();
                            int quantity = int.Parse(dr[1].ToString());
                            decimal price = decimal.Parse(dr[2].ToString());

                            AddProductsInDB(name, quantity, price, vendorId);
                        }
                        
                        rowCounter++;
                    }

                    context.SaveChanges();
                }
            }
        }

        public static void AddProductsInDB(string name, int quantity, decimal price, int vendorId)
        {
            var duplicatedProduct = context.Products.Where(p => p.Name == name && p.VendorId == vendorId);

            if (!duplicatedProduct.Any())
            {
                context.Products.Add(new Product()
                {
                    Name = name,
                    Quantity = quantity,
                    Price = price,
                    VendorId = vendorId,
                    MeasureId = context.Measures.Where(m => m.Name == "pieces").Select(m => m.Id).First()
                });
            }
        }

        public static void GetAllXmlFiles(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                foreach (string f in Directory.GetFiles(d))
                {
                    XlsReader(f);
                }
                GetAllXmlFiles(d);
            }
        }

        public static void FindAndExtactAllXlsFiles(string filePath)
        {
            using (ZipFile zip = ZipFile.Read(filePath))
            {
                foreach (var z in zip)
                {
                    if (z.FileName.EndsWith(".xls"))
                    {
                        z.Extract("tempXlsFolder");
                    }
                }
            }
        }

        public static void ClearXlsDirectory()
        {
            System.IO.DirectoryInfo downloadedMessageInfo = new DirectoryInfo("tempXlsFolder");

            foreach (FileInfo file in downloadedMessageInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in downloadedMessageInfo.GetDirectories())
            {
                dir.Delete(true);
            }
        }
    }
}

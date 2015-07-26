using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Ionic.Zip;

namespace FromZipToSql
{
    public static class Program
    {
        private static readonly ProductsEntities context = new ProductsEntities();
        private static readonly string zipFilesDirectory = Directory.GetCurrentDirectory() + @"\zipFiles\";
        private static readonly string tempFolderName = "tempXlsFolder";

        public static void Main()
        {
            List<string> zipFilesPath = GetAllFilesInDirectory(zipFilesDirectory, "zip");
            string filePath = String.Empty;
            foreach (var file in zipFilesPath)
            {
                filePath = zipFilesDirectory + file;
                ExtactXlsFilesFromZip(filePath);
            }

            List<string> xmlFilesPath = GetAllXmlFilesPath(tempFolderName);
            foreach (var file in xmlFilesPath)
            {
                XlsReader(file);
            }

            ClearDirectory(tempFolderName);
        }

        private static void XlsReader(string fileLocation)
        {
            string con =
                string.Format(
                    @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}\{1};" +
                    @"Extended Properties='Excel 8.0;HDR=Yes;'", Directory.GetCurrentDirectory(), fileLocation);

            using (OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [Sales$]", connection);
                using (OleDbDataReader dataReader = command.ExecuteReader())
                {
                    int rowCounter = 0;
                    int vendorId = 0;
                    while (dataReader.Read())
                    {
                        if (rowCounter == 0)
                        {
                            var vendor = new Vendor()
                            {
                                Name = dataReader[0].ToString()
                            };

                            context.Vendors.AddOrUpdate(v => v.Name, vendor);
                            vendorId = vendor.Id;
                        }

                        if (!string.IsNullOrEmpty(dataReader[1].ToString()))
                        {
                            string name = dataReader[0].ToString();
                            int quantity = int.Parse(dataReader[1].ToString());
                            decimal price = decimal.Parse(dataReader[2].ToString());

                            AddProductsInDB(name, quantity, price, vendorId);
                        }
                        
                        rowCounter++;
                    }

                    context.SaveChanges();
                }
            }
        }

        private static void AddProductsInDB(string name, int quantity, decimal price, int vendorId)
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

        private static List<string> GetAllXmlFilesPath(string directoryPath)
        {
            List<string> paths = new List<string>();
            foreach (string path in Directory.GetDirectories(directoryPath))
            {
                Directory.GetFiles(path).ToList().ForEach(f =>
                {
                    paths.Add(f);
                });
                GetAllXmlFilesPath(path);
            }

            return paths;
        }

        private static List<string> GetAllFilesInDirectory(string path, string fileExtension = null)
        {
            var directoryFiles = new List<string>();
            var directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                fileExtension = fileExtension == null ? "" : "." + fileExtension;
                FileInfo[] files = directory.GetFiles("*" + fileExtension);
                files.ToList().ForEach(f =>
                {
                    directoryFiles.Add(f.Name);
                }); 
            }

            return directoryFiles;
        }

        private static void ExtactXlsFilesFromZip(string zipFilePath)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(zipFilePath))
                {
                    foreach (var z in zip)
                    {
                        z.Extract(tempFolderName);
                    }
                }
            }
            catch (ZipException)
            {
                Console.WriteLine("File already exists.");
            }
        }

        public static void ClearDirectory(string folderName)
        {
            DirectoryInfo downloadedMessageInfo = new DirectoryInfo(folderName);

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
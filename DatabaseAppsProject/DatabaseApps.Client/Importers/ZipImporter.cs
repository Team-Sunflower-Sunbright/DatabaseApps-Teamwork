using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using DatabaseApps.Models;
using DatabaseApps.MsSql;
using Ionic.Zip;

namespace FromZipToSql
{
    public static class ZipImporter
    {
        private static readonly MsSqlContext context = new MsSqlContext();
        private static readonly string zipFilesDirectory = Directory.GetCurrentDirectory() + @"\zipFiles\";
        private static readonly string tempFolderName = "tempXlsFolder";

        public static void Import()
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
                    int supermarketId = 0;
                    while (dataReader.Read())
                    {
                        if (rowCounter == 0)
                        {
                            var supermarket = new Supermarket()
                            {
                                Name = dataReader[0].ToString()
                            };

                            context.Supermarkets.AddOrUpdate(v => v.Name, supermarket);
                            supermarketId = supermarket.Id;
                        }

                        if (!string.IsNullOrEmpty(dataReader[1].ToString()))
                        {
                            string name = dataReader[0].ToString();
                            int quantity = int.Parse(dataReader[1].ToString());
                            decimal price = decimal.Parse(dataReader[2].ToString());
                            double totalSum = double.Parse(dataReader[3].ToString());

                            AddProductsInDB(name, quantity, price, supermarketId, totalSum);
                        }

                        rowCounter++;
                    }

                    context.SaveChanges();
                }
            }
        }

        private static void AddProductsInDB(string name, int quantity, decimal price, int supermarketId, double totalSum)
        {
            name = name.Replace('“', '"');
            name = name.Replace('”', '"');
            var productExists = context.Products.Where(p => p.Name == name).Select(p => p.Id);
            if (productExists.Any())
            {
                context.Incomes.Add(new Income()
                {
                    Quantity = quantity,
                    Date = DateTime.Now,
                    ProductId = productExists.FirstOrDefault(),
                    SupermarketId = supermarketId,
                    SalePrice = price,
                    TotalSum = totalSum
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
            catch (Ionic.Zip.ZipException)
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
using MongoDB.Driver.Wrappers;

namespace DatabaseApps.Client.Exporters
{
    using System.Data.Entity;
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using MsSql;

    public static class XMLExporter
    {
        public static void ExportToXML(DateTime startDate, DateTime endDate)
        {
            const string dateFormat = "dd-MMM-yyy";
            var dbContext = new MsSqlContext();

            var vendors = dbContext.Vendors
                .Where(v => v.Products.Any(p => p.Incomes.Any(i => i.Date >= startDate && i.Date <= endDate)))
                .Select(v => new
                {
                    Vendor = v.Name,
                    Summary = v.Products
                        .Select(p => p.Incomes
                                .Where(i =>  i.Date >= startDate && i.Date <= endDate)
                                .GroupBy(gr => gr.Date).Select(i => new
                    {
                        Date = i.Key,
                        TotalSum = p.Incomes.Where(a => a.Date == i.Key).Sum(a => (double)a.SalePrice * a.Quantity)
                    })) 
                });

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            string fileName =
                @"vendorsSales-(" +
                startDate.Date.ToString(dateFormat) + ")-(" +
                endDate.Date.ToString(dateFormat) + ").xml";

            using (XmlWriter writer = XmlWriter.Create(fileName, settings))
            {
                writer.WriteStartDocument();


                writer.WriteStartElement("sales");

                foreach (var vendor in vendors)
                {
                    writer.WriteStartElement("sale");
                    writer.WriteAttributeString("vendor", vendor.Vendor);

                    foreach (var vs in vendor.Summary.FirstOrDefault())
                    {
                        
                        writer.WriteStartElement("summary");
                        writer.WriteAttributeString("date", vs.Date.Date.ToString(dateFormat));
                        writer.WriteAttributeString("total-sum", vs.TotalSum.ToString());
                        writer.WriteEndElement();
                        
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }
    }
}


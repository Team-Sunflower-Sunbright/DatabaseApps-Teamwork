namespace DatabaseApps.Client.Exporters
{
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

            var vendors = dbContext.Vendors.Select(v => new
            {
                vendor = v.Name,
                summary = v.Expenses.Where(e => e.Date >= startDate && e.Date <= endDate).Select(e => new
                {
                    date = e.Date,
                    totalSum = e.Amount
                })
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

                if (vendors.FirstOrDefault() != null)
                {
                    writer.WriteStartElement("sales");

                    foreach (var vendor in vendors)
                    {
                        writer.WriteStartElement("sale");
                        writer.WriteAttributeString("vendor", vendor.vendor);
                        foreach (var vs in vendor.summary)
                        {
                            writer.WriteStartElement("summary");
                            writer.WriteAttributeString("date", vs.date.Date.ToString(dateFormat));
                            writer.WriteAttributeString("total-sum", vs.totalSum.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                    }
                }
                else
                {
                    writer.WriteStartElement("Error");
                    writer.WriteAttributeString("errorMsg", "No data in current period!");
                }
                
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }
    }
}
namespace DatabaseApps.Client.Exporters
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using CodeFirst;

    public static class XMLExporter
    {
        public static void ExportToXML(DateTime startDate, DateTime endDate)
        {
            var dbContext = new DatabaseAppsModel();

            var vendors = dbContext.Vendors.Select(v => new
            {
                vendor = v.Name,
                summary = v.Expenses.Where(e => e.Period >= startDate && e.Period <= endDate).Select(e => new
                {
                    date = e.Period,
                    totalSum = e.Amount
                })
            });

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;


            using (XmlWriter writer = XmlWriter.Create("vendors.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("sales");

                foreach (var vendor in vendors)
                {
                    writer.WriteStartElement("sale");
                    writer.WriteAttributeString("vendor", vendor.vendor);
                    foreach (var vs in vendor.summary)
                    {
                        writer.WriteStartElement("summary");
                        writer.WriteAttributeString("date", vs.date.Date.ToString("dd-MMM-yyy"));
                        writer.WriteAttributeString("total-sum", vs.totalSum.ToString());
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
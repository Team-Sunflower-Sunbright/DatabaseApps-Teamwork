using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data.Entity.Migrations;
using System.Globalization;
using DatabaseApps.MsSql;
using DatabaseApps.Models;

namespace DatabaseApps.Client.Importers.XML
{
    public class XMLImporter
    {
        public static void ImportExpensesByMonth(MsSqlContext context, string xmlFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFile); 
            XmlNode vendors = xmlDoc.SelectSingleNode("expenses-by-month");

            foreach (XmlNode vendor in vendors)
            {
                var currentVendor = new Vendor()
                {
                    Name = vendor.Attributes["name"].InnerText,
                };

                context.Vendors.AddOrUpdate(v => v.Name, currentVendor);
                context.SaveChanges();
                foreach (XmlNode expenses in vendor.ChildNodes)
                {
                    var dateString = expenses.Attributes["month"].InnerText;
                    var expensesString = expenses.InnerText;
                    var date = DateTime.Parse(dateString);
                    decimal expense = Decimal.Parse(expensesString, CultureInfo.InvariantCulture);

                    context.Expenses.Add(new Expense()
                    {
                        Date = date,
                        Amount = expense,
                        VendorId = currentVendor.Id
                    });
                }
            }
            context.SaveChanges();
        }
    }
}

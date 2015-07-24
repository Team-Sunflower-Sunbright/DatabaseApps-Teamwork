namespace DatabaseApps.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Used to aggregate data for easier MySQL export.
    /// </summary>
    public class MySqlExportData
    {
        public string VendorName { get; set; }

        public IDictionary<string, double> Products { get; set; }

        public decimal Expenses { get; set; }
    }
}

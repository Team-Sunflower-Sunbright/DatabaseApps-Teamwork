namespace DatabaseApps.Models
{
    public class ExcelReportData
    {
        public string VendorName { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal TotalTaxes { get; set; }

        public decimal FinancialResult { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                this.VendorName,
                this.TotalIncome,
                this.TotalExpense,
                this.TotalTaxes,
                this.FinancialResult);
        }
    }
}

namespace DatabaseApps.Models
{
    using System;

    public class Expense
    {
        public int Id { get; set; }

        public DateTime Period { get; set; }

        public decimal Amount { get; set; }

        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}

namespace DatabaseApps.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Expense
    {
        [Key]
        public int Id { get; set; }

        public DateTime Period { get; set; }

        public decimal Amount { get; set; }

        public int VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}

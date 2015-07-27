namespace DatabaseApps.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Income
    {
        [Key]
        public int Id { get; set; }

        public double? Quantity { get; set; }

        public DateTime Date { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        public int SupermarketId { get; set; }

        public virtual Supermarket Supermarket { get; set; }

        public decimal SalePrice { get; set; }

        public double TotalSum { get; set; }
    }
}

namespace DatabaseApps.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Sale
    {
        [Key]
        public int Id { get; set; }

        public int TotalSold { get; set; }

        public decimal TotalIncome { get; set; }

        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}

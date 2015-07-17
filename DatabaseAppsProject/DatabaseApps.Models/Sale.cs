namespace DatabaseApps.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public int TotalSold { get; set; }

        public decimal TotalIncome { get; set; }

        public Product ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}

namespace DatabaseApps.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class Supermarket
    {
        private ICollection<Income> sales;
        public Supermarket()
        {
            this.sales = new HashSet<Income>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Income> Sales
        {
            get { return this.sales; }
            set { this.sales = value; }
        }
    }
}
namespace DatabaseApps.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        private ICollection<Income> sales;

        public Product()
        {
            this.sales = new HashSet<Income>();
        }

        [Key]
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int MeasureId
        {
            get;
            set;
        }

        public virtual Measure Measure
        {
            get;
            set;
        }

        public int VendorId
        {
            get;
            set;
        }

        public virtual Vendor Vendor
        {
            get;
            set;
        }

        public decimal BuyingPrice
        {
            get;
            set;
        }

        public virtual ICollection<Income> Incomes
        {
            get { return this.sales; }
            set { this.sales = value; }
        }
    }
}

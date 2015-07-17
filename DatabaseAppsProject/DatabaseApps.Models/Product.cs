namespace DatabaseApps.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Product
    {
        private ICollection<Sale> sales;

        public Product()
        {
            this.sales = new HashSet<Sale>();
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

        public decimal Price
        {
            get;
            set;
        }

        public Sale SaleId { get; set; }

        public virtual ICollection<Sale> Sales
        {
            get { return this.sales; }
            set { this.sales = value; }
        }
    }
}

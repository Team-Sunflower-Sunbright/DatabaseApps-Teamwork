namespace DatabaseApps.Models
{
    public class Product
    {
        public int ProductId
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
    }
}

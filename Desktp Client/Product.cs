namespace Facilitate.Desktop.Models
{
    public class Product
    {
        public Product()
        {
            name = string.Empty;
            id = 0;
            priceInfo = new PriceInfo();
            priceRange = new PriceRange();
            wasteFactorMainRoof = 0.00;
        }

        public string name { get; set; }
        public int id { get; set; }

        public PriceInfo priceInfo { get; set; }
        public PriceRange priceRange { get; set; }

        public double? wasteFactorMainRoof { get; set; }
    }
}

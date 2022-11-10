namespace Data
{
    public class PriceFormatting
    {
        public PriceFormatting(decimal p)
        {
            this.Price = p;
            this.Currency = "USD";
        }

        public decimal Price { get; set; }

        public string Currency { get; set; }

        public string ShowPrice()
        {
            return this.Price + this.Currency;
        }
    }
}

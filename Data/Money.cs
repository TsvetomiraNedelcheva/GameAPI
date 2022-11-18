namespace Data
{
    public class Money
    {
        public Money(decimal amount, string currencyCode)
        {
            if (amount < 0) throw new ArgumentException("Money could not be negative", nameof(amount));
            if (string.IsNullOrEmpty(currencyCode)) throw new ArgumentNullException(nameof(currencyCode));
            if (CurrencyCode.IsValidCode(currencyCode) == false) throw new ArgumentException("Invalid Currency code", nameof(currencyCode));

            Amount = amount;
            Currency = currencyCode;
        }

        public decimal Amount { get; set; }

        public string Currency { get; set; }
    }
}

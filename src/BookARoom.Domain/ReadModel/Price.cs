namespace BookARoom.Domain.ReadModel
{
    public class Price
    {
        public string Currency;
        public double Value;

        public Price(string currency, double value)
        {
            Currency = currency;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Value} {Currency}";
        }
    }
}
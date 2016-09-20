namespace BookARoom.IntegrationModel
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
    }
}
namespace BookARoom.IntegrationModel
{
    public class RoomStatusAndPrices
    {
        public string RoomIdentifier { get; }
        public Price PriceForOneAdult { get; }
        public Price PriceForTwoAdults { get; }

        public RoomStatusAndPrices(string roomIdentifier, Price priceForOneAdult, Price priceForTwoAdults)
        {
            RoomIdentifier = roomIdentifier;
            PriceForOneAdult = priceForOneAdult;
            PriceForTwoAdults = priceForTwoAdults;
        }
    }
}
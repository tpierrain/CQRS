namespace BookARoom.Domain.ReadModel
{
    public class RoomWithPrices
    {
        public string RoomIdentifier;
        public Price PriceForOneAdult;
        public Price PriceForTwoAdults;

        public RoomWithPrices(string roomIdentifier, Price priceForOneAdult, Price priceForTwoAdults)
        {
            RoomIdentifier = roomIdentifier;
            PriceForOneAdult = priceForOneAdult;
            PriceForTwoAdults = priceForTwoAdults;
        }
    }
}
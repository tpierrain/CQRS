namespace BookARoom.Domain
{
    public class RoomStatus
    {
        public string RoomIdentifier;
        public Price PriceForOneAdult;
        public Price PriceForTwoAdults;

        public RoomStatus(string roomIdentifier, Price priceForOneAdult, Price priceForTwoAdults)
        {
            RoomIdentifier = roomIdentifier;
            PriceForOneAdult = priceForOneAdult;
            PriceForTwoAdults = priceForTwoAdults;
        }
    }
}
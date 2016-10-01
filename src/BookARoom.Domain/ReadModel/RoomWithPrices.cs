namespace BookARoom.Domain.ReadModel
{
    public class RoomWithPrices
    {
        public string RoomIdentifier;
        public Price OneAdultOccupancyPrice;
        public Price TwoAdultsOccupancyPrice;

        public RoomWithPrices(string roomIdentifier, Price oneAdultOccupancyPrice, Price twoAdultsOccupancyPrice)
        {
            RoomIdentifier = roomIdentifier;
            OneAdultOccupancyPrice = oneAdultOccupancyPrice;
            TwoAdultsOccupancyPrice = twoAdultsOccupancyPrice;
        }
    }
}
namespace BookARoom.IntegrationModel
{
    public class RoomStatusAndPrices
    {
        public string RoomIdentifier { get; }
        public Price OneAdultOccupancyPrice { get; }
        public Price TwoAdultsOccupancyPrice { get; }

        public RoomStatusAndPrices(string roomIdentifier, Price oneAdultOccupancyPrice, Price twoAdultsOccupancyPrice)
        {
            RoomIdentifier = roomIdentifier;
            OneAdultOccupancyPrice = oneAdultOccupancyPrice;
            TwoAdultsOccupancyPrice = twoAdultsOccupancyPrice;
        }
    }
}
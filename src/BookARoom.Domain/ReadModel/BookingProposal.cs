using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    public class BookingProposal
    {
        public Place Place { get; }

        public IEnumerable<RoomWithPrices> AvailableRoomsWithPrices { get; }

        public BookingProposal(Place place, IEnumerable<RoomWithPrices> availableRoomsWithPrices)
        {
            this.Place = place;
            this.AvailableRoomsWithPrices = availableRoomsWithPrices;
        }
    }
}

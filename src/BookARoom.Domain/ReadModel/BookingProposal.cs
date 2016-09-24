using System.Collections.Generic;
using System.Linq;

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

        public override string ToString()
        {
            return $"Booking proposal for place: '{this.Place}' - {this.AvailableRoomsWithPrices.Count()} possible room(s)";
        }
    }
}

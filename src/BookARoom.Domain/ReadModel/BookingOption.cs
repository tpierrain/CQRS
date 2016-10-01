using System.Collections.Generic;
using System.Linq;

namespace BookARoom.Domain.ReadModel
{
    public class BookingOption
    {
        public Hotel Hotel { get; }

        public IEnumerable<RoomWithPrices> AvailableRoomsWithPrices { get; }

        public BookingOption(Hotel hotel, IEnumerable<RoomWithPrices> availableRoomsWithPrices)
        {
            this.Hotel = hotel;
            this.AvailableRoomsWithPrices = availableRoomsWithPrices;
        }

        public override string ToString()
        {
            return $"Booking option for hotel: '{this.Hotel}' - {this.AvailableRoomsWithPrices.Count()} possible room(s)";
        }
    }
}

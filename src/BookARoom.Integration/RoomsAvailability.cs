using System;
using System.Collections.Generic;

namespace BookARoom.Integration
{
    public class RoomsAvailability
    {
        public string PlaceName { get; }
        public Dictionary<DateTime, RoomStatusAndPrices[]> AvailabilitiesAt { get; }

        public RoomsAvailability(string placeName)
        {
            this.PlaceName = placeName;
            this.AvailabilitiesAt = new Dictionary<DateTime, RoomStatusAndPrices[]>();
        }
    }
}
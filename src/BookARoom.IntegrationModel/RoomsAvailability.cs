using System;
using System.Collections.Generic;

namespace BookARoom.IntegrationModel
{
    public class RoomsAvailability
    {
        public string PlaceName { get; }
        public string Location { get; }
        public Dictionary<DateTime, RoomStatusAndPrices[]> AvailabilitiesAt { get; }

        public RoomsAvailability(string placeName, string location)
        {
            this.PlaceName = placeName;
            this.Location = location;
            this.AvailabilitiesAt = new Dictionary<DateTime, RoomStatusAndPrices[]>();
        }
    }
}
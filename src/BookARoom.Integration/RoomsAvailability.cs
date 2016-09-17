using System;
using System.Collections.Generic;

namespace BookARoom.Integration
{
    public class RoomsAvailability
    {
        public string PlaceName;
        public Dictionary<DateTime, RoomStatus[]> AvailabilitiesAt;

        public RoomsAvailability(string placeName)
        {
            this.PlaceName = placeName;
            this.AvailabilitiesAt = new Dictionary<DateTime, RoomStatus[]>();
        }
    }
}
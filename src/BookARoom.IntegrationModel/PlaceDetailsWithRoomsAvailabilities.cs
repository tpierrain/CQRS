using System;
using System.Collections.Generic;

namespace BookARoom.IntegrationModel
{
    public class PlaceDetailsWithRoomsAvailabilities
    {
        // NOTE: the Id should probably be checked/controled on the BookARoom side.
        public int PlaceId { get; }
        public string PlaceName { get; }
        public string Location { get; }
        public int NumberOfRooms { get; set; }
        public Dictionary<DateTime, RoomStatusAndPrices[]> AvailabilitiesAt { get; }

        public PlaceDetailsWithRoomsAvailabilities(int placeId, string placeName, string location, int numberOfRooms)
        {
            this.PlaceId = placeId;
            this.PlaceName = placeName;
            this.Location = location;
            NumberOfRooms = numberOfRooms;
            this.AvailabilitiesAt = new Dictionary<DateTime, RoomStatusAndPrices[]>();
        }
    }
}
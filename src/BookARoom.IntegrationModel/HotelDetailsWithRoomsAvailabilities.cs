using System;
using System.Collections.Generic;

namespace BookARoom.IntegrationModel
{
    public class HotelDetailsWithRoomsAvailabilities
    {
        // NOTE: the Id should probably be checked/controled on the BookARoom side.
        public int HotelId { get; }
        public string HotelName { get; }
        public string Location { get; }
        public int NumberOfRooms { get; set; }
        public Dictionary<DateTime, RoomStatusAndPrices[]> AvailabilitiesAt { get; }

        public HotelDetailsWithRoomsAvailabilities(int hotelId, string hotelName, string location, int numberOfRooms)
        {
            this.HotelId = hotelId;
            this.HotelName = hotelName;
            this.Location = location;
            this.NumberOfRooms = numberOfRooms;
            this.AvailabilitiesAt = new Dictionary<DateTime, RoomStatusAndPrices[]>();
        }
    }
}
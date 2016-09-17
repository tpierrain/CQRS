using System;
using System.Collections.Generic;

namespace BookARoom.Domain
{
    public class RoomBookingEngine
    {
        private readonly ICatalogPlaces places;

        public RoomBookingEngine(ICatalogPlaces places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int roomNumber, int adultsCount, int childrenCount)
        {
            return this.places.SearchFromLocation(location);
        }
    }
}
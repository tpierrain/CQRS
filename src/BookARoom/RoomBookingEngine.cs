namespace BookARoom
{
    using System;
    using System.Collections.Generic;

    public class RoomBookingEngine
    {
        private readonly PlacesCatalog places;

        public RoomBookingEngine(PlacesCatalog places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int roomNumber, int adultsCout, int childrenCount)
        {
            return this.places.SearchFromLocation(location);
        }
    }
}
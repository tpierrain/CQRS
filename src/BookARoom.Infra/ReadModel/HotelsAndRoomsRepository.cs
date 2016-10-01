using System;
using System.Collections.Generic;
using System.Linq;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.ReadModel
{
    public class HotelsAndRoomsRepository : IStoreAndProvideHotelsAndRooms
    {
        public readonly Dictionary<Hotel, Dictionary<DateTime, List<RoomWithPrices>>> hotelsWithPerDateRoomsStatus;
        private readonly Dictionary<int, Hotel> hotelsPerId = new Dictionary<int, Hotel>();

        public HotelsAndRoomsRepository()
        {
            this.hotelsWithPerDateRoomsStatus = new Dictionary<Hotel, Dictionary<DateTime, List<RoomWithPrices>>>();
        }

        public IEnumerable<Hotel> Hotels => this.hotelsWithPerDateRoomsStatus.Keys;

        public IEnumerable<BookingOption> SearchAvailableHotelsInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
        {
            var result = (from hotelWithAvailabilities in this.hotelsWithPerDateRoomsStatus
                from dateAndRooms in this.hotelsWithPerDateRoomsStatus.Values
                from date in dateAndRooms.Keys
                from availableRooms in dateAndRooms.Values
                where string.Equals(hotelWithAvailabilities.Key.Location, location, StringComparison.CurrentCultureIgnoreCase)
                      && (date >= checkInDate && date <= checkOutDate)
                      && availableRooms.Count > 0
                      && dateAndRooms.Values.Contains(availableRooms)
                      && hotelWithAvailabilities.Value == dateAndRooms
                select new BookingOption(hotelWithAvailabilities.Key, availableRooms) ).ToList().Distinct();

            return result;
        }

        public void DeclareRoomBooked(int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            var hotel = this.hotelsPerId[hotelId];
            var availabilitiesPerDate = this.hotelsWithPerDateRoomsStatus[hotel];

            var availabilities = availabilitiesPerDate[checkInDate];

            RoomWithPrices roomAvailabilityToRemove = availabilities.FirstOrDefault(roomWithPrices => roomWithPrices.RoomIdentifier == roomNumber);

            availabilities.Remove(roomAvailabilityToRemove);
        }

        public void StoreHotelAvailabilities(Hotel hotel, Dictionary<DateTime, List<RoomWithPrices>> perDateRoomsAvailabilities)
        {
            this.hotelsWithPerDateRoomsStatus[hotel] = perDateRoomsAvailabilities;
        }

        public IEnumerable<Hotel> SearchFromLocation(string location)
        {
            return from hotel in this.hotelsWithPerDateRoomsStatus.Keys
                where hotel.Location == location
                select hotel;
        }

        public Hotel GetHotel(int hotelId)
        {
            return this.hotelsPerId[hotelId];
        }

        public void StoreHotel(int hotelId, Hotel hotel)
        {
            this.hotelsPerId[hotelId] = hotel;
        }
    }
}
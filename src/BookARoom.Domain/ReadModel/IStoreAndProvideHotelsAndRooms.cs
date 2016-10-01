using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    public interface IStoreAndProvideHotelsAndRooms : IProvideHotel
    {
        IEnumerable<Hotel> Hotels { get; }

        IEnumerable<BookingOption> SearchAvailableHotelsInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate);

        void StoreHotel(int hotelId, Hotel hotel);
        void StoreHotelAvailabilities(Hotel hotel, Dictionary<DateTime, List<RoomWithPrices>> perDateRoomsAvailabilities);

        void DeclareRoomBooked(int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate);
    }
}

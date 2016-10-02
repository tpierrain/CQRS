using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Allow to search BookingOptions or to get details about Hotels.
    /// </summary>
    public class ReadModelFacade : IQueryBookingOptions, IProvideHotel, IProvideReservations
    {
        // TODO: question: find a domain name instead or keep focus on the CQRS pattern to ease understanding of the MS experiences'16 audience?

        private readonly IProvideRooms roomsProvider;
        private readonly IProvideHotel hotelProvider;
        private readonly IProvideReservations reservationsProvider;

        /// <summary>
        /// Instantiates a <see cref="ReadModelFacade"/>.
        /// </summary>
        /// <param name="roomsProvider"></param>
        /// <param name="hotelProvider"></param>
        /// <param name="reservationsProvider"></param>
        /// <param name="bus"></param>
        public ReadModelFacade(IProvideRooms roomsProvider, IProvideHotel hotelProvider, IProvideReservations reservationsProvider, ISubscribeToEvents bus)
        {
            this.roomsProvider = roomsProvider;
            this.hotelProvider = hotelProvider;
            this.reservationsProvider = reservationsProvider;
        }

        #region IQueryBookingOptions members

        public IEnumerable<BookingOption> SearchBookingOptions(SearchBookingOptions query)
        {
            return this.SearchBookingOptions(query.CheckInDate, query.CheckOutDate, query.Location, query.NumberOfAdults, query.NumberOfRoomsNeeded, query.ChildrenCount);
        }

        private IEnumerable<BookingOption> SearchBookingOptions(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int numberOfRoomsNeeded = 1, int childrenCount = 0)
        {
            if (checkInDate > checkOutDate)
            {
                throw new InvalidOperationException($"Check out date ({checkOutDate}) must be after Check in date ({checkInDate}).");
            }

            return this.roomsProvider.SearchAvailableHotelsInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }

        #endregion

        #region IProvideHotel members

        public IEnumerable<Hotel> SearchFromLocation(string location)
        {
            throw new NotImplementedException();
        }

        public Hotel GetHotel(int hotelId)
        {
            return this.hotelProvider.GetHotel(hotelId);
        }

        #endregion

        public IEnumerable<Reservation> GetReservationsFor(string clientId)
        {
            return this.reservationsProvider.GetReservationsFor(clientId);
        }
    }
}
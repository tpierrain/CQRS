using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Allow to search BookingProposals or to get details about Hotels.
    /// </summary>
    public class ReadModelFacade : IQueryBookingProposals, IProvideHotel
    {
        // TODO: question: find a domain name instead or keep focus on the CQRS pattern to ease understanding of the MS experiences'16 audience?

        private readonly IProvideRooms roomsProvider;
        private readonly IProvideHotel hotelProvider;

        /// <summary>
        /// Instantiates a <see cref="ReadModelFacade"/>.
        /// </summary>
        /// <param name="roomsProvider"></param>
        /// <param name="hotelProvider"></param>
        public ReadModelFacade(IProvideRooms roomsProvider, IProvideHotel hotelProvider)
        {
            this.roomsProvider = roomsProvider;
            this.hotelProvider = hotelProvider;
        }

        #region IQueryBookingProposals members

        public IEnumerable<BookingProposal> SearchBookingProposals(SearchBookingProposal query)
        {
            return this.SearchBookingProposals(query.CheckInDate, query.CheckOutDate, query.Location, query.NumberOfAdults, query.NumberOfRoomsNeeded, query.ChildrenCount);
        }

        private IEnumerable<BookingProposal> SearchBookingProposals(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int numberOfRoomsNeeded = 1, int childrenCount = 0)
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
    }
}
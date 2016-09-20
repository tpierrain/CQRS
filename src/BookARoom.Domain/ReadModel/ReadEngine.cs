using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Search BookingProposals or details about Places.
    /// </summary>
    public class ReadEngine : ISearchBookingProposals, IProvidePlaces
    {
        // TODO: question: find a domain name instead or keep focus on the CQRS pattern to ease understanding of the MS experiences'16 audience?

        private readonly IProvideRooms roomsProvider;
        private readonly IProvidePlaces placesProvider;

        /// <summary>
        /// Instantiates a <see cref="ReadEngine"/>.
        /// </summary>
        /// <param name="roomsProvider"></param>
        /// <param name="placesProvider"></param>
        public ReadEngine(IProvideRooms roomsProvider, IProvidePlaces placesProvider)
        {
            this.roomsProvider = roomsProvider;
            this.placesProvider = placesProvider;
        }

        #region ISearchBookingProposals members

        public IEnumerable<BookingProposal> SearchBookingProposals(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int roomNumber = 1, int childrenCount = 0)
        {
            if (checkInDate > checkOutDate)
            {
                throw new InvalidOperationException($"Check out date ({checkOutDate}) must be after Check in date ({checkInDate}).");
            }

            return this.roomsProvider.SearchAvailablePlacesInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }

        #endregion

        #region IProvidePlaces members

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            throw new NotImplementedException();
        }

        public Place GetPlace(int placeId)
        {
            return this.placesProvider.GetPlace(placeId);
        }

        #endregion
    }
}
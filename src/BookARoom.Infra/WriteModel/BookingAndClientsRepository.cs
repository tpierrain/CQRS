using System;
using System.Collections.Generic;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.WriteModel
{
    public class BookingAndClientsRepository : IBookingRepository, IClientRepository
    {
        private readonly Dictionary<string, List<Booking>> perClientBookings;

        public BookingAndClientsRepository()
        {
            this.perClientBookings = new Dictionary<string, List<Booking>>();
        }

        public void Save(Booking booking)
        {
            // In our case the Guid is provided here, at the persistence level
            this.perClientBookings[booking.ClientId].Add(booking);
        }

        public bool IsClientAlready(string clientIdentifier)
        {
            return this.perClientBookings.ContainsKey(clientIdentifier);
        }

        public void CreateClient(string clientIdentifier)
        {
            if (!this.perClientBookings.ContainsKey(clientIdentifier))
            {
                this.perClientBookings[clientIdentifier] = new List<Booking>();
            }
        }

        public IEnumerable<Booking> GetBookingsFrom(string clientIdentifier)
        {
            return this.perClientBookings[clientIdentifier];
        }

        public Booking GetBooking(string clientId, Guid bookingId)
        {
            var allBookingsForThisClient = this.perClientBookings[clientId];
            foreach (var booking in allBookingsForThisClient)
            {
                if (booking.BookingId == bookingId)
                {
                    return booking;
                }
            }

            return Booking.Null;
        }

        public void Update(Booking booking)
        {
            throw new NotImplementedException();
        }
    }
}
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
    }
}
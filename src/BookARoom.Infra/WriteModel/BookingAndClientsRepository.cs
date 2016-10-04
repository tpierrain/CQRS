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
            perClientBookings = new Dictionary<string, List<Booking>>();
        }

        public void Save(Booking booking)
        {
            perClientBookings[booking.ClientId].Add(booking);
        }

        public bool IsClientAlready(string clientIdentifier)
        {
            return perClientBookings.ContainsKey(clientIdentifier);
        }

        public void CreateClient(string clientIdentifier)
        {
            if (!perClientBookings.ContainsKey(clientIdentifier))
            { 
                perClientBookings[clientIdentifier] = new List<Booking>();
            }
        }

        public IEnumerable<Booking> GetBookingsFrom(string clientIdentifier)
        {
            if (perClientBookings.ContainsKey(clientIdentifier))
            { 
                return perClientBookings[clientIdentifier];
            }

            return new List<Booking>();
        }

        public Booking GetBooking(string clientId, Guid bookingGuid)
        {
            var bookingList = this.perClientBookings[clientId];
            foreach (var booking in bookingList)
            {
                if (booking.BookingId == bookingGuid)
                {
                    return booking;
                }
            }

            return Booking.Null;
        }

        public void Update(Booking booking)
        {
            // Method existing for real Db
        }
    }
}
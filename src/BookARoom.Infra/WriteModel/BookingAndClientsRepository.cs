using System;
using System.Collections.Generic;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.WriteModel
{
    public class BookingAndClientsRepository : IBookingRepository, IClientRepository
    {
        private readonly Dictionary<string, List<Booking>> perClientCommands;

        public BookingAndClientsRepository()
        {
            this.perClientCommands = new Dictionary<string, List<Booking>>();
        }

        public void Save(Booking booking)
        {
            // In our case the Guid is provided here, at the persistence level
            this.perClientCommands[booking.ClientId].Add(booking);
        }

        public bool IsClientAlready(string clientIdentifier)
        {
            return this.perClientCommands.ContainsKey(clientIdentifier);
        }

        public void CreateClient(string clientIdentifier)
        {
            if (!this.perClientCommands.ContainsKey(clientIdentifier))
            {
                this.perClientCommands[clientIdentifier] = new List<Booking>();
            }
        }

        public IEnumerable<Booking> GetBookingCommandsFrom(string clientIdentifier)
        {
            return this.perClientCommands[clientIdentifier];
        }

        public Booking GetBooking(string clientId, Guid bookingId)
        {
            var allBookingsForThisClient = this.perClientCommands[clientId];
            foreach (var booking in allBookingsForThisClient)
            {
                if (booking != null && booking.BookingId == bookingId)
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
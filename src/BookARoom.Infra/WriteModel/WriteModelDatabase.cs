using System.Collections.Generic;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.WriteModel
{
    public class WriteModelDatabase : IBookingStore
    {
        public long BookingCount { get; private set; }

        private readonly Dictionary<string, List<ICommand>> perClientCommands;

        public WriteModelDatabase()
        {
            this.perClientCommands = new Dictionary<string, List<ICommand>>();
        }

        public void BookARoom(BookARoomCommand bookingRequest)
        {
            if (!this.perClientCommands.ContainsKey(bookingRequest.ClientId))
            {
                this.perClientCommands[bookingRequest.ClientId] = new List<ICommand>();
            }

            this.perClientCommands[bookingRequest.ClientId].Add(bookingRequest);
            this.BookingCount++;
        }
    }
}
using System.Collections.Generic;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.WriteModel
{
    public class WriteModelDatabase : IClientAndBookingRepository
    {
        public long BookingCount { get; private set; }

        private readonly Dictionary<string, List<ICommand>> perClientCommands;

        public WriteModelDatabase()
        {
            this.perClientCommands = new Dictionary<string, List<ICommand>>();
        }

        public void Save(BookARoomCommand bookingRequest)
        {
            this.perClientCommands[bookingRequest.ClientId].Add(bookingRequest);
            this.BookingCount++;
        }

        public bool IsClientAlready(string clientIdentifier)
        {
            return this.perClientCommands.ContainsKey(clientIdentifier);
        }

        public void CreateClient(string clientIdentifier)
        {
            if (!this.perClientCommands.ContainsKey(clientIdentifier))
            {
                this.perClientCommands[clientIdentifier] = new List<ICommand>();
            }
        }

        public IEnumerable<ICommand> GetBookingRequestsFrom(string clientIdentifier)
        {
            return this.perClientCommands[clientIdentifier];
        }
    }
}
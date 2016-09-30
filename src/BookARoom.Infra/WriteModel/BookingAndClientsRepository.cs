using System.Collections.Generic;
using BookARoom.Domain;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.WriteModel
{
    public class BookingAndClientsRepository : IBookingRepository, IClientRepository
    {
        private readonly Dictionary<string, List<ICommand>> perClientCommands;

        public BookingAndClientsRepository()
        {
            this.perClientCommands = new Dictionary<string, List<ICommand>>();
        }

        public void Save(BookARoomCommand bookingCommand)
        {
            this.perClientCommands[bookingCommand.ClientId].Add(bookingCommand);
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

        public IEnumerable<ICommand> GetBookingCommandsFrom(string clientIdentifier)
        {
            return this.perClientCommands[clientIdentifier];
        }
    }
}
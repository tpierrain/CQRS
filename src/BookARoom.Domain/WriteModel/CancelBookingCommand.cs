using System;

namespace BookARoom.Domain.WriteModel
{
    public class CancelBookingCommand: ICommand
    {
        public Guid BookingCommandGuid { get; }
        public string ClientId { get; }

        public CancelBookingCommand(Guid bookingCommandGuid, string clientId)
        {
            this.BookingCommandGuid = bookingCommandGuid;
            this.ClientId = clientId;
        }
    }
}
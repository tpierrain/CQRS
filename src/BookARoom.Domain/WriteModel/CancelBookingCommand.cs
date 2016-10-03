using System;

namespace BookARoom.Domain.WriteModel
{
    public class CancelBookingCommand: ICommand
    {
        public Guid BookingId { get; }
        public string ClientId { get; }

        public CancelBookingCommand(Guid bookingId, string clientId)
        {
            this.BookingId = bookingId;
            this.ClientId = clientId;
        }
    }
}
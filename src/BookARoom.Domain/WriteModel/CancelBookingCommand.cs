using System;

namespace BookARoom.Domain.WriteModel
{
    public class CancelBookingCommand : ICommand
    {
        public Guid BookingGuid { get; set; }
        public string ClientId { get; set; }

        public CancelBookingCommand(Guid bookingGuid, string clientId)
        {
            BookingGuid = bookingGuid;
            ClientId = clientId;
        }
    }
}
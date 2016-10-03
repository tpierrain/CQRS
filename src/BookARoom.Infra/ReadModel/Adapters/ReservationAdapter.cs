using System.Collections.Generic;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using ISubscribeToEvents = BookARoom.Domain.ReadModel.ISubscribeToEvents;

namespace BookARoom.Infra.ReadModel.Adapters
{
    public class ReservationAdapter : IProvideReservations
    {
        private readonly ISubscribeToEvents eventsSubscriber;
        private readonly Dictionary<string, List<Reservation>> perClientReservations = new Dictionary<string, List<Reservation>>();

        public ReservationAdapter(ISubscribeToEvents eventsSubscriber)
        {
            this.eventsSubscriber = eventsSubscriber;
            this.eventsSubscriber.RegisterHandler<RoomBooked>(Handle);
        }

        private void Handle(RoomBooked @event)
        {
            if (!this.perClientReservations.ContainsKey(@event.ClientId))
            {
                this.perClientReservations[@event.ClientId] = new List<Reservation>();
            }

            var reservation = new Reservation(@event.Guid, @event.ClientId, @event.HotelName, @event.HotelId.ToString(), @event.RoomNumber, @event.CheckInDate, @event.CheckOutDate);
            this.perClientReservations[@event.ClientId].Add(reservation);
        }

        public IEnumerable<Reservation> GetReservationsFor(string clientId)
        {
            List<Reservation> result;
            this.perClientReservations.TryGetValue(clientId, out result);

            if (result == null)
            {
                result = new List<Reservation>();    
            }

            return result;
        }
    }
}
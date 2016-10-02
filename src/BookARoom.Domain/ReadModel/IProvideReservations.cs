using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    public interface IProvideReservations
    {
        IEnumerable<Reservation> GetReservationsFor(string clientId);
    }
}
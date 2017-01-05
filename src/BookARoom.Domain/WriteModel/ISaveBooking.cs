using System;

namespace BookARoom.Domain.WriteModel
{
    public interface ISaveBooking
    {
        void Save(Booking booking);
    }
}
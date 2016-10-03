using System;

namespace BookARoom.Domain.WriteModel
{
    public class Booking
    {
        public static Booking Null { get; } = new NullBooking();

        // We provide getters only so that the state of this domain object is only changed via one of its operations (methods)
        public Guid BookingId { get; }
        public string ClientId { get; }
        public int HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public bool IsCanceled { get; private set; }

        public Booking(Guid bookingId , string clientId, int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.BookingId = bookingId;
            this.ClientId = clientId;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }

        public virtual bool IsForClient(string clientId)
        {
            if (this.ClientId == clientId)
            {
                return true;
            }

            return false;
        }

        public virtual void Cancel()
        {
            this.IsCanceled = true;
        }

        private class NullBooking : Booking
        {
            public NullBooking() : base(Guid.Empty, string.Empty, 0, string.Empty, DateTime.Now, DateTime.Now)
            {
            }

            public override bool IsForClient(string clientId)
            {
                return false;
            }

            public override void Cancel()
            {
            }
        }
    }
}
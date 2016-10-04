using System;

namespace BookARoom.Domain.WriteModel
{
    public class Booking
    {
        public bool IsCanceled { get; private set; }
        // We provide getters only so that the state of this domain object is only changed via one of its operations (methods)
        public Guid BookingId { get; }
        public string ClientId { get; }
        public int HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public static Booking Null => new NullBooking();

        private class NullBooking : Booking
        {
            public NullBooking() : base(Guid.Empty, string.Empty, -1, string.Empty, DateTime.Now, DateTime.Now)
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
            return this.ClientId == clientId;
        }

        public virtual void Cancel()
        {
            this.IsCanceled = true;
        }
    }
}
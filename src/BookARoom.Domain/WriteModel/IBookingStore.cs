namespace BookARoom.Domain.WriteModel
{
    public interface IBookingStore
    {
        void BookARoom(BookARoomCommand bookingRequest);
    }
}
namespace BookARoom.Domain.WriteModel
{
    public class BookingCommandHandler
    {
        public IBookingStore BookingStore { get; }

        public BookingCommandHandler(IBookingStore bookingStore)
        {
            this.BookingStore = bookingStore;
        }

        public void Handle(BookARoomCommand command)
        {
            this.BookingStore.BookARoom(command);
        }
    }
}
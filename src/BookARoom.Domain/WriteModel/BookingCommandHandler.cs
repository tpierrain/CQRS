namespace BookARoom.Domain.WriteModel
{
    public class BookingCommandHandler
    {
        public IBookRooms BookingStore { get; }

        public BookingCommandHandler(IBookRooms bookingStore)
        {
            this.BookingStore = bookingStore;
        }

        public void Handle(BookARoomCommand command)
        {
            this.BookingStore.BookARoom(command);
        }
    }
}
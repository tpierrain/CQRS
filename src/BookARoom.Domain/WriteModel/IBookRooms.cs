namespace BookARoom.Domain.WriteModel
{
    public interface IBookRooms
    {
        void BookARoom(BookARoomCommand bookingCommand);
    }
}
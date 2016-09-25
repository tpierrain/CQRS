namespace BookARoom.Domain
{
    public interface ISendCommands
    {
        void Send<T>(T command) where T : ICommand;
    }
}
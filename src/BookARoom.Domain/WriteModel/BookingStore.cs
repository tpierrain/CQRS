namespace BookARoom.Domain.WriteModel
{
    public class BookingStore : IBookingStore
    {
        private readonly IClientAndBookingRepository repository;

        public BookingStore(IClientAndBookingRepository repository)
        {
            this.repository = repository;
        }

        public void BookARoom(BookARoomCommand bookingRequest)
        {
            if (!this.repository.IsClientAlready(bookingRequest.ClientId))
            {
                this.repository.CreateClient(bookingRequest.ClientId);    
            }

            this.repository.Save(bookingRequest);
        }
    }
}

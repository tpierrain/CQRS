using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra.Web
{
    public class CompositionRootHelper
    {
        public static ReadModelFacade BuildTheReadModelHexagon(IProvideRooms roomsAdapter, IProvidePlaces placesAdapter)
        {
            return new ReadModelFacade(roomsAdapter, placesAdapter);
        }

        public static BookingCommandHandler BuildTheWriteModelHexagon(IBookingRepository bookingRepository, IClientRepository clientRepository, IPublishEvents bus)
        {
            return new BookingCommandHandler(new BookingStore(bookingRepository, clientRepository, bus));
        }
    }
}
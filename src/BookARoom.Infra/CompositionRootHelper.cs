using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;

namespace BookARoom.Infra
{
    /// <summary>
    /// Ease the integration of the various hexagons (one for the read model, the other for the write model).
    /// </summary>
    public class CompositionRootHelper
    {
        public static ReadModelFacade BuildTheReadModelHexagon(IProvideRooms roomsAdapter, IProvideHotel hotelAdapter)
        {
            return new ReadModelFacade(roomsAdapter, hotelAdapter);
        }

        public static BookingCommandHandler BuildTheWriteModelHexagon(IBookingRepository bookingRepository, IClientRepository clientRepository, IPublishEvents bus)
        {
            return new BookingCommandHandler(new BookingStore(bookingRepository, clientRepository, bus));
        }
    }
}
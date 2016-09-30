using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra.MessageBus;

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

        /// <summary>
        /// Subscribe the "command handler" to per-type command publication on the bus.
        /// </summary>
        /// <param name="bookingCommandHandler">The callback/handler provider.</param>
        /// <param name="bus">The bus to subscribe on.</param>
        public static void SubscribeCommands(BookingCommandHandler bookingCommandHandler, FakeBus bus)
        {
            bus.RegisterHandler<BookARoomCommand>(bookingCommandHandler.Handle);
        }
    }
}
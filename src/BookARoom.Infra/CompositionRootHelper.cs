using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;

namespace BookARoom.Infra
{
    /// <summary>
    /// Ease the integration of the various hexagons (one for the read model, the other for the write model).
    /// </summary>
    public class CompositionRootHelper
    {
        public static ReadModelFacade BuildTheReadModelHexagon(IProvideRooms roomsAdapter, IProvideHotel hotelAdapter, IProvideReservations reservationAdapter = null, ISubscribeToEvents bus = null)
        {
            if (bus == null)
            {
                bus = new FakeBus();
            }

            if (reservationAdapter == null)
            {
                reservationAdapter = new ReservationAdapter(bus);
            }

            return new ReadModelFacade(roomsAdapter, hotelAdapter, reservationAdapter, bus);
        }

        public static WriteModelFacade BuildTheWriteModelHexagon(ISaveBooking saveBooking, IHandleClients handleClients, IPublishEvents eventPublisher, ISubscribeToEvents eventSubscriber)
        {
            var writeModelCommandHandler = new WriteModelFacade(new BookingStore(saveBooking, handleClients, eventPublisher));
            CompositionRootHelper.SubscribeCommands(writeModelCommandHandler, eventSubscriber);

            return writeModelCommandHandler;
        }

        /// <summary>
        /// Subscribe the "command handler" to per-type command publication on the eventPublisher.
        /// </summary>
        /// <param name="writeModelFacade">The callback/handler provider.</param>
        /// <param name="bus">The eventPublisher to subscribe on.</param>
        private static void SubscribeCommands(WriteModelFacade writeModelFacade, ISubscribeToEvents bus)
        {
            bus.RegisterHandler<BookingCommand>(writeModelFacade.Handle);
        }
    }
}
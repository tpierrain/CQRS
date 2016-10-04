using BookARoom.Domain.WriteModel;
using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.WriteModel;
using NFluent;
using NUnit.Framework;
using System.Linq;
using BookARoom.Infra.ReadModel.Adapters;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class CancelBookingTests
    {
        [Test]
        public void Should_impact_booking_engine_when_CancelBookingCommand_is_sent()
        {
            var bookingEngine = new BookingAndClientsRepository();
            var bus = new FakeBus();
            CompositionRootHelper.BuildTheWriteModelHexagon(bookingEngine, bookingEngine, bus, bus);

            var hotelId = 2;
            var roomNumber = "101";
            var clientId = "thomas@pierrain.net";
            var bookingCommand = new BookingCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: hotelId, roomNumber: roomNumber, checkInDate: Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1));

            Check.That(bookingEngine.GetBookingsFrom(clientId)).IsEmpty();

            bus.Send(bookingCommand);

            Check.That(bookingEngine.GetBookingsFrom(clientId)).HasSize(1);
            var bookingGuid = bookingEngine.GetBookingsFrom(clientId).First().BookingId;

            var cancelBookingCommand = new CancelBookingCommand(bookingGuid, clientId);

            bus.Send(cancelBookingCommand);

            Check.That(bookingEngine.GetBookingsFrom(clientId)).HasSize(1);
            Check.That(bookingEngine.GetBookingsFrom(clientId).First().IsCanceled).IsTrue();
        }

    }
}
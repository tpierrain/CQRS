using System;
using BookARoom.Domain;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.WriteModel;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class CancelBookingTests
    {
        [Test]
        public void Should_Update_booking_engine_when_CancelBookingCommand_is_sent()
        {
            var bookingEngine = new BookingAndClientsRepository();
            var bus = new FakeBus();
            CompositionRootHelper.BuildTheWriteModelHexagon(bookingEngine, bookingEngine, bus, bus);

            var hotelId = 2;
            var roomNumber = "101";
            var clientId = "thomas@pierrain.net";
            var bookingCommand = new BookingCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: hotelId, roomNumber: roomNumber, checkInDate: Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1));
            
            bus.Send(bookingCommand);

            Check.That(bookingEngine.GetBookingCommandsFrom(clientId)).ContainsExactly(bookingCommand);

            var cancelBookingCommand = new CancelBookingCommand(bookingCommand.Guid);
            bus.Send(cancelBookingCommand);

            Check.That(bookingEngine.GetBookingCommandsFrom(clientId)).IsEmpty();
        }
    }

    public class CancelBookingCommand: ICommand
    {
        public Guid BookingCommandGuid { get; }

        public CancelBookingCommand(Guid bookingCommandGuid)
        {
            BookingCommandGuid = bookingCommandGuid;
        }
    }
}
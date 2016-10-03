# CQRS lab instructions

The objective of this lab is to add the cancel a reservation feature.

## Step1: Add an acceptance test

1. Create a new __CancelBookingTests__ test fixture within the BookARoom.Tests projects (within the 'Acceptance' directory)
2. Add a first failing test (__Should_Update_booking_engine_when_CancelBookingCommand_is_sent()__): 
````C#
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
            var bookingCommand = new BookARoomCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: hotelId, roomNumber: roomNumber, checkInDate: Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1));
            
            bus.Send(bookingCommand);

            Check.That(bookingEngine.GetBookingCommandsFrom(clientId)).ContainsExactly(bookingCommand);

            var cancelBookingCommand = new CancelBookingCommand(bookingCommand.Guid);
            bus.Send(cancelBookingCommand);

            Check.That(bookingEngine.GetBookingCommandsFrom(clientId)).IsEmpty();
        }
    }

    // Note: since we 'TDD as if you meant it', the newly created command sits aside the test (we'll move it in a second step).
    public class CancelBookingCommand: ICommand
    {
        public Guid BookingCommandGuid { get; }

        public CancelBookingCommand(Guid bookingCommandGuid)
        {
            BookingCommandGuid = bookingCommandGuid;
        }
    }
}
````
3. We make the test work
4. We refactor



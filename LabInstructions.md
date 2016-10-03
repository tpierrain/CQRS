# CQRS lab instructions

The objective of this lab is to add the cancel a reservation feature.

## Step1: Add a failing acceptance test

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


## Step2: Make it work

1.  We __move the CancelBookingCommand__ type to the proper project (__BookARoom.Domain\WriteModel__) in order for it to be referenced from within the domain logic.

2.  We __register a handler for this new command__. It means:
    1. To make the *WriteModelFacade* type implements: *IHandleCommand<CancelBookingCommand>
    * (with an implementation throwing a *System.NotImplementedException*)
    2. To make the CompositionRootHelper subscribes the proper handler for this CancelBookingCommand

    Here is the impact on the code:

    ````C#
    namespace BookARoom.Domain.WriteModel
    {
        public class WriteModelFacade : IHandleCommand<BookingCommand>, IHandleCommand<CancelBookingCommand>
        {
            // existing code

            public void Handle(CancelBookingCommand command)
            {
            throw new System.NotImplementedException();
        }
    }

    ````

    and

    ````C#
    /// <summary>
    /// Ease the integration of the various hexagons (one for the read model, the other for the write model).
    ///</summary>
    public class CompositionRootHelper
    {
        // existing code

        /// <summary>
        /// Subscribe the "command handler" to per-type command publication on the eventPublisher.
        /// </summary>
        /// <param name="writeModelFacade">The callback/handler provider.</param>
        /// <param name="bus">The eventPublisher to subscribe on.</param>
        private static void SubscribeCommands(WriteModelFacade writeModelFacade, ISubscribeToEvents bus)
        {
            bus.RegisterHandler<BookingCommand>(writeModelFacade.Handle);
            bus.RegisterHandler<CancelBookingCommand>(writeModelFacade.Handle); // the line to be added

        }

        // existing code
    }

    ````


3.  We replace the System.NotImplementedException of the (WriteModelFacade) handler by a code calling a method we create on-the-fly on the IBookRooms interface:

    ````C#
    // (...) somewhere within the WriteModelFacade type
    public void Handle(CancelBookingCommand command)
    {
    this.BookingStore.CancelBooking(command); // better than a NotImplementedException right?
    }
    ````

    ````C#
    namespace BookARoom.Domain.WriteModel
    {
    public interface IBookRooms
    {
    void BookARoom(BookingCommand bookingCommand);
    void CancelBooking(CancelBookingCommand cancelBookingCommand); // the new operation
    }
    }
    ````

    We then implement this method on the __BookingStore__ concrete type:
    
    ````C#
    using System;

    namespace BookARoom.Domain.WriteModel
    {
    public class BookingStore : IBookRooms
    {
    // existing code

    public void CancelBooking(CancelBookingCommand cancelBookingCommand)
    {
    var booking = this.bookingRepository.GetBooking(cancelBookingCommand.ClientId, cancelBookingCommand.BookingCommandGuid);

    if (booking.IsForClient(cancelBookingCommand.ClientId))
    {
    // We cancel the booking
    booking.Cancel();

    // And save its updated version
    this.bookingRepository.Update(booking);
    }
    else
    {
    throw new InvalidOperationException("Can't cancel a booking for another client.");
    }
    }

    // existing code
    }
    }
    ````

    Since this newly implemented *CancelBooking* method refers to undefined methods and type, __we need to catch-up the implementation__ in order to make it build again. It means:
    1. __To Add a new *ClientId* property__ on the existing *CancelBookingCommand* type (needed to prevent someone from cancelling someone else's booking) and to fix the acceptance test that uses it (*Should_Update_booking_engine_when_CancelBookingCommand_is_sent()*)
    2. __To Add 2 methods on the IBookingRepository interface__: *Booking GetBooking(string clientId, Guid bookingId)* and *void Update(Booking booking)*
    3. __To implement a new *Booking* type__ that has *Cancel()* and a *IsForClient(string clientId)* methods.
    4. To catch-up *IBookingRepository* implementation of the *BookingAndClientsRepository* concrete type __by adding the 2 missing methods__: GetBooking(...) and Update(...)

    Let's see what it takes with code:

    ````C#
    public class CancelBookingCommand: ICommand
    {
        public Guid BookingCommandGuid { get; }
        public string ClientId { get; } // new property

        public CancelBookingCommand(Guid bookingCommandGuid, string clientId)
        {
            this.BookingCommandGuid = bookingCommandGuid;
            this.ClientId = clientId; // new property assignment
        }
    }
    ````
    with the acceptance test

    ````C#
    [Test]
    public void Should_Update_booking_engine_when_CancelBookingCommand_is_sent()
    {
        // existing code
        var cancelBookingCommand = new CancelBookingCommand(bookingCommand.Guid, clientId); // add the new clientId parameter
        // existing code
    }
    ````

    then
    ````C#
    namespace BookARoom.Domain.WriteModel
    {
        public interface IBookingRepository
        {
            Guid Save(BookingCommand bookingCommand);
            Booking GetBooking(string clientId, Guid bookingId); // new method
            void Update(Booking booking); // new method
        }
    }
    ````
    and
    ````C#
    namespace BookARoom.Domain.WriteModel
    {
        public class Booking // new type!
        {
            public bool IsForClient(string clientId)
            {
                throw new NotImplementedException();
            }

            public void Cancel()
            {
                throw new NotImplementedException();
            }
        }
    }
    ````
    and
    ````C#
    namespace BookARoom.Infra.WriteModel
    {
        public class BookingAndClientsRepository : IBookingRepository, IClientRepository
        {
            // existing code

            public Booking GetBooking(string clientId, Guid bookingId)
            {
                throw new NotImplementedException();
            }

            public void Update(Booking booking)
            {
                throw new NotImplementedException();
            }
        }
    }
    ````
    __From now, it should compile again ;-)__ and the *Should_Update_booking_engine_when_CancelBookingCommand_is_sent()* test should now fail due to one of the many NotImplementedException we left on the field.

    - - -
    Time to implement all these behaviours by replacing every NotImplementedException by some code. Follow the white rabbit here...

    We start with the implementation of the *GetBooking(string clientId, Guid bookingId)* method on the BookingAndClientsRepository type:

    ````C#
    namespace BookARoom.Infra.WriteModel
    {
        public class BookingAndClientsRepository : IBookingRepository, IClientRepository
        {
            private readonly Dictionary<string, List<ICommand>> perClientCommands;

            // existing code

            public Booking GetBooking(string clientId, Guid bookingId)
            {
                var allCommandsForThisClient = this.perClientCommands[clientId];
                foreach (var command in allCommandsForThisClient)
                {
                    var bookingCommand = command as BookingCommand;
                    if (bookingCommand != null && bookingCommand.Guid == bookingId)
                    {
                        return new Booking(bookingCommand.ClientId, bookingCommand.HotelId, bookingCommand.RoomNumber, bookingCommand.CheckInDate, bookingCommand.CheckOutDate);
                    }
                }

                return Booking.Null;
            }

            // existing code
        }
    }
    ````
    which by the way, force us to add a __Null (object pattern)__ property on the new *Booking* type that we need to implement to get rid of its previous NotImplementedExceptions:

    ````C#
    public class Booking
    {
        public static Booking Null { get; } = new NullBooking();

        // We provide getters only so that the state of this domain object is only changed via one of its operations (methods)
        public Guid BookingId { get; }
        public string ClientId { get; }
        public int HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public bool IsCanceled { get; private set; }

        public Booking(Guid bookingId , string clientId, int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.BookingId = bookingId;
            this.ClientId = clientId;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }

        public virtual bool IsForClient(string clientId)
        {
            if (this.ClientId == clientId)
            {
                return true;
            }

            return false;
        }

        public virtual void Cancel()
        {
            this.IsCanceled = true;
        }

        private class NullBooking : Booking
        {
            public NullBooking() : base(Guid.Empty, string.Empty, 0, string.Empty, DateTime.Now, DateTime.Now)
            {
            }

            public override bool IsForClient(string clientId)
            {
                return false;
            }

            public override void Cancel()
            {
            }
        }
    }
    ````
    we continue to replace all our NotImplementedException by some code. Next-one pointed out by our acceptance test is the *Update()* method of the *BookingAndClientsRepository* type:




    - - -



4. We refactor



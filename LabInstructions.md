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
            var bookingCommand = new BookingCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: hotelId, roomNumber: roomNumber, checkInDate: Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1));

            bus.Send(bookingCommand);

            Check.That(bookingEngine.GetBookingsFrom(clientId)).HasSize(1);
            var bookingGuid = bookingEngine.GetBookingsFrom(clientId).First().BookingId;

            var cancelBookingCommand = new CancelBookingCommand(bookingGuid);
            bus.Send(cancelBookingCommand);

            // Booking is still there, but canceled
            Check.That(bookingEngine.GetBookingsFrom(clientId)).HasSize(1);
            Check.That(bookingEngine.GetBookingsFrom(clientId).First().IsCanceled).IsTrue();
        }
    }

    // Note: since we 'TDD as if you meant it', the newly created command sits aside the test (we'll move it in a second step).
    public class CancelBookingCommand: ICommand
    {
        public Guid BookingId { get; }

        public CancelBookingCommand(Guid bookingId)
        {
            this.BookingId = bookingId;
        }
    }
}
````

To make our solution build again, we must create (Alt-Enter) a new *IsCancelled* property for the *Booking* type :

````C#
public class Booking
{
    // existing code

    public bool IsCanceled { get; private set; }  // a new property to identify booking cancelation

    // existing code
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
    }
    ````

    and

    ````C#
            /// <summary>
            /// Ease the integration of the various hexagons (one for the read model, the other for the write model).
            /// </summary>
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
                    var booking = this.bookingRepository.GetBooking(cancelBookingCommand.ClientId, cancelBookingCommand.BookingId);
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
    
    3. __To implement new methods on the *Booking* type__ (*Cancel()* and *IsForClient(string clientId)*)
    
    4. To catch-up *IBookingRepository* implementation of the *BookingAndClientsRepository* concrete type __by adding the 2 missing methods__: GetBooking(...) and Update(...)

    Let's see what it takes with code:

    ````C#
        public class CancelBookingCommand: ICommand
        {
            public Guid BookingId { get; }
            public string ClientId { get; } // new property

            public CancelBookingCommand(Guid bookingId, string clientId)
            {
                this.BookingId = bookingId;
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
            var cancelBookingCommand = new CancelBookingCommand(bookingGuid, clientId);
            // existing code
        }
    ````

    then
    
    ````C#
        namespace BookARoom.Domain.WriteModel
        {
            public interface IBookingRepository
            {
                void Save(Booking booking);
                Booking GetBooking(string clientId, Guid bookingId);
                void Update(Booking booking);
            }
        }
    ````
    
    and
    
    ````C#
        namespace BookARoom.Domain.WriteModel
        {
            public class Booking
            {
                // existing code

                public bool IsForClient(string clientId)
                {
                    throw new NotImplementedException();
                }

                public void Cancel()
                {
                    throw new NotImplementedException();
                }

                // existing code
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
using System;

namespace BookARoom.Domain.WriteModel
{
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
}
````

we continue to replace all our NotImplementedException by some code. Next-one pointed out by our acceptance test is the *Update()* method of the *BookingAndClientsRepository* type:


````C#
public void Update(Booking booking)
{
    if (!this.perClientBookings.ContainsKey(booking.ClientId))
    {
        this.perClientBookings[booking.ClientId] = new List<Booking>();
    }

    var bookingsForThisClient = this.perClientBookings[booking.ClientId];

    int? index = null;
    for (int i = 0; i < bookingsForThisClient.Count; i++)
    {
        if (bookingsForThisClient[i].BookingId == booking.BookingId)
        {
            index = i;
            break;
        }
    }

    if (index.HasValue)
    {
        bookingsForThisClient[index.Value] = booking;
    }
}
````

We run our tests again, and TADA! it's all green.

- - -

## Step3: Write a failing acceptance test showing that a 'CancelBooking' task updates the "My Reservations" read model

Here it is:
````C#
[Test]
public void Should_Update_readmodel_user_reservations_when_CancelBookingCommand_is_sent()
{
    var bookingEngine = new BookingAndClientsRepository();
    var bus = new FakeBus(synchronousPublication:true);
    CompositionRootHelper.BuildTheWriteModelHexagon(bookingEngine, bookingEngine, bus, bus);

    var hotelsAndRoomsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
    hotelsAndRoomsAdapter.LoadAllHotelsFiles();
    var reservationAdapter = new ReservationAdapter(bus);
    CompositionRootHelper.BuildTheReadModelHexagon(hotelsAndRoomsAdapter, hotelsAndRoomsAdapter, reservationAdapter, bus);

    var clientId = "thomas@pierrain.net";
    Check.That(reservationAdapter.GetReservationsFor(clientId)).IsEmpty();

    var hotelId = 2;
    var roomNumber = "101";
    var bookingCommand = new BookingCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: hotelId, roomNumber: roomNumber, checkInDate: Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1));

    bus.Send(bookingCommand);

    var bookingGuid = bookingEngine.GetBookingsFrom(clientId).First().BookingId;

    Check.That(reservationAdapter.GetReservationsFor(clientId)).HasSize(1);

    var reservation = reservationAdapter.GetReservationsFor(clientId).First();
    Check.That(reservation.RoomNumber).IsEqualTo(roomNumber);
    Check.That(reservation.HotelId).IsEqualTo(hotelId.ToString());

    var cancelCommand = new CancelBookingCommand(bookingGuid, clientId);
    bus.Send(cancelCommand);

    Check.That(reservationAdapter.GetReservationsFor(clientId)).HasSize(0);
}

````

So far, we've added the concept of cancelation for the *Booking* (on the write-side). Now, it's time to add the same notion on the read-side (*Reservation*) to make this test compile.

````C#
    namespace BookARoom.Domain.ReadModel
    {
        public class Reservation
        {
            // existing code

            public bool IsCanceled { get; private set; } // NEW PROP

            // existing code
        }
    }
````


## Step4: Make it work

For that, __the write-side domain logic will have to triggering a *BookingCanceled* event that will impact the read-side model__ (reservations to begin, rooms availabilities in a second time).

To know where to raise event, we follow the white rabbit from the main CommandHandler (i.e. the WriteModelFacade) to the *BookingStore.CancelBooking(cmd)* method: bingo! this is it.

````C#
    using System;

    namespace BookARoom.Domain.WriteModel
    {
        public class BookingStore : IBookRooms
        {
            // existing code

            public void CancelBooking(CancelBookingCommand command)
            {
                var booking = this.bookingRepository.GetBooking(command.BookingGuid, command.ClientId);
                if (booking.IsForClient(command.ClientId))
                {
                    booking.Cancel();

                    this.bookingRepository.Update(booking);

                    // HERE, WE INSTANTIATE AND PUBLISH A BRAND NEW EVENT --------------
                    var bookingCanceled = new BookingCanceled(booking.ClientId, booking.BookingId);
                    this.publishEvents.PublishTo(bookingCanceled);
                    // THE EVENT HAS BEEN PUBLISHED ------------------------------------
                }
                else
                {
                    throw new InvalidOperationException("Can't cancel someone else booking.");
                }
            }

            // existing code
        }
    }

````

Alt-Enter on the red BookingCanceled type and we create it on-the-fly:

````C#
    using System;

    namespace BookARoom.Domain.WriteModel
    {
        public class BookingCanceled : IEvent
        {
            public string ClientId { get; } // immutable
            public Guid BookingId { get; }

            public BookingCanceled(string clientId, Guid bookingId)
            {
                this.ClientId = clientId;
                this.BookingId = bookingId;
            }
        }
    }
````

Now, we just have to make the *ReservationAdapter* to subscribe this new event. This is achieved within its constructor where we declare a new callback handler to be used. Like this:

````C#
    namespace BookARoom.Infra.ReadModel.Adapters
    {
        public class ReservationAdapter : IProvideReservations
        {
            private readonly ISubscribeToEvents eventsSubscriber;
            private readonly Dictionary<string, List<Reservation>> perClientReservations = new Dictionary<string, List<Reservation>>();

            public ReservationAdapter(ISubscribeToEvents eventsSubscriber)
            {
                this.eventsSubscriber = eventsSubscriber;

                // subscribes to the events
                this.eventsSubscriber.RegisterHandler<RoomBooked>(Handle);
                this.eventsSubscriber.RegisterHandler<BookingCanceled>(Handle); // NEW EVENT SUBSCRIPTION
            }

            private void Handle(BookingCanceled @event) // NEW CALLBACK
            {
                // Find the reservation made by this client and declares it Canceled
                var reservationsForThisClient = this.perClientReservations[@event.ClientId];
                foreach (var reservation in reservationsForThisClient)
                {
                    if (reservation.Guid == @event.BookingId)
                    {
                        reservation.Cancel();
                    }
                }
            }

            // existing code
        }
    }
````
Time for us to Implement the Cancel() method on the *Reservation* type and Voila! The acceptance test is GREEN ;)

````C#
    namespace BookARoom.Domain.ReadModel
    {
        public class Reservation
        {

            // existing code

            public void Cancel()  // NEW METHOD
            {
                this.IsCanceled = true;
            }

            // existing code
        }
    }
````


## Step5: Write a failing acceptance test showing that a 'CancelBooking' task impacts the "rooms search engine" on the read-side

Provided here, in a non-refactored form to ease Copy-and-Paste (but will deserve some extract methods in your IDE):

````C#
    [Test]
    public void Should_impact_room_search_results_when_CancelBookingCommand_is_sent()
    {
        // Initialize Read-model side
        var bus = new FakeBus(synchronousPublication: true);
        var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
        var reservationsAdapter = new ReservationAdapter(bus);
        hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

        // Initialize Write-model side
        var bookingRepository = new BookingAndClientsRepository();
        CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus, bus);

        var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter, reservationsAdapter, bus);

        // Search Rooms availabilities
        var checkInDate = Constants.MyFavoriteSaturdayIn2017;
        var checkOutDate = checkInDate.AddDays(1);

        var searchQuery = new SearchBookingOptions(checkInDate, checkOutDate, location: "New York", numberOfAdults: 2);
        var bookingOptions = readFacade.SearchBookingOptions(searchQuery);

        // We should get 1 booking option with 13 available rooms in it.
        Check.That(bookingOptions).HasSize(1);

        var bookingOption = bookingOptions.First();
        var initialRoomsNumbers = 13;
        Check.That(bookingOption.AvailableRoomsWithPrices).HasSize(initialRoomsNumbers);

        // Now, let's book that room!
        var firstRoomOfThisBookingOption = bookingOption.AvailableRoomsWithPrices.First();
        var clientId = "thomas@pierrain.net";
        var bookingCommand = new BookingCommand(clientId: clientId, hotelName: "New York Sofitel", hotelId: bookingOption.Hotel.Identifier, roomNumber: firstRoomOfThisBookingOption.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);

        // We send the BookARoom command
        bus.Send(bookingCommand);

        // We check that both the BookingRepository (Write model) and the available rooms (Read model) have been updated.
        Check.That(bookingRepository.GetBookingsFrom(clientId).Count()).IsEqualTo(1);
        var bookingId = bookingRepository.GetBookingsFrom(clientId).First().BookingId;

        // Fetch rooms availabilities now. One room should have disappeared from the search result
        bookingOptions = readFacade.SearchBookingOptions(searchQuery);
        Check.That(bookingOptions).HasSize(1);
        Check.That(bookingOption.AvailableRoomsWithPrices).As("available matching rooms").HasSize(initialRoomsNumbers - 1);

        // We cancel our booking
        var cancelBookingCommand = new CancelBookingCommand(bookingId, clientId);
        bus.Send(cancelBookingCommand);

        // Search again and the missing room should be back on the search result again
        bookingOptions = readFacade.SearchBookingOptions(searchQuery);
        Check.That(bookingOptions).HasSize(1);
        Check.That(bookingOption.AvailableRoomsWithPrices).As("available matching rooms").HasSize(initialRoomsNumbers - 1 + 1);
    }
````
## Step6: Make it work

As we previously did for the *ReservationAdapter*, let's make the *HotelsAndRoomsAdapter* (implementing *IProvideRooms*) subscribes to the *BookingCanceled* event.

````C#
    public class HotelsAndRoomsAdapter : IProvideRooms, IProvideHotel
    {
        private readonly ISubscribeToEvents eventsSubscriber;
        private readonly IStoreAndProvideHotelsAndRooms repository;

        public HotelsAndRoomsAdapter(string integrationFilesDirectoryPath, ISubscribeToEvents eventsSubscriber)
        {
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
            this.repository = new HotelsAndRoomsRepository();

            this.eventsSubscriber = eventsSubscriber;
            this.eventsSubscriber.RegisterHandler<RoomBooked>(this.Handle);
            this.eventsSubscriber.RegisterHandler<BookingCanceled>(this.Handle); // NEW EVENT REGISTRATION
        }

        // NEW CALLBACK FOR BOOKING CANCELED EVENT
        private void Handle(BookingCanceled bookingCanceled)
        {
            throw new NotImplementedException();
        }

        // existing code
    }
````

For once, I'll let you implement it alone to make the last acceptance test turn to green.
As you will see, there is something more to do since the read-model will need some extra information in order to be able to make the corresponding room appeard again.

Will you need to add extra information from the source event?
Instead, will you need to request those informations on the read-model side (near to the need)?

As you have probably already understand, __there is no one size fits all CQRS architecture__. Only trade-offs and options to adjust to your domain needs and constraints.


- - -

## Step7: Integrate our work to the (Web site) UI

TODO with a DELETE VERB.

- - -

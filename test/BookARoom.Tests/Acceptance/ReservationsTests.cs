using System.Collections.Generic;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;
using BookARoom.Infra.WriteModel;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class ReservationsTests
    {
        [Test]
        public void Should_retrieve_updated_list_of_reservations()
        {
            var bus = new FakeBus();
            var adapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
            adapter.LoadHotelFile("New York Sofitel-availabilities.json");
            var reservationAdapter = new ReservationAdapter(bus);
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(adapter, adapter, reservationAdapter, bus);

            var clientId = "thomas@pierrain.net";
            IEnumerable<Reservation> reservations = readFacade.GetReservationsFor(clientId);
            Check.That(reservations).IsEmpty();

            var bookingAndClientsRepository = new BookingAndClientsRepository();
            CompositionRootHelper.BuildTheWriteModelHexagon(bookingAndClientsRepository, bookingAndClientsRepository, bus, bus);

            var hotelId = 1;
            var hotelName = "New York Sofitel";
            var roomNumber = "101";
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);
            bus.Send(new BookingCommand(clientId, hotelName, hotelId, roomNumber, checkInDate, checkOutDate));

            reservations = readFacade.GetReservationsFor(clientId);
            Check.That(reservations).HasSize(1);
            var reservation = reservations.First();

            Check.That(reservation.ClientId).IsEqualTo(clientId);
            Check.That(reservation.HotelId).IsEqualTo(hotelId.ToString()); // TODO: make the hotelId a string and not an int.
            Check.That(reservation.RoomNumber).IsEqualTo(roomNumber);
            Check.That(reservation.CheckInDate).IsEqualTo(checkInDate);
            Check.That(reservation.CheckOutDate).IsEqualTo(checkOutDate);
        }
    }
}

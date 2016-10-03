using BookARoom.Domain;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class BookingConfirmationController : Controller
    {
        private readonly ISendCommands bus;

        public BookingConfirmationController(ISendCommands bus)
        {
            this.bus = bus;
        }

        // GET: /<controller>/

        [HttpPost]
        public IActionResult Index(BookingRequestViewModel viewModel)
        {
            if (!string.IsNullOrWhiteSpace(viewModel.ClientMail))
            {
                // Create the task and send it to the bus
                var bookingCommand = new BookingCommand(clientId: viewModel.ClientMail, hotelName: viewModel.HotelName, hotelId: int.Parse(viewModel.HotelId), roomNumber: viewModel.RoomId, checkInDate: viewModel.CheckInDate, checkOutDate: viewModel.CheckOutDate);
                this.bus.Send(bookingCommand);

                viewModel.BookingSucceeded = true;
            }
            else
            {
                viewModel.BookingSucceeded = false;
            }

            return View(viewModel);
        }
    }
}

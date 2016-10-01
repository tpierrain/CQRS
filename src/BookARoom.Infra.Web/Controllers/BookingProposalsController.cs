using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class BookingProposalsController : Controller
    {
        private ISendCommands bus;
        private readonly IQueryBookingProposals searchService;

        public BookingProposalsController(ISendCommands bus, IQueryBookingProposals searchService)
        {
            this.bus = bus;
            this.searchService = searchService;
        }

        // GET: /<controller>/
        public IActionResult Index(BookingProposalsViewModel bookingProposalsViewModel)
        {
            // Why da heck is my bookingProposalsViewModel.Proposals always null here... I can't figure it out yet ;-(
            return View(bookingProposalsViewModel);
        }

        [HttpPost]
        public IActionResult Index(SearchRoomQueryViewModel queryViewModel)
        {
            var searchQuery = new SearchBookingProposal(queryViewModel.CheckInDate, queryViewModel.CheckOutDate, queryViewModel.Destination, queryViewModel.NumberOfAdults);
            var searchResult = this.searchService.SearchBookingProposals(searchQuery);

            var bookingProposalsViewModel = new BookingProposalsViewModel(queryViewModel.Destination, searchResult);

            bookingProposalsViewModel.Location = searchQuery.Location;

            return View(bookingProposalsViewModel);
        }
    }
}

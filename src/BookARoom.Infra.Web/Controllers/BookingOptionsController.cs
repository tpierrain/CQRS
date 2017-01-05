using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class BookingOptionsController : Controller
    {
        private readonly IQueryBookingOptions searchService;

        public BookingOptionsController(IQueryBookingOptions searchService)
        {
            this.searchService = searchService;
        }

        // GET: /<controller>/
        public IActionResult Index(BookingOptionsViewModel bookingOptionsViewModel)
        {
            return View(bookingOptionsViewModel);
        }

        [HttpPost]
        public IActionResult Index(SearchRoomQueryViewModel queryViewModel)
        {
            var searchQuery = new SearchBookingOptions(queryViewModel.CheckInDate, queryViewModel.CheckOutDate, queryViewModel.Destination, queryViewModel.NumberOfAdults);
            var searchResult = this.searchService.SearchBookingOptions(searchQuery);

            var bookingOptionsViewModel = new BookingOptionsViewModel(queryViewModel, queryViewModel.Destination, searchResult);

            bookingOptionsViewModel.Location = searchQuery.Location;
             
            return View(bookingOptionsViewModel);
        }
    }
}

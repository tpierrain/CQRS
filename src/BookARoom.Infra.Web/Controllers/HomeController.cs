using System;
using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookARoom.Infra.Web.Controllers
{
    public class HomeController : Controller
    {
        private ISendCommands bus;
        private readonly IQueryBookingProposals searchService;

        public HomeController(ISendCommands bus, IQueryBookingProposals searchService)
        {
            this.bus = bus;
            this.searchService = searchService;
        }

        public IActionResult Index()
        {
            var defaultCheckInDate = new DateTime(2017, 09, 16);
            var defaultSearchQuery = new SearchRoomQueryViewModel("Budapest", defaultCheckInDate, defaultCheckInDate.AddDays(1));
            return View(defaultSearchQuery);
        }

        [HttpPost]
        public IActionResult Index(SearchRoomQueryViewModel queryViewModel)
        {
            // instantiate a command here and post it to the bus
            var adultsCount = 2; // TODO: hide this default value within the web form

            var searchQuery = new SearchBookingProposal(queryViewModel.CheckInDate, queryViewModel.CheckOutDate, queryViewModel.Destination, adultsCount);
            var searchResult = this.searchService.SearchBookingProposals(searchQuery);

            var bookingProposalsViewModel = new BookingProposalsViewModel(queryViewModel.Destination, searchResult);

            bookingProposalsViewModel.Location = searchQuery.Location;

            return RedirectToAction("Index", "BookingProposals", bookingProposalsViewModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

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
            var defaultSearchQuery = new SearchRoomQueryViewModel("Budapest", defaultCheckInDate, defaultCheckInDate.AddDays(1), numberOfAdults:2);
            return View(defaultSearchQuery);
        }

        [HttpPost]
        public IActionResult Index(SearchRoomQueryViewModel queryViewModel)
        {
            var searchQuery = new SearchBookingProposal(queryViewModel.CheckInDate, queryViewModel.CheckOutDate, queryViewModel.Destination, queryViewModel.NumberOfAdults);
            var searchResult = this.searchService.SearchBookingProposals(searchQuery);

            var bookingProposalsViewModel = new BookingProposalsViewModel(queryViewModel.Destination, searchResult);

            bookingProposalsViewModel.Location = searchQuery.Location;

            //return View(bookingProposalsViewModel) // note: I wish I could do this, but all I found is the following RedirectToAction (which seems to make an http roundtrip and does not transfer properly the bookingProposalsViewModel instance ;-( I still miss a thing in my ASP.NET MVC understanding ;-)
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

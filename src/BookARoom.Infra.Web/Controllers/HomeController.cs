using System;
using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookARoom.Infra.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueryBookingOptions searchService;

        public HomeController(ISendCommands bus, IQueryBookingOptions searchService)
        {
            this.searchService = searchService;
        }

        public IActionResult Index()
        {
            ViewData["Message"] = "Search a room";

            var defaultCheckInDate = new DateTime(2017, 09, 16);
            var defaultSearchQuery = new SearchRoomQueryViewModel("Budapest", defaultCheckInDate, defaultCheckInDate.AddDays(1), numberOfAdults:2);
            return View(defaultSearchQuery);
        }

        
        public IActionResult About()
        {
            ViewData["Message"] = "What is BookARoom?";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Wanna know more?";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

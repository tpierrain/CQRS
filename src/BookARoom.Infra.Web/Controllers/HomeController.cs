using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View();
        }

        [HttpPost]
        public IActionResult Index(SearchRoomViewModel viewModel)
        {
            // instantiate a command here and post it to the bus
            var adultsCount = 2; // TODO: hide this default value within the web form

            var searchQuery = new SearchBookingProposalQuery(viewModel.CheckInDate, viewModel.CheckOutDate, viewModel.Destination, adultsCount);
            
            // TODO: see what's the recommendation regarding the ASP.NET core threading model
            //this.bus.Send(searchQuery);
            var searchResult = this.searchService.SearchBookingProposals(searchQuery);

            return View();
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

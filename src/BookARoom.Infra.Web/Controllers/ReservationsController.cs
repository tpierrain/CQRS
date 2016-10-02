using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IProvideReservations reservationsProvider;

        public ReservationsController(IProvideReservations reservationsProvider)
        {
            this.reservationsProvider = reservationsProvider;
        }

        // GET: /<controller>/
        public IActionResult Index(ReservationsViewModel reservationsViewModel)
        {
            return View(reservationsViewModel);
        }

        [HttpPost]
        public IActionResult Index(QueryReservationsViewModel queryReservationsViewModel)
        {
            var reservations = this.reservationsProvider.GetReservationsFor(queryReservationsViewModel.ClientMail);

            var reservationsViewModel = new ReservationsViewModel(queryReservationsViewModel.ClientMail, reservations);

            return View(reservationsViewModel);
        }
    }
}

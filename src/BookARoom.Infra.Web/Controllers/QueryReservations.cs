using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class QueryReservations : Controller
    {
        public IActionResult Index(string email)
        {
            ViewData["Message"] = "Consult your reservations";
            return View(new QueryReservationsViewModel(email));
        }
    }
}

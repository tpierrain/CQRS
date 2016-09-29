using BookARoom.Infra.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BookARoom.Infra.Web.Controllers
{
    public class BookingProposalsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index(BookingProposalsViewModel bookingProposalsViewModel)
        {

            return View(bookingProposalsViewModel);
        }
    }
}

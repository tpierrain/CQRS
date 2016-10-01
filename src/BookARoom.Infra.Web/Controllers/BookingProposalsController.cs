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
            // Why da heck is my bookingProposalsViewModel.Proposals always null here... I can't figure it out yet ;-(
            return View(bookingProposalsViewModel);
        }
    }
}

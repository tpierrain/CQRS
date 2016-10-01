using System.Collections.Generic;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingOptionsViewModel
    {
        public SearchRoomQueryViewModel SearchCriterias { get; set; }
        public BookingRequestViewModel BookingRequest { get; set; }
        public string Location { get; set; }
        public IEnumerable<BookingOption> Options { get; set; }
        

        public BookingOptionsViewModel()
        {
        }

        public BookingOptionsViewModel(SearchRoomQueryViewModel searchCriterias, string location, IEnumerable<BookingOption> Options)
        {
            this.SearchCriterias = searchCriterias;
            this.Location = location;
            this.Options = Options;
        }
    }
}